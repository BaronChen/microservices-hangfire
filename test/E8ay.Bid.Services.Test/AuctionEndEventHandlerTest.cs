using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using System.Linq;
using E8ay.Common.HangFire;

using E8ay.Common.ViewModels;

using E8ay.Common.HangFire.EventBus;
using E8ay.Common.HangFire.EventData;
using E8ay.Common.Enums;
using E8ay.Common.Exceptions;
using E8ay.Bid.Services.EventHandler;

namespace E8ay.Bid.Services.Test
{
    [TestClass]
    public class AuctionEndEventHandlerTest
    {
        private IBidService _mockBidService;
        private AuctionEndEventHandler _eventHandler;

        [TestInitialize]
        public void Init()
        {
            _mockBidService = Substitute.For<IBidService>();
            _eventHandler = new AuctionEndEventHandler(_mockBidService);
        }

        [TestMethod]
        public async Task Should_Call_Service_With_Correct_Argument()
        {
            var data = new AuctionEndEventData()
            {
                ItemId = "Test_Id"
            };

            var @event = new Event<AuctionEndEventData>()
            {
                EventName = AuctionEndEventData.EventName,
                Data = data
            };

            await _eventHandler.Handle(@event);
             _mockBidService.Received(1).FinaliseAuction(data.ItemId);
            _mockBidService.ClearReceivedCalls();
            
        }

    }
}
