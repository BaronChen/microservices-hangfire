using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Common.Pusher
{
    public interface IPusherManager
    {
        Task PushNotification(string channel, string pusherEventName, object data);
    }
}
