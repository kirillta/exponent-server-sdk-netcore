# exponent-server-sdk-netcore #

Server-side library for working with Expo using .Net Core 2.0+.
This is a direct port of [exponent-server-sdk-python](https://github.com/expo/exponent-server-sdk-python)


### Installation ###

```
PM> Install-Package ExponentServerSdk -Version 1.1.2
```


### Usage ###

The push notifications [documentation](https://docs.expo.io/versions/latest/guides/push-notifications.html) is available on the expo site.


```cs
using Floxdc.ExponentServerSdk;

...

public async Task<PushResponse> SendNotification()
{
    const string expoToken = "ExponentPushToken[*****]";
    var client = new PushClient();
    var notification = new PushMessage(expoToken, title: "A new message from your friend");

    try
    {
        return await client.Publish(notification);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        throw;
    }
}
```


