using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.Exceptions
{
    public class MongoUpdateException: Exception
    {
        public MongoUpdateException(string message): base(message) { }
    }
}
