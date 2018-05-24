using E8ay.Item.Data;
using E8ay.Item.Services.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using System.Linq;
using E8ay.Common.HangFire;
using E8ay.Common.Pusher;
using E8ay.Common.ViewModels;
using E8ay.Item.Data.Models;
using E8ay.Common.HangFire.EventBus;
using E8ay.Common.HangFire.EventData;
using E8ay.Common.Enums;
using E8ay.Common.Exceptions;
using E8ay.Item.Services.EventHandler;

namespace E8ay.Item.Services.Test
{
    [TestClass]
    public class AuctionFinaliseEventHandlerTest
    {
        private IItemService _mockItemServie;
        private AuctionFinaliseEventHandler _eventHandler;

        [TestInitialize]
        public void Init()
        {
            _mockItemServie = Substitute.For<IItemService>();
            _eventHandler = new AuctionFinaliseEventHandler(_mockItemServie);
        }

        [TestMethod]
        public async Task Should_Call_Service_With_Correct_Argument()
        {
            var data = new AuctionFinaliseEventData()
            {
                ItemId = "Test_Id",
                HighestBiderId = "TEST_BIDDER_ID",
                HighestPrice = 9999
            };

            var @event = new Event<AuctionFinaliseEventData>()
            {
                EventName = AuctionFinaliseEventData.EventName,
                Data = data
            };

            await _eventHandler.Handle(@event);
            await _mockItemServie.Received(1).UpdateAuctionItemBidInfo(data.ItemId, data.HighestPrice, data.HighestBiderId, ItemStatus.Sold, PusherConstants.AuctionEndEvent);
            _mockItemServie.ClearReceivedCalls();

            @event.Data.HighestBiderId = null;
            await _eventHandler.Handle(@event);
            await _mockItemServie.Received(1).UpdateAuctionItemBidInfo(data.ItemId, data.HighestPrice, data.HighestBiderId, ItemStatus.End, PusherConstants.AuctionEndEvent);
        }

    }
}
