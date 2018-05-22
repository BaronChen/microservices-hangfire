using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E8ay.Common.Models
{
    public class ServiceResult
    {
        public List<string> Errors { get; set; } = new List<string>();

        public bool IsSuccess => !Errors.Any();
    }
}
