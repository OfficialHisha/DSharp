# D#
An unofficial Dlive.tv API written in C#

Example usage of the D# API

## Getting chat events
(There are more events than shown, but I picked out the events I think are most commonly used)
```CSharp
using System;
using DSharp;
using DSharp.Subscription;
using DSharp.Subscription.Chat;

namespace DSharpExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            Subscription.Subscribe("your blockchain username");
            Subscription.OnChatEvent += OnChatMessage;
        }

        static void OnChatMessage(ChatMessage message)
        {
            switch (message.EventType)
            {
                case ChatEventType.MESSAGE:
                    ChatTextMessage textMessage = message as ChatTextMessage;
                    Console.WriteLine($"[{DateTime.Now}] {textMessage.User.Displayname}: {textMessage.Content}");
                    break;
                case ChatEventType.GIFT:
                    ChatGiftMessage giftMessage = message as ChatGiftMessage;
                    Console.WriteLine($"[{DateTime.Now}] {giftMessage.User.Displayname} just gifted a {giftMessage.GiftType}");
                    break;
                case ChatEventType.FOLLOW:
                    UserChatMessage followMessage = message as UserChatMessage;
                    Console.WriteLine($"[{DateTime.Now}] {followMessage.User.Displayname} just followed");
                    break;
                case ChatEventType.SUBSCRIPTION:
                    ChatSubscriptionMessage subMessage = message as ChatSubscriptionMessage;
                    Console.WriteLine($"[{DateTime.Now}] {subMessage.User.Displayname} just subscribed for {subMessage.Months} months");
                    break;
                case ChatEventType.GIFTSUB:
                    ChatGiftSubscriptionMessage gSubMessage = message as ChatGiftSubscriptionMessage;
                    Console.WriteLine($"[{DateTime.Now}] {gSubMessage.GiftingUser.Displayname} just gifted {gSubMessage.Months} months of subscription to {gSubMessage.ReceivingUser.Displayname}");
                    break;
                case ChatEventType.HOST:
                    ChatHostMessage hostMessage = message as ChatHostMessage;
                    Console.WriteLine($"[{DateTime.Now}] {hostMessage.User.Displayname} just hosted with {hostMessage.Viewers} viewers");
                    break;
            }
        }
    }
}
```
## Sending chat messages
(Again, there are many more mutations than just sending messages, but I believe this is the most common use case)
```CSharp
using DSharp;
using DSharp.Mutation;

namespace DSharpExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            Dlive.AuthorizationToken = "Your user token";

            Mutation.SendChatMessage("stream page for the message", "the message");
        }
    }
}
```

## Getting user data
```CSharp
using DSharp;
using DSharp.Query;

namespace DSharpExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            Dlive.AuthorizationToken = "Your user token";

            UserData info = Query.GetMyInfo();
            PublicUserData publicInfo = info.Public;
            PrivateUserData privateInfo = info.Private;

            System.Console.WriteLine($"Private user info: {info.Private}");
            System.Console.WriteLine($"Public user info: {info.Public}");

            // GetMyInfo returns both your public and private info
            // and can be accessed as shown above

            // The following queries does not require authorization
            PublicUserData publicUser = Query.GetPublicInfo("blockchain username");
            System.Console.WriteLine(publicUser.Displayname);

            PublicUserData alsoPublicUser = Query.GetPublicInfoByDisplayname("Dlive displayname");
            System.Console.WriteLine(alsoPublicUser.Displayname);

            // Both methods return the same (public) data, the examples are just to show that you can access the
            // information in multiple ways
        }
    }
}
```
