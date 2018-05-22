using E8ay.Common.Api.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Common.ApiClient
{
    public interface IItemServiceApiClient
    {
        Task<StandardResponse<bool>> ValidateItemForBidding(string itemId);
    }
}
