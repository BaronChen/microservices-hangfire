using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire.EventBus
{
    internal class EventProcessor : IEventProcessor
    {
        private IEventHandlerRegistry _registry;

        public EventProcessor(IEventHandlerRegistry eventHandlerRegistry)
        {
            _registry = eventHandlerRegistry;
        }

        public void Process<T>(Event<T> e)
        {
            var handler = _registry.GetHandler<T>(e.EventName);
            
            handler?.Handle(e);
        }
    }
}
