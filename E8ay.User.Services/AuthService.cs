using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.User.Services
{
    public interface IAuthService
    {
        Task<string> SignInAndGetToken(string username, string password);

        Task SeedUser();
    }
}
