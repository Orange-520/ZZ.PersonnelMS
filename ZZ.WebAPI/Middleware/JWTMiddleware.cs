using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ZZ.WebAPI.Middleware
{
	public class JWTMiddleware
	{
		private readonly RequestDelegate _next;

		public JWTMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			string token = context.Request.Headers["Authorization"].ToString();

			if (token.Length > 6)
			{
				// 从请求头中获取JWT令牌
				token = token.Substring(7);
				JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
				JwtSecurityToken jwtToken = (JwtSecurityToken)handler.ReadToken(token);
				// 将声明加入到HttpContext.User.Claims中
				context.User = new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims)); 
			}
			await _next(context);
		}
	}
}
