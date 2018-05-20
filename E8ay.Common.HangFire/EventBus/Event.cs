using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire.EventBus
{
    public class Event<T>
    {
        public string EventName { get; set; }

        public IEnumerable<string> TargetQueues { get; set; } = new List<string>();

        public T Data { get; set; }
    }
}
