using E8ay.Common.HangFire.EventData;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire.EventBus
{
    public interface IEventHandlerRegistry
    {
        bool AddHandler<T>(string eventName, Type type) where T : IEventData;

        IEventHandler<T> GetHandler<T>(string eventName) where T : IEventData;
    }
}
