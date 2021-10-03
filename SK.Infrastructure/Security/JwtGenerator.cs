using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SK.Infrastructure.Security
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOptions<JwtOptions> _options;

        public JwtGenerator(IConfiguration configuration, UserManager<AppUser> userManager, IOptions<JwtOptions> options)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
            _userManager = userManager;
            _options = options;
        }

        public async Task<string> CreateToken(AppUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            };

            // generate signing credentials
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_options.Value.ExpiryMinutes),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}