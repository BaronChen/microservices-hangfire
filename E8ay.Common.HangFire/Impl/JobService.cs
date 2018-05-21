using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using E8ay.Common.HangFire.EventBus;
using Hangfire;
using Hangfire.States;

namespace E8ay.Common.HangFire.Impl
{
    internal class JobService : IJobService
    {
        private IEventProcessor _eventProcessor;

        public JobService(IEventProcessor eventProcessor)
        {
            _eventProcessor = eventProcessor;
        }
        
        public void PublishEvent<T>(Event<T> e)
        {
            if (e.TargetQueues.Count() == 0)
            {
                throw new ApplicationException("Event should target at least one queue");
            }

            if (string.IsNullOrEmpty(e.EventName))
            {
                throw new ApplicationException("Event name cannot be empty");
            }

            var client = new BackgroundJobClient();
            foreach (var queueName in e.TargetQueues)
            {
                var state = new EnqueuedState(queueName);

                client.Create(() => _eventProcessor.Process(e), state);
            }
            
        }
    }
}
