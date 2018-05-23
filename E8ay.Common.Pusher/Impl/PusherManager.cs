using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PusherServer;


namespace E8ay.Common.Pusher.Impl
{
    internal class PusherManager: IPusherManager
    {
        private readonly PusherConfigOptions _options;

        public PusherManager(IOptions<PusherConfigOptions> options)
        {
            _options = options.Value;
        }

        public async Task PushNotification(string channel, string pusherEventName, object data)
        {
            var pusher = GetPusher();

            await pusher.TriggerAsync(
                channel,
                pusherEventName,
                data);
        }

        private PusherServer.Pusher GetPusher()
        {
            var options = new PusherOptions
            {
                Cluster = _options.Cluster,
                Encrypted = true,
                JsonSerializer = new JsonSerializer()
            };

            var pusher = new PusherServer.Pusher(
              _options.AppId,
              _options.ApiKey,
              _options.AppSecret,
              options);

            return pusher;
        }

    }
}
