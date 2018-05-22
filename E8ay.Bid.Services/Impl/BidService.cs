using AutoMapper;
using E8ay.Bid.Data;
using E8ay.Bid.Data.Models;
using E8ay.Common.ApiClient;
using E8ay.Common.HangFire;
using E8ay.Common.HangFire.EventBus;
using E8ay.Common.HangFire.EventData;
using E8ay.Common.Models;
using E8ay.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Bid.Services.Impl
{
    internal class BidService: IBidService
    {
        private readonly IAuctionBidRepository _auctionBidRepository;
        private readonly IItemServiceApiClient _itemServiceApiClient;
        private readonly IJobService _jobService;
        private readonly IMapper _mapper;

        public BidService(IAuctionBidRepository auctionBidRepository, IItemServiceApiClient itemServiceApiClient, IJobService jobService)
        {
            _auctionBidRepository = auctionBidRepository;
            _itemServiceApiClient = itemServiceApiClient;
            _jobService = jobService;

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<AuctionBid, AuctionBidViewModel>();
                cfg.CreateMap<AuctionBidViewModel, AuctionBid>();
            });

            _mapper = config.CreateMapper();
        }

        public async Task DeleteAllBids()
        {
            await _auctionBidRepository.DeleteAll();
        }

        public void FinaliseAuction(string itemId)
        {
            var bids = _auctionBidRepository.GetListByItemId(itemId);

            var @event = new Event<AuctionFinaliseEventData>() { Data = new AuctionFinaliseEventData() { ItemId = itemId }, EventName = AuctionFinaliseEventData.EventName };
            @event.TargetQueues.Add(QueueConstants.AuctionEndFinalise);


            if (bids.Any())
            {
                var highestBid = bids.OrderByDescending(x => x.BidPrice).First();
                @event.Data.HighestBiderId = highestBid.UserId;
                @event.Data.HighestPrice = highestBid.BidPrice;
            }
            else
            {
                @event.Data.HighestBiderId = null;
                @event.Data.HighestPrice = 0;
            }

            _jobService.PublishEvent(@event);
        }

        public async Task<ServiceResult> PlaceBid(AuctionBidViewModel bidViewModel, string userId)
        {
            var result = new ServiceResult();
            if (bidViewModel.UserId != userId)
            {
                result.Errors.Add("discrepancy in userId");
                return result;
            }

            //Should check if item actually exist as well
            var validationResult = await _itemServiceApiClient.ValidateItemForBidding(bidViewModel.ItemId);

            if (validationResult == null)
            {
                throw new ApplicationException("Unexpected error!");
            }

            if (!validationResult.Data)
            {
                result.Errors.AddRange(validationResult.Errors);
                return result;
            }

            var bids = _auctionBidRepository.GetListByItemId(bidViewModel.ItemId);

            if (bids.Any() && bidViewModel.BidPrice <= bids.Max(x => x.BidPrice))
            {
                result.Errors.Add("Your price need to be higher than the other bids.");
                return result;
            }

            var bid = _mapper.Map<AuctionBidViewModel, AuctionBid>(bidViewModel);
            bid.Id = Guid.NewGuid().ToString();
            bid.CreatedDateTime = DateTime.Now;
            await _auctionBidRepository.Create(bid);

            var @event = new Event<BidPlacedForItemEventData>() { Data = new BidPlacedForItemEventData() { ItemId = bid.ItemId, UserId = bid.UserId, BidPrice = bid.BidPrice }, EventName = BidPlacedForItemEventData.EventName};

            @event.TargetQueues.Add(QueueConstants.ItemQueue);

            _jobService.PublishEvent(@event);
            
            return result;
        }
    }
}
