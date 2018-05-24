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
using E8ay.Bid.Data;
using E8ay.Bid.Services.Impl;
using E8ay.Common.ApiClient;
using E8ay.Common.Api.Base;
using E8ay.Bid.Data.Models;

namespace E8ay.Item.Services.Test
{
    [TestClass]
    public class BidServiceTest
    {
        private IAuctionBidRepository _mockAuctionBidRepository;
        private IItemServiceApiClient _mockItemServiceApiClient;
        private IJobService _mockJobService;
        private BidService _bidService;

        [TestInitialize]
        public void Init()
        {
            _mockAuctionBidRepository = Substitute.For<IAuctionBidRepository>();
            _mockItemServiceApiClient = Substitute.For<IItemServiceApiClient>();
            _mockJobService = Substitute.For<IJobService>();
            _bidService = new BidService(_mockAuctionBidRepository, _mockItemServiceApiClient, _mockJobService);
        }

        [TestMethod]
        public async Task Should_Create_Place_Bid()
        {
            var bid = new AuctionBidViewModel()
            {
                ItemId = "TEST_ITEM_ID",
                UserId = "TEST_USER_ID",
                BidPrice = 9999
            };
            
            MockGetListByItemIdWithMaxBidPrice(1000, bid.ItemId);
            MockValidateItemForBiddingWithResult(true);
            var result = await _bidService.PlaceBid(bid, bid.UserId);

            result.IsSuccess.ShouldBeTrue();

            await _mockItemServiceApiClient.Received(1).ValidateItemForBidding(Arg.Is<string>(x => x == bid.ItemId));
            await _mockAuctionBidRepository.Received(1).Create(Arg.Is<AuctionBid>(x => !string.IsNullOrEmpty(x.Id) && x.ItemId == bid.ItemId && x.UserId == bid.UserId && x.BidPrice == bid.BidPrice));
            _mockJobService.Received(1).PublishEvent(Arg.Is<Event<BidPlacedForItemEventData>>(
                e => e.EventName == BidPlacedForItemEventData.EventName && e.Data.ItemId == bid.ItemId && e.Data.UserId == bid.UserId && e.Data.BidPrice == bid.BidPrice));
        }

        [TestMethod]
        public async Task Should_Reject_Bid_If_User_Id_Are_Not_The_Same()
        {
            var bid = new AuctionBidViewModel()
            {
                ItemId = "TEST_ITEM_ID",
                UserId = "TEST_USER_ID",
                BidPrice = 9999
            };
            
            var result = await _bidService.PlaceBid(bid, "OTHER_USER_ID");

            result.IsSuccess.ShouldBeFalse();
            result.Errors.ShouldContain("discrepancy in userId");
        }

        [TestMethod]
        public async Task Should_Reject_Bid_If_Not_Highest()
        {
            var bid = new AuctionBidViewModel()
            {
                ItemId = "TEST_ITEM_ID",
                UserId = "TEST_USER_ID",
                BidPrice = 9999
            };
            MockGetListByItemIdWithMaxBidPrice(10000, bid.ItemId);
            MockValidateItemForBiddingWithResult(true);
            var result = await _bidService.PlaceBid(bid, bid.UserId);

            result.IsSuccess.ShouldBeFalse();
            result.Errors.ShouldContain("Your price need to be higher than the other bids.");
        }

        [TestMethod]
        public async Task Should_Reject_Bid_If_Validation_For_item_Fail()
        {
            var bid = new AuctionBidViewModel()
            {
                ItemId = "TEST_ITEM_ID",
                UserId = "TEST_USER_ID",
                BidPrice = 9999
            };
            MockGetListByItemIdWithMaxBidPrice(1000, bid.ItemId);
            MockValidateItemForBiddingWithResult(false);
            var result = await _bidService.PlaceBid(bid, bid.UserId);

            result.IsSuccess.ShouldBeFalse();
            result.Errors.ShouldContain("test item validation error");
        }


        [TestMethod]
        public void Should_Finalise_Auction_With_Highest_Price_And_Bidder()
        {
            var testItemId = "TEST_ITEM_ID";
            var highestPrice = 9000;
            var bidList = GetBidList(highestPrice, testItemId);
            MockGetListByItemIdWithMaxBidPrice(highestPrice, testItemId);

            _bidService.FinaliseAuction(testItemId);

            _mockJobService.Received(1).PublishEvent(Arg.Is<Event<AuctionFinaliseEventData>>(
              e => e.EventName == AuctionFinaliseEventData.EventName 
                && e.Data.ItemId == testItemId 
                && e.Data.HighestBiderId == bidList.OrderByDescending(x => x.BidPrice).First().UserId 
                && e.Data.HighestPrice == highestPrice));

        }

        [TestMethod]
        public void Should_Finalise_Auction_With_Null_Value_If_No_Bids()
        {
            var testItemId = "TEST_ITEM_ID";
           
            _bidService.FinaliseAuction(testItemId);
            _mockAuctionBidRepository.GetListByItemId(Arg.Any<string>()).Returns(new List<AuctionBid>());

            _mockJobService.Received(1).PublishEvent(Arg.Is<Event<AuctionFinaliseEventData>>(
              e => e.EventName == AuctionFinaliseEventData.EventName
                && e.Data.ItemId == testItemId
                && e.Data.HighestBiderId == null
                && e.Data.HighestPrice == 0));

        }

        private void MockValidateItemForBiddingWithResult(bool result)
        {
            _mockItemServiceApiClient.ValidateItemForBidding(Arg.Any<string>()).Returns(
                new StandardResponse<bool>() {
                    Data = result,
                    Errors = result ? new List<string> { } : new List<string> { "test item validation error"}
                });
        }

        private void MockGetListByItemIdWithMaxBidPrice(decimal maxPrice, string itemId)
        {
            var bidList = GetBidList(maxPrice, itemId);
            _mockAuctionBidRepository.GetListByItemId(Arg.Any<string>()).Returns(bidList);
        }

        private IList<AuctionBid> GetBidList(decimal maxPrice, string itemId)
        {
            return new List<AuctionBid>()
                {
                    new AuctionBid()
                    {
                        UserId= "Other_User_1",
                        ItemId=itemId,
                        BidPrice = maxPrice
                    },
                    new AuctionBid()
                    {
                        UserId= "Other_User_2",
                        ItemId=itemId,
                        BidPrice = maxPrice/2
                    },
                    new AuctionBid()
                    {
                        UserId= "Other_User_3",
                        ItemId=itemId,
                        BidPrice = maxPrice/3
                    }
                };
        }
    }
}
