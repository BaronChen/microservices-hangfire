using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire.EventBus
{
    public interface IEventHandlerRegistry
    {
        bool AddHandler<T>(string eventName, Type type);

        IEventHandler<T> GetHandler<T>(string eventName);
    }
}
