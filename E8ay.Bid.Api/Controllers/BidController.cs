using E8ay.Common.Api.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E8ay.Bid.Api.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/bids")]
    public class BidController:BaseController
    {
        
        public BidController()
        {
            
        }

        [HttpPost]
        [Route("")]
        public IActionResult Create()
        {
            
            return Ok();
        }
        
    }
}
