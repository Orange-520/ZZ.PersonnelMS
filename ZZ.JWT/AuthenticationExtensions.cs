using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ZZ.JWT
{
    public static class AuthenticationExtensions
    {
        public static AuthenticationBuilder AddJWTAuthentication(this IServiceCollection services, JWTOptions jwtOpt)
        {
            // 以Jwt的方法做身份认证
            return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                // 配置JWT
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // 验证发行商
                        ValidateAudience = true, // 验证应用者
                        ValidateLifetime = true, // 验证是否过期
                        ValidateIssuerSigningKey = true, // 验证 key
                        ValidIssuer = jwtOpt.Issuer,
                        ValidAudience = jwtOpt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpt.SigningKey))
                    };
                });
        }
    }
}
