using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E8ay.Common.Api.Base
{
    public class StandardResponse<T>
    {
        public List<string> Errors { get; set; } = new List<string>();

        public List<string> Messages { get; set; } = new List<string>();

        public List<string> Warnings { get; set; } = new List<string>();

        public T Data { get; set; }

        public bool IsError  => Errors.Any();
    }
}
