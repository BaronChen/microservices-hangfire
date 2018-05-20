using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire.EventBus
{
    public class EventProcessor : IEventProcessor
    {
        private EventHandlerRegistry _registry;

        public EventProcessor(EventHandlerRegistry eventHandlerRegistry)
        {
            _registry = eventHandlerRegistry;
        }

        public void Process<T>(Event<T> e)
        {
            var handler = _registry.GetHandler(e.EventName);
            
            handler?.Handle(e);
        }
    }
}
