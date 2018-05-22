using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.Data.Models
{
    public abstract class BaseModel
    {
        [BsonId]
        public virtual string Id { get; set; }
    }
}
