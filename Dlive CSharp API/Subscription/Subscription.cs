using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DSharp.Subscription.Chat;
using DSharp.Subscription.Chest;
using DSharp.Utility;

namespace DSharp.Subscription
{
    public static class Subscription
    {
        public static bool IsConnected { get; private set; }

        public static event Action OnConnected;
        public static event Action<string> OnDisconnected;
        public static event Action<string> OnError;
        public static event Action<ChatMessage> OnChatEvent;
        public static event Action<ChestMessage> OnChestEvent;

        private static CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private static ClientWebSocket _socket = new ClientWebSocket();

        private static bool _isAlive;

        [Obsolete("This method is not yet implemented. Use Subscribe for now")]
        public static SubscriptionData SubscribeByDisplayName(string displayName, SubscriptionType subscriptionType = SubscriptionType.CHAT)
        {
            throw new NotImplementedException("This method is not yet implemented. Use Subscribe for now");
        }

        public static SubscriptionData Subscribe(string streamerUsername, SubscriptionType subscriptionType = SubscriptionType.CHAT)
        {
            if (!IsConnected)
            {
                _socket.Options.AddSubProtocol("graphql-ws");

                Connect().Wait();
            }

            List<string> ids = new List<string>();
            
            switch (subscriptionType)
            {
                case SubscriptionType.CHAT:
                    return new SubscriptionData(SubscribeChat(streamerUsername).Result);
                case SubscriptionType.CHEST:
                    return new SubscriptionData(chestEventId: SubscribeChest(streamerUsername).Result);
                case SubscriptionType.ALL:
                    return new SubscriptionData(SubscribeChat(streamerUsername).Result, SubscribeChest(streamerUsername).Result);
                default:
                    return new SubscriptionData();
            }
        }

        private static async Task Connect()
        {
            byte[] messageBuffer = new byte[128];

            await _socket.ConnectAsync(Dlive.SubscriptionEndpoint, CancellationToken.None);

            messageBuffer = Encoding.ASCII.GetBytes("{\"type\":\"connection_init\"}");

            await _socket.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
            messageBuffer = new byte[128];
            await _socket.ReceiveAsync(new ArraySegment<byte>(messageBuffer), CancellationToken.None);

            dynamic response = JsonConvert.DeserializeObject(Encoding.ASCII.GetString(messageBuffer));
            if (response.type.ToString() != "connection_ack")
            {
                string error = response.payload.message.ToString();
                await _socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, error, CancellationToken.None);
                OnError?.Invoke(error);
                throw new WebSocketConnectionRefusedException($"The connection was refused by remote host with reason: {error}");
            }

            Receive().ConfigureAwait(false);
            IsConnected = true;
            _isAlive = true;
            OnConnected?.Invoke();
        }

