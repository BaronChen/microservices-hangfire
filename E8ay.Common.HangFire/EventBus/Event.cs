using E8ay.Common.HangFire.EventData;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire.EventBus
{
    public class Event<T> where T : IEventData
    {
        public string EventName { get; set; }

        public IList<string> TargetQueues { get; set; } = new List<string>();

        public T Data { get; set; }
    }
}
