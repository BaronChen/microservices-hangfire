using E8ay.Common.HangFire.EventData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Common.HangFire.EventBus
{
    internal class EventProcessor : IEventProcessor
    {
        private IEventHandlerRegistry _registry;

        public EventProcessor(IEventHandlerRegistry eventHandlerRegistry)
        {
            _registry = eventHandlerRegistry;
        }

        public async Task Process<T>(Event<T> e) where T : IEventData
        {
            var handler = _registry.GetHandler<T>(e.EventName);

            if (handler != null)
            {
                await handler.Handle(e);
            }
        }
    }
}