        private static async Task KeepAliveCheck()
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
                    await SafeDisconnect("Lost connection to remote host");
                    await Connect();
                }

                _isAlive = false;
            } while (IsConnected);
        }

        private static async Task SafeDisconnect(string reason)
        {
            // Currently unused
            _cancellationToken.Cancel();
            await Task.Delay(5000);
            Console.WriteLine("Closing socket");
            await _socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, reason, CancellationToken.None);
            Console.WriteLine("Done");
            IsConnected = false;
            OnDisconnected?.Invoke(reason);
        }

        private static async Task Receive()
        {
            do
            {
                byte[] messageBuffer = new byte[4096];
                await _socket.ReceiveAsync(new ArraySegment<byte>(messageBuffer), _cancellationToken.Token);
                string debug = Encoding.ASCII.GetString(messageBuffer);// Sometimes we get an unreadable message?
                Task.Run(() => ParseMessage(JsonConvert.DeserializeObject(debug))).ConfigureAwait(false);
            } while (IsConnected);
        }

        private static void ParseMessage(dynamic response)
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

        private static void BuildChatMessage(string id, dynamic data)
        {
            Enum.TryParse(data[0].type.ToString().ToUpper(), out ChatEventType type);

            Console.WriteLine(data[0].type.ToString());
            
            switch (type)
            {
                case ChatEventType.MESSAGE:
                    OnChatEvent?.Invoke(new ChatTextMessage(id, data[0].content.ToString(), Util.UserObjectToPublicUserData(data[0].sender)));
                    break;
                case ChatEventType.GIFT:
                    Enum.TryParse(data[0].gift.ToString(), out GiftType giftType);
                    OnChatEvent?.Invoke(new ChatGiftMessage(id, giftType, int.Parse(data[0].amount.ToString()), data[0].message.ToString(), Util.UserObjectToPublicUserData(data[0].sender)));
                    break;
                case ChatEventType.SUBSCRIPTION:
                    OnChatEvent?.Invoke(new ChatSubscriptionMessage(id, int.Parse(data[0].month.ToString()), Util.UserObjectToPublicUserData(data[0].sender)));
                    break;
                case ChatEventType.HOST:
                    OnChatEvent?.Invoke(new ChatHostMessage(id, int.Parse(data[0].viewer.ToString()), Util.UserObjectToPublicUserData(data[0].sender)));
                    break;
                case ChatEventType.CHAT_MODE:
                    Enum.TryParse(data[0].mode.ToString(), out ChatMode mode);
                    OnChatEvent?.Invoke(new ChatModeChangeMessage(id, mode, Util.UserObjectToPublicUserData(data[0].sender)));
                    break;
                case ChatEventType.BAN:
                    OnChatEvent?.Invoke(new ChatBanMessage(id, Util.UserObjectToPublicUserData(data[0].sender), Util.UserObjectToPublicUserData(data[0].bannedBy)));
                    break;
                case ChatEventType.MOD:
                    ModeratorStatusChange change = bool.Parse(data[0].add.ToString()) ? ModeratorStatusChange.PROMOTED : ModeratorStatusChange.DEMOTED;
                    OnChatEvent?.Invoke(new ChatModStatusChangeMessage(id, change, Util.UserObjectToPublicUserData(data[0].sender)));
                    break;
                case ChatEventType.EMOTE:
                    OnChatEvent?.Invoke(new ChatSubscriptionMessage(id, data[0].emote.ToString(), Util.UserObjectToPublicUserData(data[0].sender)));
                    break;
                case ChatEventType.TIMEOUT:
                    OnChatEvent?.Invoke(new ChatTimeoutMessage(id, int.Parse(data[0].minute.ToString()), Util.UserObjectToPublicUserData(data[0].sender), Util.UserObjectToPublicUserData(data[0].bannedBy)));
                    break;
                case ChatEventType.CLIP:
                    OnChatEvent?.Invoke(new ChatClipMessage(id, Util.UserObjectToPublicUserData(data[0].sender), new Uri($"https://dlive.tv/clip/{data[0].url.ToString()}")));
                    break;
                case ChatEventType.GIFTSUB:
                    Console.WriteLine(data[0]);
                    int months;

                    if (!int.TryParse(data[0].count.ToString(), out months))
                        months = 1;

                    OnChatEvent?.Invoke(new ChatGiftSubscriptionMessage(id, months, Util.UserObjectToPublicUserData(data[0].sender), Util.UserObjectToPublicUserData(data[0].receiver)));
                    break;
                default:
                    object user = data[0].sender;
                    OnChatEvent?.Invoke(user != null ? new UserChatMessage(type, id, Util.UserObjectToPublicUserData(data[0].sender)): new ChatMessage(type, id));
                    break;
            }
        }

        private static void BuildChestMessage(string id, dynamic data)
        {
            Enum.TryParse(data.type.ToString().ToUpper(), out ChestEventType type);

            Console.WriteLine(data.type.ToString());

            switch (type)
            {
                case ChestEventType.GIVEAWAYSTARTED://1568841544000
                    OnChestEvent?.Invoke(new ChestGiveawayStartedMessage(id, float.Parse(data.pricePool.ToString()) / 100000, int.Parse(data.durationInSeconds.ToString()), Util.EpocMSToDateTime(double.Parse(data.endTime.ToString()))));
                    break;
                case ChestEventType.VALUEEXPIRED:
                    OnChestEvent?.Invoke(new ChestValueExpiredMessage(id, float.Parse(data.value.ToString()) / 100000, Util.EpocMSToDateTime(double.Parse(data.endTime.ToString()))));
                    break;
                case ChestEventType.VALUEUPDATED:
                    OnChestEvent?.Invoke(new ChestValueUpdatedMessage(id, float.Parse(data.value.ToString()) / 100000));
                    break;
                default:
                    OnChestEvent?.Invoke(new ChestMessage(type, id));
                    break;
            }
        }

        private static async Task<string> SubscribeChat(string username)
        {
            string id = $"{ username }_chat";
            byte[] messageBuffer = new byte[512];
            
            messageBuffer = Encoding.ASCII.GetBytes($"{{\"id\":\"{id}\",\"type\":\"start\",\"payload\":{{\"query\":\"subscription{{streamMessageReceived(streamer:\\\"{username}\\\"){{__typename}}}}\"}}}}");
            await _socket.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, CancellationToken.None);

            return id;
        }

        private static async Task<string> SubscribeChest(string username)
        {
            string id = $"{ username }_chest";
            byte[] messageBuffer = new byte[512];

            messageBuffer = Encoding.ASCII.GetBytes($"{{\"id\":\"{id}\",\"type\":\"start\",\"payload\":{{\"query\":\"subscription{{treasureChestMessageReceived(streamer:\\\"{username}\\\"){{__typename}}}}\"}}}}");
            await _socket.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, CancellationToken.None);

            return id;
        }
    }
}
