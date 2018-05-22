using AutoMapper;
using E8ay.Bid.Data;
using E8ay.Bid.Data.Models;
using E8ay.Common.ApiClient;
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
        private readonly IMapper _mapper;

        public BidService(IAuctionBidRepository auctionBidRepository, IItemServiceApiClient itemServiceApiClient)
        {
            _auctionBidRepository = auctionBidRepository;
            _itemServiceApiClient = itemServiceApiClient;

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<AuctionBid, AuctionBidViewModel>();
                cfg.CreateMap<AuctionBidViewModel, AuctionBid>();
            });

            _mapper = config.CreateMapper();
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

            if (!validationResult.Data)
            {
                result.Errors.AddRange(validationResult.Errors);
                return result;
            }

            var bids = _auctionBidRepository.GetListByItemId(bidViewModel.ItemId);

            if (bidViewModel.BidPrice <= bids.Max(x => x.BidPrice))
            {
                result.Errors.Add("Your price need to be higher than the other bids.");
                return result;
            }

            var bid = _mapper.Map<AuctionBidViewModel, AuctionBid>(bidViewModel);
            bid.CreatedDateTime = DateTime.Now;
            await _auctionBidRepository.Create(bid);

            return result;
        }
    }
}
