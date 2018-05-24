using E8ay.Common.HangFire.EventBus;
using E8ay.Common.HangFire.EventData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using System.Linq;

namespace E8ay.Common.HangFire.Test.EventBus
{
    [TestClass]
    public class EventProcessorTest
    {
        private IEventHandler<TestEventData> _mockHandler;
        private IEventHandlerRegistry _mockRegistry;
        private EventProcessor _processor;

        [TestInitialize]
        public void Init()
        {
            _mockRegistry = Substitute.For<IEventHandlerRegistry>();
            _mockHandler = Substitute.For<IEventHandler<TestEventData>>();
            _processor = new EventProcessor(_mockRegistry);
        }

        [TestMethod]
        public async Task Should_Process_Event()
        {
            _mockRegistry.GetHandler<TestEventData>(TestEventData.EventName).Returns(_mockHandler);
            var @event = new Event<TestEventData>() { EventName = TestEventData.EventName };
            await _processor.Process(@event);
            await _mockHandler.Received(1).Handle(@event);
        }

        [TestMethod]
        public void Should_Process_Nothing_If_No_Handler_Exist_For_Event()
        {
            _mockRegistry.GetHandler<TestEventData>(TestEventData.EventName).Returns<object>(null);
            var @event = new Event<TestEventData>() { EventName = TestEventData.EventName };
            Should.NotThrow(async () => await _processor.Process(@event));
            
        }
    }
}