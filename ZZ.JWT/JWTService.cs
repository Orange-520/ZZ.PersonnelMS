using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ZZ.JWT
{
    public class JWTService : IJWTService
    {
        public string BuildJWT(IEnumerable<Claim> claims, JWTOptions JwtOption)
        {
            // 对称安全密钥（将指定字符串中的所有字符编码为字节序列（获取签名密钥））
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOption.SigningKey));
            // 时间间隔
            TimeSpan ExpiryDuration = TimeSpan.FromSeconds(JwtOption.ExpireSeconds);

            // 定义 Microsoft.IdentityModel.Tokens。用于数字签名的密钥、算法和摘要。
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: JwtOption.Issuer,
                audience: JwtOption.Audience,
                claims: claims,
                // 定义JWT令牌还不能被接受的时间的。也就是说，在NotBefore时间之前，JWT令牌将不被认为是合法的，这通常用于避免网络时钟同步问题或者nonce重放的问题。当JWT令牌处于NotBefore时间之后或刚好在NotBefore时间时生成，JWT令牌才可被接受。
                notBefore: DateTime.Now,
                // 设置过期时间
                expires: DateTime.Now.Add(ExpiryDuration),
                signingCredentials: creds);

            // JwtSecurityTokenHandler类：用于创建和验证 JWT 。WriteToken：生成 JWT。
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
