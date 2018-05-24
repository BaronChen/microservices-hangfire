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

namespace E8ay.Item.Services.Test
{
    [TestClass]
    public class ItemServiceTest
    {
        private IAuctionItemRepository _mockAuctionItemRepository;
        private IJobService _mockJobService;
        private IPusherManager _mockPusherManager;
        private ItemService _itemService;

        [TestInitialize]
        public void Init()
        {
            _mockAuctionItemRepository = Substitute.For<IAuctionItemRepository>();
            _mockJobService = Substitute.For<IJobService>();
            _mockPusherManager = Substitute.For<IPusherManager>();
            _itemService = new ItemService(_mockAuctionItemRepository, _mockJobService, _mockPusherManager);
        }

        [TestMethod]
        public async Task Should_Create_Auction_Item()
        {
            var item = new AuctionItemViewModel()
            {
                Name = "TEST_ITEM",
                Description = "TEST_Description"
            };

            await _itemService.CreateAuctionItem(item);

            await _mockAuctionItemRepository.Received(1).Create(Arg.Is<AuctionItem>(x => !string.IsNullOrEmpty(x.Id)));

            _mockJobService.Received(1).PublishDelayedEvent(Arg.Is<Event<AuctionEndEventData>>(e => !string.IsNullOrEmpty(e.Data.ItemId)), Arg.Any<TimeSpan>());

        }

        [TestMethod]
        public void Should_Get_All_Auction_Items()
        {
            var itemList = new List<AuctionItem>()
            {
                new AuctionItem()
                {
                    Name = "TEST_ITEM1",
                    Description = "TEST_Description1"
                },
                new AuctionItem()
                {
                    Name = "TEST_ITEM2",
                    Description = "TEST_Description2"
                },
                new AuctionItem()
                {
                    Name = "TEST_ITEM3",
                    Description = "TEST_Description3"
                }
            };

            _mockAuctionItemRepository.GetAll().Returns(itemList);

            var items = _itemService.GetAllAuctionItems();
            items.Count().ShouldBe(itemList.Count());
            items.First().Name.ShouldBe(itemList.First().Name);
            items.Last().Description.ShouldBe(itemList.Last().Description);

            _mockAuctionItemRepository.Received(1).GetAll();

        }

        [TestMethod]
        public void Should_Approve_Valid_Item_For_Biding()
        {
            var item = new AuctionItem()
            {
                Id = "12345",
                Name = "TEST_ITEM",
                Description = "TEST_Description",
                EndDateTime = DateTime.Now.AddDays(2),
                Status = ItemStatus.Listed
            };

            _mockAuctionItemRepository.GetById(Arg.Any<string>()).Returns(item);

            var result = _itemService.ValidateItemForBidding(item.Id);

            result.IsSuccess.ShouldBe(true);

            _mockAuctionItemRepository.Received(1).GetById(Arg.Is<string>(x => x == item.Id));
        }

        [TestMethod]
        public void Should_Reject_Valid_Item_For_Biding()
        {
            var itemList = new List<AuctionItem>()
            {
                new AuctionItem()
                {
                    Id = "12345",
                    Name = "TEST_ITEM1",
                    Description = "TEST_Description1",
                    EndDateTime = DateTime.Now.AddSeconds(-1),
                    Status = ItemStatus.Listed
                },

                new AuctionItem()
                {
                    Id = "54321",
                    Name = "TEST_ITEM2",
                    Description = "TEST_Description2",
                    EndDateTime = DateTime.Now.AddMinutes(10),
                    Status = ItemStatus.End
                }
            };

            _mockAuctionItemRepository.GetById(Arg.Is<string>(x => itemList.Any(i => i.Id == x))).Returns(
                callinfo => itemList.SingleOrDefault(x => x.Id == callinfo.ArgAt<string>(0))
            );

            var result = _itemService.ValidateItemForBidding("Other_Id");
            result.IsSuccess.ShouldBe(false);
            result.Errors.ShouldContain("Item not exists.");

            result = _itemService.ValidateItemForBidding(itemList[0].Id);
            result.IsSuccess.ShouldBe(false);
            result.Errors.ShouldContain("Cannot bid as auction for this item has ended.");

            result = _itemService.ValidateItemForBidding(itemList[1].Id);
            result.IsSuccess.ShouldBe(false);
            result.Errors.ShouldContain("Cannot bid as auction for this item has ended.");

            _mockAuctionItemRepository.Received(3).GetById(Arg.Any<string>());

        }

        [TestMethod]
        public async Task Should_Update_Item_Info_Base_On_Input()
        {
            var item = new AuctionItem()
            {
                Id = "12345",
                Name = "TEST_ITEM1",
                Description = "TEST_Description1",
                EndDateTime = DateTime.Now.AddSeconds(-1),
                Status = ItemStatus.Listed
            };

            _mockAuctionItemRepository.GetById(Arg.Any<string>()).Returns(item);

            var newPrice = 100m;
            var highestBiderId = "HIGHEST_BIDER";
            await _itemService.UpdateAuctionItemBidInfo(item.Id, newPrice, highestBiderId, ItemStatus.UnderOffer);

            await _mockAuctionItemRepository.Received().Update(
                Arg.Is<AuctionItem>(x => x.Id == item.Id
                    && x.HighestPrice == newPrice
                    && x.HighestBiderId == highestBiderId
                    && x.Status == ItemStatus.UnderOffer));
            await _mockPusherManager.DidNotReceive().PushNotification(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<object>());


            await _itemService.UpdateAuctionItemBidInfo(item.Id, newPrice, highestBiderId, ItemStatus.UnderOffer, notifyWithEvent: PusherConstants.AuctionEndEvent);
            await _mockPusherManager.Received(1).PushNotification(PusherConstants.ItemChannel, PusherConstants.AuctionEndEvent, Arg.Any<AuctionItemViewModel>());
            
        }

        [TestMethod]
        public void Should_Hide_Update_Exception()
        {
            var item = new AuctionItem()
            {
                Id = "12345",
                Name = "TEST_ITEM1",
                Description = "TEST_Description1",
                EndDateTime = DateTime.Now.AddSeconds(-1),
                Status = ItemStatus.Listed
            };

            _mockAuctionItemRepository.GetById(Arg.Any<string>()).Returns(item);

            var newPrice = 100m;
            var highestBiderId = "HIGHEST_BIDER";

            _mockAuctionItemRepository.When(x => x.Update(Arg.Any<AuctionItem>())).Do(x => { throw new MongoUpdateException("test error"); });
            Should.NotThrow(async () => await _itemService.UpdateAuctionItemBidInfo(item.Id, newPrice, highestBiderId, ItemStatus.UnderOffer));

        }

        [TestMethod]
        public void Should_Throw_Non_Update_Exception()
        {
            var item = new AuctionItem()
            {
                Id = "12345",
                Name = "TEST_ITEM1",
                Description = "TEST_Description1",
                EndDateTime = DateTime.Now.AddSeconds(-1),
                Status = ItemStatus.Listed
            };

            _mockAuctionItemRepository.GetById(Arg.Any<string>()).Returns(item);

            var newPrice = 100m;
            var highestBiderId = "HIGHEST_BIDER";

            _mockAuctionItemRepository.When(x => x.Update(Arg.Any<AuctionItem>())).Do(x => { throw new Exception("test error"); });
            Should.Throw<Exception>(async () => await _itemService.UpdateAuctionItemBidInfo(item.Id, newPrice, highestBiderId, ItemStatus.UnderOffer));

        }
    }
}
