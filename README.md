# D#
An unofficial Dlive.tv API written in C#

Example usage of the D# API

## Getting chat events
(There are more events than shown, but I picked out the events I think are most commonly used)
```CSharp
using System;
using DSharp.Dlive;
using DSharp.Dlive.Subscription;
using DSharp.Dlive.Subscription.Chat;

namespace DSharpExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            Subscription myChatSubscription = Subscription.SubscriptionByDisplayName(myAccountDisplayName);
            // or
            Subscription myAlternativChatSubscription = new Subscription(myAccountUsername);
            
	    myChatSubscription.OnChatEvent += OnChatMessage;
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
using DSharp.Dlive;

namespace DSharpExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            DliveAccount myAccount = new DliveAccount("Your Dlive user token");

            myAccount.Mutation.SendChatMessage("channelToSendMessageTo", "Message to send");
        }
    }
}
```

All mutations require usernames to be supplied where identifiers are needed.
In order to easily convert a display name into a username, the following utility method can be used.

```CSharp
using DSharp.Utility;

namespace DSharpExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            string displayName = Util.DliveUsernameToDisplayName("blockchain username");
            // or the other way
            string username = Util.DliveDisplayNameToUsername("Dlive display name");
        }
    }
}
```


## Getting user data
```CSharp
using DSharp.Dlive;
using DSharp.Dlive.Query;

namespace DSharpExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            DliveAccount myAccount = new DliveAccount("Your Dlive user token");

            UserData myAccountInfo = myAccount.Query.GetMyInfo();
			
	    // Get your follower count
            Console.WriteLine(myAccountInfo.Public.NumFollowers);
            
            // Get your subscriber count
            Console.WriteLine(myAccountInfo.Private.SubscriberCount);

            // GetMyInfo returns both your public and private info
            // and can be accessed as shown above

            // It is also possible to use public queries without using an account
	    PublicUserData publicUser = PublicQuery.GetPublicInfoByDisplayname("Dlive display name");
	    // or
	    PublicUserData alternativePublicUser = PublicQuery.GetPublicInfo("blockchain username");

            // Both methods return the same (public) data, the examples are just to show that you can access the
            // information in multiple ways
        }
    }
}
```
