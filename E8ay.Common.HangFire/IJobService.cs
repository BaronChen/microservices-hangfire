using E8ay.Common.HangFire.EventBus;
using E8ay.Common.HangFire.EventData;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire
{
    public interface IJobService
    {
        void PublishEvent<T>(Event<T> e) where T : IEventData;

        void PublishDelayedEvent<T>(Event<T> e, TimeSpan delay) where T : IEventData;
    
    }
}
