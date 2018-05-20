using AutoMapper;
using E8ay.Common.Enums;
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
        private readonly IMapper _mapper;

        public ItemService(IAuctionItemRepository auctionItemRepository)
        {
            _auctionItemRepository = auctionItemRepository;

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<AuctionItem, AuctionItemViewModel>();
                cfg.CreateMap<AuctionItemViewModel, AuctionItem>();
            });

            _mapper = config.CreateMapper();
        }

        public async Task CreateAuctionItem(AuctionItemViewModel itemViewModel)
        {
            var item = _mapper.Map<AuctionItemViewModel, AuctionItem>(itemViewModel);

            await _auctionItemRepository.CreateItem(item);
        }

        public IEnumerable<AuctionItemViewModel> GetAllAuctionItems()
        {
            return _auctionItemRepository.GetAllItems().Select(x => _mapper.Map<AuctionItem, AuctionItemViewModel>(x));
        }

        public async Task SeedAuctionItems()
        {
            await _auctionItemRepository.DeleteAll();
            for (var i = 0; i <= 10; i ++)
            {
                var item = new AuctionItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Item " + i,
                    Description = "Item description " + i,
                    Status = ItemStatus.Listed,
                    StartDateTime = DateTime.Now,
                    EndDateTime = DateTime.Now.AddMinutes(i),
                    HighestPrice = 0,
                    HighestBiderId = string.Empty
                };

                await _auctionItemRepository.CreateItem(item);
            }
        }
    }
}
