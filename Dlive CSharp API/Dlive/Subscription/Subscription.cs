using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DSharp.Dlive.Subscription.Chat;
using DSharp.Dlive.Subscription.Chest;
using DSharp.Utility;
using DSharp.Dlive.Query;

namespace DSharp.Dlive.Subscription
{
    public class Subscription
    {
        public SubscriptionType Type { get; }
        public bool IsConnected { get; private set; }

        public event Action OnConnected;
        public event Action<string> OnDisconnected;
        public event Action<string> OnError;
        public event Action<ChatMessage> OnChatEvent;
        public event Action<ChestMessage> OnChestEvent;
        
        public string ChatId { get; private set; }
        public string ChestId { get; private set; }

        private readonly string _username;

        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private ClientWebSocket _socket = new ClientWebSocket();

        private bool _isAlive;

        public Subscription(string streamerUsername, SubscriptionType subscriptionType = SubscriptionType.CHAT)
        {
            Type = subscriptionType;
            _socket.Options.AddSubProtocol("graphql-ws");
            _username = streamerUsername;
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
                Console.Error.WriteLine($"The connection was refused by remote host with reason: {error}");
            }

            Receive();
            IsConnected = true;
            _isAlive = true;

            switch (Type)
            {
                case SubscriptionType.CHAT:
                    SubscribeChat().Wait();
                    break;
                case SubscriptionType.CHEST:
                    SubscribeChest().Wait();
                    break;
                case SubscriptionType.ALL:
                    SubscribeChat().Wait();
                    SubscribeChest().Wait();
                    break;
                default:
                    break;
            }

            OnConnected?.Invoke();
            KeepAliveCheck();
        }

