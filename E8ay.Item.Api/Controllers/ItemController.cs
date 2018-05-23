using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E8ay.Common.Api.Base;
using E8ay.Item.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E8ay.Item.Api.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/items")]
    public class ItemsController : BaseController
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult List()
        {
            var items = _itemService.GetAllAuctionItems();

            return OkResult(items);
        }

        //Service to service endpoint is not authorised for now
        [AllowAnonymous]
        [HttpGet]
        [Route("{id}/validate-for-bid")]
        public IActionResult ValidateForBid(string id)
        {
            var result = _itemService.ValidateItemForBidding(id);

            if (result.IsSuccess)
            {
                return OkResult(true);
            }
            else
            {
                return BadResult(false, result.Errors);
            }
        }
    }
}