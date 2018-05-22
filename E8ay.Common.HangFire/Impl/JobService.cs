using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using E8ay.Common.HangFire.EventBus;
using E8ay.Common.HangFire.EventData;
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
        
        public void PublishEvent<T>(Event<T> e) where T : IEventData
        {
            ValidateEvent(e);

            var client = new BackgroundJobClient();
            foreach (var queueName in e.TargetQueues)
            {
                var state = new EnqueuedState(queueName);

                client.Create(() => _eventProcessor.Process(e), state);
            }
            
        }

        public void PublishDelayedEvent<T>(Event<T> e, TimeSpan delay) where T : IEventData
        {
            ValidateEvent(e);

            BackgroundJob.Schedule(() => PublishEvent<T>(e), delay);
        }

        private void ValidateEvent<T>(Event<T> e) where T : IEventData
        {
            if (e.TargetQueues.Count() == 0)
            {
                throw new ApplicationException("Event should target at least one queue");
            }

            if (string.IsNullOrEmpty(e.EventName))
            {
                throw new ApplicationException("Event name cannot be empty");
            }
        }

    }
}
