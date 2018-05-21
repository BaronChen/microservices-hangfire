using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire.EventBus
{
    internal class EventHandlerRegistry: IEventHandlerRegistry
    {
        private readonly IServiceProvider _serviceProvider;

        private Dictionary<string, Type> _register = new Dictionary<string, Type>();

        public EventHandlerRegistry(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public bool AddHandler<T>(string eventName, Type type)
        {
            if (!typeof(IEventHandler<T>).IsAssignableFrom(type))
            {
                throw new ApplicationException("Handler must implements IEventHandler interface");
            }

            return _register.TryAdd(eventName, type);
        }

        public IEventHandler<T> GetHandler<T>(string eventName)
        {
            Type type;
            if (_register.TryGetValue(eventName, out type))
            {
                var handler = _serviceProvider.GetService(type);
                return (IEventHandler<T>)handler;
            }
            else
            {
                return null;
            }
        }
    }
}
