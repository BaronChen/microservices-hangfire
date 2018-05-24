using E8ay.Common.HangFire.EventBus;
using E8ay.Common.HangFire.EventData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Common.HangFire.Test.EventBus
{
    public class TestEventData : IEventData
    {
        public static string EventName = "TEST_EVENT";
    }

    public class TestEventHandler : IEventHandler<TestEventData>
    {
        public Task Handle(Event<TestEventData> e)
        {
            throw new NotImplementedException();
        }
    }
}
