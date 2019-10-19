using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DSharp.Dlive.Query;
using Newtonsoft.Json;
using DSharp.Dlive.Subscription.Chat;
using DSharp.Dlive.Subscription.Chest;
using DSharp.Subscription;
using DSharp.Utility;

namespace DSharp.Dlive.Subscription
{
    public class Subscription
    {
        public static bool IsConnected { get; private set; }

        public event Action OnConnected;
        public event Action<string> OnDisconnected;
        public event Action<string> OnError;
        public event Action<ChatMessage> OnChatEvent;
        public event Action<ChestMessage> OnChestEvent;
        
        public string ChatId { get; private set; }
        public string ChestId { get; private set; }

        private static CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private static ClientWebSocket _socket = new ClientWebSocket();

        private static bool _isAlive;

        public Subscription(string streamerUsername, SubscriptionType subscriptionType = SubscriptionType.CHAT)
        {
            _socket.Options.AddSubProtocol("graphql-ws");
        }

        public static Subscription SubscriptionByDisplayName(string streamerDisplayName,
            SubscriptionType subscriptionType = SubscriptionType.CHAT)
        {
            return new Subscription(Util.DliveDisplayNameToUsername(streamerDisplayName), subscriptionType);
        }

        public void Connect()
        {
            if (IsConnected)
            {
                return;
            }
            
            byte[] messageBuffer = new byte[128];

            _socket.ConnectAsync(Dlive.SubscriptionEndpoint, CancellationToken.None).Wait();

            messageBuffer = Encoding.ASCII.GetBytes("{\"type\":\"connection_init\"}");

            _socket.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
            messageBuffer = new byte[128];
            _socket.ReceiveAsync(new ArraySegment<byte>(messageBuffer), CancellationToken.None).Wait();

            dynamic response = JsonConvert.DeserializeObject(Encoding.ASCII.GetString(messageBuffer));
            if (response.type.ToString() != "connection_ack")
            {
                string error = response.payload.message.ToString();
                _socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, error, CancellationToken.None).Wait();
                OnError?.Invoke(error);
                throw new WebSocketConnectionRefusedException($"The connection was refused by remote host with reason: {error}");
            }

            Receive().ConfigureAwait(false);
            IsConnected = true;
            _isAlive = true;
            OnConnected?.Invoke();
        }

        private async Task KeepAliveCheck()
        {
            // Currently unused
            do
            {
                await Task.Delay(2000);

                Console.WriteLine($"Keep alive check: {_isAlive}");

                if (!_isAlive)
                {
                    Console.WriteLine("Connection lost");
                    OnError?.Invoke("Lost connection to remote host");
                    Disconnect("Lost connection to remote host");
                    Connect();
                }

                _isAlive = false;
            } while (IsConnected);
        }
        
