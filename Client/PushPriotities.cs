namespace Floxdc.ExponentServerSdk
{
    /// <summary>
    /// The delivery priority of the message. Specify "default" or omit this field 
    /// to use the default priority on each platform, which is "normal" on Android 
    /// and "high" on iOS. 
    /// 
    /// On Android, normal-priority messages won't open network connections on 
    /// sleeping devices and their delivery may be delayed to conserve the battery. 
    /// High-priority messages are delivered immediately if possible and may wake 
    /// sleeping devices to open network connections, consuming energy. 
    /// 
    /// On iOS, normal-priority messages are sent at a time that takes into account 
    /// power considerations for the device, and may be grouped and delivered in 
    /// bursts. They are throttled and may not be delivered by Apple. High-priority 
    /// messages are sent immediately. Normal priority corresponds to APNs priority 
    /// level 5 and high priority to 10. 
    /// </summary>
    public enum PushPriotities
    {
        None = 0,
        Default = 1,
        Normal = 2,
        High = 3
    }
}
