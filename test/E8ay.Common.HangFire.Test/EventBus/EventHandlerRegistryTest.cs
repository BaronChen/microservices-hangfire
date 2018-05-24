using E8ay.Common.HangFire.EventBus;
using E8ay.Common.HangFire.EventData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace E8ay.Common.HangFire.Test.EventBus
{
    [TestClass]
    public class EventHandlerRegistryTest
    {
        private IServiceProvider _mockServiceProvider;
        private EventHandlerRegistry _registry;

        [TestInitialize]
        public void Init()
        {
            _mockServiceProvider = Substitute.For<IServiceProvider>();
            _mockServiceProvider.GetService(typeof(TestEventHandler)).Returns(new TestEventHandler());
            _registry = new EventHandlerRegistry(_mockServiceProvider);
        }


        [TestMethod]
        public async Task Should_Be_Able_To_Add_And_Get_Handler()
        {
            _registry.AddHandler<TestEventData>(TestEventData.EventName, typeof(TestEventHandler));

            var handler = _registry.GetHandler<TestEventData>(TestEventData.EventName);
            _mockServiceProvider.Received().GetService(typeof(TestEventHandler));
            handler.ShouldNotBe(null);
            handler.GetType().ShouldBe(typeof(TestEventHandler));

        }

        [TestMethod]
        public async Task Should_Throw_Exception_For_Invalid_Handler()
        {
            Should.Throw<ApplicationException>(() => _registry.AddHandler<TestEventData>(TestEventData.EventName, typeof(Object)));
        }

        [TestMethod]
        public async Task Should_Return_Null_If_No_Handler()
        {
            var handler = _registry.GetHandler<TestEventData>("OTHER_EVENT");
            handler.ShouldBe(null);
        }

    }

 
}
