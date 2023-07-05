using System.Security.Claims;

namespace ZZ.JWT
{
    public interface IJWTService
    {
        string BuildJWT(IEnumerable<Claim> claims, JWTOptions options);
    }
}
