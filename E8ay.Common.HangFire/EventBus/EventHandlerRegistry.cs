using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire.EventBus
{
    public class EventHandlerRegistry
    {
        private readonly IServiceProvider _serviceProvider;

        private Dictionary<string, Type> _register = new Dictionary<string, Type>();

        private EventHandlerRegistry(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public bool AddHandler(string eventName, Type type)
        {
            if (!typeof(IEventHandler).IsAssignableFrom(type))
            {
                throw new ApplicationException("Handler must implements IEventHandler interface");
            }

            return _register.TryAdd(eventName, type);
        }

        public IEventHandler GetHandler(string eventName)
        {
            Type type;
            if (_register.TryGetValue(eventName, out type))
            {
                var handler = _serviceProvider.GetService(type);
                return (IEventHandler)handler;
            }
            else
            {
                return null;
            }
        }
    }
}
