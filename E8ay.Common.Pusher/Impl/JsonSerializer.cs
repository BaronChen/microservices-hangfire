using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PusherServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.Pusher.Impl
{
    internal class JsonSerializer : ISerializeObjectsToJson
    {
        public string Serialize(object objectToSerialize)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.SerializeObject(objectToSerialize, serializerSettings);
        }
    }
}
