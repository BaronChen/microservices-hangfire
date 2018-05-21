using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.Api.Base
{
    public class BaseController: Controller
    {
        protected IActionResult OkResult<T>(T data)
        {
            return OkResult(data, new List<string>() {});
        }

        protected IActionResult OkResult<T>(T data, string message)
        {
            return OkResult(data, new List<string>() { message });
        }

        protected IActionResult OkResult<T>(T data, IEnumerable<string> messages)
        {
            var response = new StandardResponse<T> { Data = data };
            response.Messages.AddRange(messages);
            return Ok(response);
        }

        protected IActionResult BadResult(string error)
        {
            return BadResult(new List<string>() { error });
        }

        protected IActionResult BadResult(IEnumerable<string> errors)
        {
            var response = new StandardResponse<object> { Data = null };
            response.Errors.AddRange(errors);
            return BadRequest(response);
        }

    }
}
