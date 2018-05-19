using E8ay.User.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.User.Services.Impl
{
    internal class AuthService: IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthService(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<string> SignInAndGetToken(string username, string password)
        {

            var identity = await GetClaimsIdentity(username, password);
            if (identity == null)
            {
                return null;
            }

            var jwt = GenerateJwt(identity, username, new JsonSerializerSettings { Formatting = Formatting.Indented });

            return jwt;
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return null;

            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return null;

            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return _jwtGenerator.GenerateClaimsIdentity(userName, userToVerify.Id);
            }

            return null;
        }

        private string GenerateJwt(ClaimsIdentity identity,  string userName,  JsonSerializerSettings serializerSettings)
        {
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = _jwtGenerator.GenerateEncodedToken(userName, identity),
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }


        public async Task SeedUser()
        {
            await SeedUser("user1", "password", "7fe4921f575249afcf88bb5d");
            await SeedUser("user2", "password", "969754f6c1b85714f82d3466");
        }


        private async Task SeedUser(string username, string password, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            await _userManager.CreateAsync(new AppUser() { UserName = username, Id = id }, password);
        }
    }
}
