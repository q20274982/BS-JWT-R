using BS_JWT_R.Models;
using BS_JWT_R.Models.DTO;
using BS_JWT_R.Models.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BS_JWT_R.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration Configuration;
        private readonly JwtContext _context;
        public JwtHelper(IConfiguration configuration, JwtContext context)
        {
            Configuration = configuration;
            _context = context;
        }

        public async Task<AuthResultDto> GenerateToken(string userName, int expireMinute = 30)
        {
            var issuer = Configuration.GetValue<string>("JwtSettings:Issuer");
            var signKey = Configuration.GetValue<string>("JwtSettings:SignKey");

            // 設定要加入到 JWT Token 中的聲明
            var claims = new List<Claim>();

            //使用定義的規格 https://datatracker.ietf.org/doc/html/rfc7519#section-4.1
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            // 自行擴充
            claims.Add(new Claim("roles", "Admin"));
            claims.Add(new Claim("roles", "Users"));

            // 宣告集合所描述的身分識別
            var userClaimsIdentity = new ClaimsIdentity(claims);

            // 建立一組對稱式加密的金鑰，主要用於 JWT 簽章之用
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));

            // 用來產生數位簽章的密碼編譯演算法
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // 預留位置，適用於和已發行權杖相關的所有屬性，用來定義JWT的相關設定
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = issuer,
                Subject = userClaimsIdentity,
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = signingCredentials
            };

            // 用來產生JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var serializeToken = tokenHandler.WriteToken(securityToken);

            var refreshToken = new RefreshToken()
            {
                Username = userName,
                Token = Guid.NewGuid().ToString()
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResultDto()
            {
                Token = serializeToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        public RefreshToken GetRefreshToken(string refreshToken)
        {
            var source = _context.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken);

            if (source is null) return default;

            return source;
        }
    }
}
