using E8ay.Common.HangFire.EventData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Common.HangFire.EventBus
{
    public interface IEventProcessor
    {
        Task Process<T>(Event<T> e) where T : IEventData;
    }
}
