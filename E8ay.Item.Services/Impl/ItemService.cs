using AutoMapper;
using E8ay.Common.Enums;
using E8ay.Common.Exceptions;
using E8ay.Common.HangFire;
using E8ay.Common.HangFire.EventBus;
using E8ay.Common.HangFire.EventData;
using E8ay.Common.Models;
using E8ay.Common.Pusher;
using E8ay.Common.ViewModels;
using E8ay.Item.Data;
using E8ay.Item.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Item.Services.Impl
{
    internal class ItemService : IItemService
    {
        private readonly IAuctionItemRepository _auctionItemRepository;
        private readonly IJobService _jobService;
        private readonly IPusherManager _pusherManager;
        private readonly IMapper _mapper;

        public ItemService(IAuctionItemRepository auctionItemRepository, IJobService jobService, IPusherManager pusherManager)
        {
            _auctionItemRepository = auctionItemRepository;
            _jobService = jobService;
            _pusherManager = pusherManager;

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<AuctionItem, AuctionItemViewModel>();
                cfg.CreateMap<AuctionItemViewModel, AuctionItem>();
            });

            _mapper = config.CreateMapper();
        }

        public async Task CreateAuctionItem(AuctionItemViewModel itemViewModel)
        {
            var item = _mapper.Map<AuctionItemViewModel, AuctionItem>(itemViewModel);
            
            if (string.IsNullOrWhiteSpace(item.Id))
            {
                item.Id = Guid.NewGuid().ToString();
            }

            await _auctionItemRepository.Create(item);

            ScheduleAuctionEndEvent(item);
        }

        public IEnumerable<AuctionItemViewModel> GetAllAuctionItems()
        {
            return _auctionItemRepository.GetAll().Select(x => _mapper.Map<AuctionItem, AuctionItemViewModel>(x));
        }

        public ServiceResult ValidateItemForBidding(string itemId)
        {
            var result = new ServiceResult();
            var item = _auctionItemRepository.GetById(itemId);

            if (item == null)
            {
                result.Errors.Add($"Item not exists.");
                return result;
            }

            if (item.Status == ItemStatus.Sold || item.Status == ItemStatus.End || item.EndDateTime <= DateTime.Now)
            {
                result.Errors.Add("Cannot bid as auction for this item has ended.");
            }

            return result;
        }

        public async Task UpdateAuctionItemBidInfo(string id, decimal price, string highestBiderId, ItemStatus? itemStatus = null, string notifyWithEvent = null)
        {
            var item = _auctionItemRepository.GetById(id);

            if (item == null)
            {
                //TODO: Log here
                return;
            }

            item.HighestBiderId = highestBiderId;
            item.HighestPrice = price;

            if (itemStatus.HasValue)
            {
                item.Status = itemStatus.Value;
            }

            try
            {
                await _auctionItemRepository.Update(item);
            }
            catch(MongoUpdateException ex)
            {
                //TODO: fail to update, log here
            }
            catch(Exception)
            {
                throw;
            }

            if (!string.IsNullOrWhiteSpace(notifyWithEvent))
            {
                var itemViewModel = _mapper.Map<AuctionItem, AuctionItemViewModel>(item);
                await _pusherManager.PushNotification(PusherConstants.ItemChannel, notifyWithEvent, itemViewModel);
            }
        }

        public async Task SeedAuctionItems()
        {
            await _auctionItemRepository.DeleteAll();

            var idList = new string[]
            {
                "ca714074-2026-4f31-b656-f748e500293d",
                "04f1e771-99de-4fab-992b-c1dd042c477c",
                "b606eb52-c7d3-4ed3-93b6-0f28fc7f922c",
                "469be489-626c-466a-9cc9-452cb859ae73",
                "dfecbffb-8432-4e4d-80fa-86c0eb3f5238",
                "a3c11a1d-0f8b-40be-969a-083867e6ccad",
                "a1b3b921-bc0e-4181-a2d1-0168a86a2dd4",
                "16ff1488-8ed1-431d-8620-e99c7b36b152",
                "2abcbb85-3cf9-477e-a633-d6306d14064b",
                "75223e74-6c8a-4966-95cd-24c677710f68"
            };

            for (var i = 0; i < 10; i ++)
            {
                var item = new AuctionItem()
                {
                    Id = idList[i],
                    Name = "Item " + i,
                    Description = "Item description " + i,
                    Status = ItemStatus.Listed,
                    StartDateTime = DateTime.Now,
                    EndDateTime = DateTime.Now.AddMinutes(i+1),
                    HighestPrice = 0,
                    HighestBiderId = string.Empty
                };

                await _auctionItemRepository.Create(item);
                ScheduleAuctionEndEvent(item);
            }

        }

        private void ScheduleAuctionEndEvent(AuctionItem item)
        {
            var @event = new Event<AuctionEndEventData>() { Data = new AuctionEndEventData() { ItemId = item.Id }, EventName = AuctionEndEventData.EventName };

            _jobService.PublishDelayedEvent(@event, item.EndDateTime - DateTime.Now);
        }

      
    }
}
