using E8ay.Common.Api.Base;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Common.ApiClient.Impl
{
    public class ItemServiceApiClient:IItemServiceApiClient
    {
        private readonly ApiClientOptions _options;
        
        public ItemServiceApiClient(IOptions<ApiClientOptions> options)
        {
            _options = options.Value;
        }

        
        public async Task<StandardResponse<bool>> ValidateItemForBidding(string itemId)
        {
            var contents = await GetResponse($"{_options.ItemServiceUrl}/api/items/{itemId}/validate-for-bid");

            return JsonConvert.DeserializeObject<StandardResponse<bool>>(contents);
        }

        private async Task<string> GetResponse(string url)
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(url);
            var contents = await response.Content.ReadAsStringAsync();

            return contents;
        }

    }
}
