using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using System.Text;
using ZZ.Commons;
using ZZ.Domain.Entities.Identity;
using ZZ.JWT;
using ZZ.Infrastructure;
using ZZ.WebAPI.Filter;
using ZZ.WebAPI.Middleware;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(option =>
	//忽略循环引用
	option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 注册服务
//builder.Services.AddScoped<IdentityService>();
builder.Services.AddScoped<IdentityRepository>();
builder.Services.AddScoped<CommonRepository>();
builder.Services.AddScoped<OfficeRepository>();
//builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
//builder.Services.AddScoped<ICommonRepository, CommonRepository>();
builder.Services.AddScoped<IDistributedCacheHelper, DistributedCacheHelper>();


// 注册 Filter
builder.Services.Configure<MvcOptions>(options =>
{
	options.Filters.Add<UnitOfWorkFilter>();
});


// 跨域
string[] url = new string[] { "http://localhost:8080" };
builder.Services.AddCors(option =>
{
	option.AddDefaultPolicy(nb =>
	{
		nb.WithOrigins(url).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
	});
});


// 注册数据库上下文
builder.Services.AddDbContext<MyDbContext>(option =>
{
	string? conn = Environment.GetEnvironmentVariable("DefaultDB:ConnStr2");
	option.UseSqlServer(conn);
});


// 数据加密
builder.Services.AddDataProtection();

// 配置 Identity
builder.Services.AddIdentityCore<User>(options =>
{
	// 是不是必须有数字？
	options.Password.RequireDigit = false;
	// 是不是必须有小写字母？
	options.Password.RequireLowercase = false;
	// 是不是必须有特殊符号？
	options.Password.RequireNonAlphanumeric = false;
	// 是不是必须有大写字母？
	options.Password.RequireUppercase = false;
	// 获取或设置密码必须具有的最小长度。默认为6。
	options.Password.RequiredLength = 6;
	// 重置密码时，生成哪种形式的令牌。如果设置为 DefaultEmailProvider，则链接很短，适用于短信验证码；如果不设置为  DefaultEmailProvider，则适用于邮箱重置链接。
	options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
	// 获取或设置用于生成帐户确认电子邮件中使用的令牌的令牌提供程序。
	// 邮件的激活链接，生成比较简单的链接
	options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
}).AddRoles<Role>().AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders();

//IdentityBuilder idBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);

//idBuilder.AddEntityFrameworkStores<MyDbContext>()
//	// 添加默认令牌提供程序，用于为重置密码、更改电子邮件生成令牌、修改电话号码的操作因素身份验证令牌生成。
//	.AddDefaultTokenProviders()
//	// 增加角色管理器
//	.AddRoleManager<RoleManager<Role>>()
//	// 增加用户管理
//	.AddUserManager<UserManager<User>>();



//builder.Services.AddAuthorization();

// 读取appsettings.json配置文件中的信息
// 这一句的意义是？能够让我通过 IOptions<> 获取到一个配置类实例对象，对象中蕴含着配置值？
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));
// 配置JWT
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//{
//	var jwtSettings = builder.Configuration.GetSection("JWT").Get<JWTSettings>();

//	byte[] keyBytes = Encoding.UTF8.GetBytes(jwtSettings.SigningKey);

//	var SigningKey = new SymmetricSecurityKey(keyBytes);

//	options.TokenValidationParameters = new()
//	{
//		ValidateIssuer = false, // 验证发行商
//		ValidateAudience = false, // 验证应用者
//		ValidateLifetime = true, // 验证是否过期
//		ValidateIssuerSigningKey = true, // 验证 key
//		IssuerSigningKey = SigningKey,

//	};
//});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["JWT:Issuer"],
			ValidAudience = builder.Configuration["JWT:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))
		};
	});

// 对 OpenAPI 进行配置，之后 Swagger UI 右上角会增加一个全局请求头 Authorization 配置按钮。
builder.Services.AddSwaggerGen(options =>
{
	var scheme = new OpenApiSecurityScheme()
	{
		// 说明，描述信息
		Description = "Authorization header.\r\nExample:'Bearer 123456abcde'",
		// 一个简单的对象，允许在内部和外部引用规范中的其他组件。
		Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Authorization" },
		// 必需的。在RFC7235中定义的授权头中使用的HTTP授权方案的名称。
		Scheme = "oauth2",
		// 必需的。要使用的头、查询或cookie参数的名称。
		Name = "Authorization",
		// 必需的。API密钥的位置。取值为query、header 或 cookie.
		In = ParameterLocation.Header,
		// 必需的。安全方案的类型。取值为apiKey、http、oauth2，openIdConnect。
		Type = SecuritySchemeType.ApiKey
	};
	// 添加一个或多个“securityDefinitions”，描述你的API是如何被保护的。生成的Swagger
	options.AddSecurityDefinition("Authorization", scheme);
	var requirement = new OpenApiSecurityRequirement();
	requirement[scheme] = new List<string>();
	// 添加全局安全需求
	options.AddSecurityRequirement(requirement);
});


// 注册 Redis 缓存 
builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = "localhost";
	// 设置缓存的 Key 前缀。
	options.InstanceName = "ZZ_";
});


// 日志
builder.Services.AddLogging(builder =>
{
	Log.Logger = new LoggerConfiguration()
	   // .MinimumLevel.Information().Enrich.FromLogContext()
	   .WriteTo.Console()
	   .WriteTo.File("E:\\temp\\ZZPeronnelMS.log")
	   .CreateLogger();
	builder.AddSerilog();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

string configFilePath = builder.Configuration["FilePath"];

// 允许访问静态文件，并且提供 Web 根目录外的文件
// FileProvider：用于资源定位的文件系统。
// RequestPath：映射到静态资源的相对请求路径。
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(configFilePath),
	RequestPath = "/image"
});

// 配置跨域，放在 app.UseAuthorization() 中间件前
app.UseCors();

// 在 app.UseAuthorization() 前添加 app.UseAuthentication() 中间件;
// 尝试对用户进行身份验证，然后才会允许用户访问安全资源。
app.UseAuthentication();

// 用于授权用户访问安全资源的授权中间件
app.UseAuthorization();

// 注册自定义中间件
app.UseMiddleware<JWTMiddleware>();

app.MapControllers();

app.Run();