        private async Task KeepAliveCheck()
        {
            do
            {
                await Task.Delay(25000);

                if (!_isAlive)
                {
                    Console.Error.WriteLine("Lost connection to remote host");
                    OnError?.Invoke("Lost connection to remote host");
                    OnDisconnected?.Invoke("Lost connection to remote host");
                    IsConnected = false;
                    //Disconnect("Lost connection to remote host");
                    //Connect();
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
                Task.Run(() => ParseMessage(JsonConvert.DeserializeObject(Encoding.ASCII.GetString(messageBuffer))));
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
            Enum.TryParse(data[0].roomRole.ToString().ToUpper(), out RoomRole roomRole);

            #if DEBUG
                Console.WriteLine(data[0].type.ToString());
                Console.WriteLine(data[0].ToString());
            #endif

            switch (type)
            {
                case ChatEventType.MESSAGE:
                    OnChatEvent?.Invoke(new ChatTextMessage(channel, data[0].id.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender), roomRole, bool.Parse(data[0].subscribing.ToString()), data[0].content.ToString(), long.Parse(data[0].subLength.ToString())));
                    break;
                case ChatEventType.GIFT:
                    Enum.TryParse(data[0].gift.ToString(), out GiftType giftType);
                    OnChatEvent?.Invoke(new ChatGiftMessage(channel, data[0].id.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender), roomRole, bool.Parse(data[0].subscribing.ToString()), giftType, int.Parse(data[0].amount.ToString()), data[0].message.ToString()));
                    break;
                case ChatEventType.SUBSCRIPTION:
                    OnChatEvent?.Invoke(new ChatSubscriptionMessage(channel, data[0].id.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender), roomRole, bool.Parse(data[0].subscribing.ToString()), int.Parse(data[0].month.ToString())));
                    break;
                case ChatEventType.HOST:
                    OnChatEvent?.Invoke(new ChatHostMessage(channel, data[0].id.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender), roomRole, bool.Parse(data[0].subscribing.ToString()), int.Parse(data[0].viewer.ToString())));
                    break;
                case ChatEventType.CHAT_MODE:
                    Enum.TryParse(data[0].mode.ToString(), out ChatMode mode);
                    OnChatEvent?.Invoke(new ChatModeChangeMessage(channel, data[0].id.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender), roomRole, bool.Parse(data[0].subscribing.ToString()), mode));
                    break;
                case ChatEventType.BAN:
                    Enum.TryParse(data[0].bannedByRoomRoleToString().ToUpper(), out RoomRole bannedByRoomRole);
                    OnChatEvent?.Invoke(new ChatBanMessage(channel, data[0].id.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender), roomRole, Util.DliveUserObjectToPublicUserData(data[0].bannedBy), bannedByRoomRole));
                    break;
                case ChatEventType.MOD:
                    ModeratorStatusChange change = bool.Parse(data[0].add.ToString()) ? ModeratorStatusChange.PROMOTED : ModeratorStatusChange.DEMOTED;
                    OnChatEvent?.Invoke(new ChatModStatusChangeMessage(channel, data[0].id.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender), roomRole, bool.Parse(data[0].subscribing.ToString()), change));
                    break;
                case ChatEventType.EMOTE:
                    OnChatEvent?.Invoke(new ChatEmoteMessage(channel, data[0].id.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender), roomRole, bool.Parse(data[0].subscribing.ToString()), data[0].emote.ToString()));
                    break;
                case ChatEventType.TIMEOUT:
                    Enum.TryParse(data[0].bannedByRoomRoleToString().ToUpper(), out RoomRole timedoutByRoomRole);
                    OnChatEvent?.Invoke(new ChatTimeoutMessage(channel, data[0].id.ToString(), int.Parse(data[0].minute.ToString()), Util.DliveUserObjectToPublicUserData(data[0].sender), roomRole, Util.DliveUserObjectToPublicUserData(data[0].bannedBy), timedoutByRoomRole));
                    break;
                case ChatEventType.CLIP:
                    OnChatEvent?.Invoke(new ChatClipMessage(channel, data[0].id.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender), roomRole, bool.Parse(data[0].subscribing.ToString()), new Uri($"https://dlive.tv/clip/{data[0].url.ToString()}")));
                    break;
                case ChatEventType.GIFTSUB:
                    if (!int.TryParse(data[0].count.ToString(), out int months))
                        months = 1;

                    OnChatEvent?.Invoke(new ChatGiftSubscriptionMessage(channel, data[0].id.ToString(), months, Util.DliveUserObjectToPublicUserData(data[0].sender), PublicQuery.GetPublicInfoByDisplayName(data[0].receiver.ToString()), roomRole));
                    break;
                default:
                    object user = data[0].sender;
                    OnChatEvent?.Invoke(user != null ? new UserChatMessage(type, channel, data[0].id.ToString(), Util.DliveUserObjectToPublicUserData(data[0].sender), roomRole, (bool)data[0].subscribing.ToString()): new ChatMessage(type, channel, data[0].id.ToString()));
                    break;
            }
        }

        private void BuildChestMessage(string channel, dynamic data)
        {
            Enum.TryParse(data.type.ToString().ToUpper(), out ChestEventType type);

            #if DEBUG
                Console.WriteLine(data.type.ToString());
                Console.WriteLine(data.ToString());
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

        private async Task SubscribeChat()
        {
            string id = $"{_username}_chat";
            byte[] messageBuffer = new byte[512];
            
            messageBuffer = Encoding.ASCII.GetBytes($"{{\"id\":\"{id}\",\"type\":\"start\",\"payload\":{{\"query\":\"subscription{{streamMessageReceived(streamer:\\\"{_username}\\\"){{__typename}}}}\"}}}}");
            await _socket.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
            ChatId = id;
        }

        private async Task SubscribeChest()
        {
            string id = $"{_username}_chest";
            byte[] messageBuffer = new byte[512];

            messageBuffer = Encoding.ASCII.GetBytes($"{{\"id\":\"{id}\",\"type\":\"start\",\"payload\":{{\"query\":\"subscription{{treasureChestMessageReceived(streamer:\\\"{_username}\\\"){{__typename}}}}\"}}}}");
            await _socket.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
            ChestId = id;
        }
    }
}
