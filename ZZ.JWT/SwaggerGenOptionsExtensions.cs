using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ZZ.JWT
{
    public static class SwaggerGenOptionsExtensions
    {
        /// <summary>
        /// 对 OpenAPI 进行配置，之后 Swagger UI 右上角会增加一个全局请求头 Authorization 配置按钮。
        /// </summary>
        /// <param name="c"></param>
        public static void AddAuthenticationHeader(this SwaggerGenOptions c)
        {
            // 添加一个或多个“securityDefinitions”，描述你的API是如何被保护的。生成的Swagger
            c.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
            {
                // 说明，描述信息
                Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
                // 必需的。要使用的头、查询或cookie参数的名称。
                Name = "Authorization",
                // 必需的。API密钥的位置。取值为query、header 或 cookie.
                In = ParameterLocation.Header,
                // 必需的。安全方案的类型。取值为apiKey、http、oauth2，openIdConnect。
                Type = SecuritySchemeType.ApiKey,
                // 必需的。在RFC7235中定义的授权头中使用的HTTP授权方案的名称。
                Scheme = "Authorization"
            });

            // 添加全局安全需求
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                   new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Authorization"
                        },
                        Scheme = "oauth2",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        }
    }
}
