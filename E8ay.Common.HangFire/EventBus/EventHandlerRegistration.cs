using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire.EventBus
{
    public class EventHandlerRegistration
    {
        private Dictionary<string, IEventHandler> _register = new Dictionary<string, IEventHandler>();

        private EventHandlerRegistration()
        {
        }

        public static EventHandlerRegistration Create()
        {
            return new EventHandlerRegistration();
        }

        public bool AddHandler(string eventName, IEventHandler handler)
        {
            return _register.TryAdd(eventName, handler);
        }
    }
}
