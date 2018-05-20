using System;
using System.Collections.Generic;
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
            var client = new BackgroundJobClient();
            foreach (var queueName in e.TargetQueues)
            {
                var state = new EnqueuedState("your-queue");

                client.Create(() => _eventProcessor.Process(e), state);
            }
            
        }
    }
}
