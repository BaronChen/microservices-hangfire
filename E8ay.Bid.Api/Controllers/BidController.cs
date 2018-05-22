using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E8ay.Bid.Services;
using E8ay.Common.Api.Base;
using E8ay.Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E8ay.Bid.Api.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/bids")]
    public class BidController:BaseController
    {
        private readonly IBidService _bidService;
        
        public BidController(IBidService bidService)
        {
            _bidService = bidService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Create([FromBody]AuctionBidViewModel bidViewModel)
        {
            var userId = GetUserId();
            var result = await _bidService.PlaceBid(bidViewModel, userId);

            if (result.IsSuccess)
            {
                return OkResult<object>(null, "Bid placed");
            }
            else
            {
                return BadResult(result.Errors);
            }
        }
    }
}