         private void Disconnect(string reason)
        {
            // Currently unused
            _cancellationToken.Cancel();
            Task.Delay(5000).Wait();
            _socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, reason, CancellationToken.None).Wait();
            IsConnected = false;
            OnDisconnected?.Invoke(reason);
        }

        private async Task Receive()
        {
            do
            {
                byte[] messageBuffer = new byte[4096];
                await _socket.ReceiveAsync(new ArraySegment<byte>(messageBuffer), _cancellationToken.Token);
                string debug = Encoding.ASCII.GetString(messageBuffer);// Sometimes we get an unreadable message?
                Task.Run(() => ParseMessage(JsonConvert.DeserializeObject(debug))).ConfigureAwait(false);
            } while (IsConnected);
        }

        private void ParseMessage(dynamic response)
        {
            switch (response.type.ToString())
            {
                case "ka":
                    _isAlive = true;
                    break;
                case "data":
                    if (response.id.ToString().Contains("_chat"))
                        // Chat event
                        BuildChatMessage(response.id.ToString(), response.payload.data.streamMessageReceived);
                    else
                        //Chest event
                        BuildChestMessage(response.id.ToString(), response.payload.data.treasureChestMessageReceived);
                    break;
                default:
                    //Unknown type
                    Console.WriteLine("Unknown message, please report on GitHub");
                    Console.WriteLine(response);
                    break;
            }
        }

        private void BuildChatMessage(string channel, dynamic data)
        {
            Enum.TryParse(data[0].type.ToString().ToUpper(), out ChatEventType type);

            #if DEBUG
                Console.WriteLine(data[0].type.ToString());
            #endif
            
            switch (type)
            {
                case ChatEventType.MESSAGE:
                    OnChatEvent?.Invoke(new ChatTextMessage(channel, data[0].id.ToString(), data[0].content.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender)));
                    break;
                case ChatEventType.GIFT:
                    Enum.TryParse(data[0].gift.ToString(), out GiftType giftType);
                    OnChatEvent?.Invoke(new ChatGiftMessage(channel, data[0].id.ToString(), giftType, int.Parse(data[0].amount.ToString()), data[0].message.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender)));
                    break;
                case ChatEventType.SUBSCRIPTION:
                    OnChatEvent?.Invoke(new ChatSubscriptionMessage(channel, data[0].id.ToString(), int.Parse(data[0].month.ToString()), Util.DliveUserObjectToPublicUserData(data[0].sender)));
                    break;
                case ChatEventType.HOST:
                    OnChatEvent?.Invoke(new ChatHostMessage(channel, data[0].id.ToString(), int.Parse(data[0].viewer.ToString()), Util.DliveUserObjectToPublicUserData(data[0].sender)));
                    break;
                case ChatEventType.CHAT_MODE:
                    Enum.TryParse(data[0].mode.ToString(), out ChatMode mode);
                    OnChatEvent?.Invoke(new ChatModeChangeMessage(channel, data[0].id.ToString(), mode, Util.DliveUserObjectToPublicUserData(data[0].sender)));
                    break;
                case ChatEventType.BAN:
                    OnChatEvent?.Invoke(new ChatBanMessage(channel, data[0].id.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender), Util.DliveUserObjectToPublicUserData(data[0].bannedBy)));
                    break;
                case ChatEventType.MOD:
                    ModeratorStatusChange change = bool.Parse(data[0].add.ToString()) ? ModeratorStatusChange.PROMOTED : ModeratorStatusChange.DEMOTED;
                    OnChatEvent?.Invoke(new ChatModStatusChangeMessage(channel, data[0].id.ToString(), change, Util.DliveUserObjectToPublicUserData(data[0].sender)));
                    break;
                case ChatEventType.EMOTE:
                    OnChatEvent?.Invoke(new ChatSubscriptionMessage(channel, data[0].id.ToString(), data[0].emote.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender)));
                    break;
                case ChatEventType.TIMEOUT:
                    OnChatEvent?.Invoke(new ChatTimeoutMessage(channel, data[0].id.ToString(), int.Parse(data[0].minute.ToString()), Util.DliveUserObjectToPublicUserData(data[0].sender), Util.DliveUserObjectToPublicUserData(data[0].bannedBy)));
                    break;
                case ChatEventType.CLIP:
                    OnChatEvent?.Invoke(new ChatClipMessage(channel, data[0].id.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender), new Uri($"https://dlive.tv/clip/{data[0].url.ToString()}")));
                    break;
                case ChatEventType.GIFTSUB:
                    Console.WriteLine(data[0]);
                    int months;

                    if (!int.TryParse(data[0].count.ToString(), out months))
                        months = 1;

                    OnChatEvent?.Invoke(new ChatGiftSubscriptionMessage(channel, data[0].id.ToString(), months, Util.DliveUserObjectToPublicUserData(data[0].sender), Util.DliveUserObjectToPublicUserData(data[0].receiver)));
                    break;
                default:
                    object user = data[0].sender;
                    OnChatEvent?.Invoke(user != null ? new UserChatMessage(type, channel, data[0].id.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender)): new ChatMessage(type, channel, data[0].id.ToString()));
                    break;
            }
        }

        private void BuildChestMessage(string channel, dynamic data)
        {
            Enum.TryParse(data.type.ToString().ToUpper(), out ChestEventType type);

            #if DEBUG
                Console.WriteLine(data.type.ToString());
            #endif

            switch (type)
            {
                case ChestEventType.GIVEAWAYSTARTED:
                    OnChestEvent?.Invoke(new ChestGiveawayStartedMessage(channel, float.Parse(data.pricePool.ToString()) / 100000, int.Parse(data.durationInSeconds.ToString()), Util.EpocMSToDateTime(double.Parse(data.endTime.ToString()))));
                    break;
                case ChestEventType.VALUEEXPIRED:
                    OnChestEvent?.Invoke(new ChestValueExpiredMessage(channel, float.Parse(data.value.ToString()) / 100000, Util.EpocMSToDateTime(double.Parse(data.endTime.ToString()))));
                    break;
                case ChestEventType.VALUEUPDATED:
                    OnChestEvent?.Invoke(new ChestValueUpdatedMessage(channel, float.Parse(data.value.ToString()) / 100000));
                    break;
                default:
                    OnChestEvent?.Invoke(new ChestMessage(type, channel));
                    break;
            }
        }

        private async Task SubscribeChat(string username)
        {
            string id = $"{username}_chat";
            byte[] messageBuffer = new byte[512];
            
            messageBuffer = Encoding.ASCII.GetBytes($"{{\"id\":\"{id}\",\"type\":\"start\",\"payload\":{{\"query\":\"subscription{{streamMessageReceived(streamer:\\\"{username}\\\"){{__typename}}}}\"}}}}");
            await _socket.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
            ChatId = id;
        }

        private async Task SubscribeChest(string username)
        {
            string id = $"{username}_chest";
            byte[] messageBuffer = new byte[512];

            messageBuffer = Encoding.ASCII.GetBytes($"{{\"id\":\"{id}\",\"type\":\"start\",\"payload\":{{\"query\":\"subscription{{treasureChestMessageReceived(streamer:\\\"{username}\\\"){{__typename}}}}\"}}}}");
            await _socket.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
            ChestId = id;
        }
    }
}
