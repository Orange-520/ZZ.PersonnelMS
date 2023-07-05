using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using ZZ.Commons;
using ZZ.Domain.Entities.Identity;
using ZZ.Infrastructure;
using ZZ.JWT;
using ZZ.WebAPI.Filter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(option =>
	//忽略循环引用
	option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* 注册范围服务 */
// 什么是范围，就是此次请求过程中，不同类通过构造函数注入的对象，都是同一个。
//builder.Services.AddScoped<IdentityService>();
builder.Services.AddScoped<IdentityRepository>();
builder.Services.AddScoped<DepartmentRepository>();
builder.Services.AddScoped<PositionRepository>();
builder.Services.AddScoped<JoinUsRepository>();
builder.Services.AddScoped<OfficeRepository>();
//builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
//builder.Services.AddScoped<ICommonRepository, CommonRepository>();
builder.Services.AddScoped<IDistributedCacheHelper, DistributedCacheHelper>();

// 注册JWT生成服务
builder.Services.AddScoped<IJWTService, JWTService>();


/* 注册 Filter */
builder.Services.Configure<MvcOptions>(options =>
{
	options.Filters.Add<UnitOfWorkFilter>();
});


// 跨域
string[] cors = builder.Configuration.GetSection("Cors").GetChildren().Select(e => e.Value).ToArray()!;
if (!cors.Any())
{
	Console.WriteLine("没有配置允许跨域访问的地址");
}
else
{
    foreach (var item in cors)
    {
        Console.WriteLine("CORS之允许访问的网址：{0}", item);
    }
    builder.Services.AddCors(option =>
    {
        option.AddDefaultPolicy(nb =>
        {
            nb.WithOrigins(cors).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        });
    });
}



// 注册数据库上下文
builder.Services.AddDbContext<MyDbContext>(option =>
{
	string? conn = Environment.GetEnvironmentVariable("DefaultDB:ConnStr2");
	option.UseSqlServer(conn);
});


// 将数据保护服务添加到了依赖注入容器中，该服务可用于加密或解密应用程序中的数据。
// 通过这种方式，可以轻松地使用 Data Protection API 来加密和保护应用程序中的敏感信息。
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



// 获取appsettings.json配置文件中的信息
// 方式一，配置实体类，使用实体类对应的属性获取：
//JWTSettings jwtSettings = builder.Configuration.GetSection("JWT").Get<JWTSettings>();

// 方式二，直接获取Jwt配置项中为Issuer的属性项
//string Issuer = builder.Configuration["JWT:Issuer"];


// 向服务容器（Service Container）注册一个名为JWTOptions的配置对象，它的配置信息来源于应用程序的配置文件（appsettings.json）中名为JWT的节点。
// 在代码中使用IOptions<JWTOptions>依赖注入方式，在需要配置JWT选项的地方，就能够获取到从配置文件中读取的配置项。
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));

// 为 Swagger 设置能输入请求头
builder.Services.Configure<SwaggerGenOptions>(c =>
{
    c.AddAuthenticationHeader();
});

//// 授权，权限认证
//builder.Services.AddAuthorization(options =>
//{
//	// 策略授权
//	options.AddPolicy("AdminAndPersonnelInCharge", policy =>
//		policy.RequireRole("admin,personnelInCharge")
//	);
//});
// 授权，权限认证
builder.Services.AddAuthorization();


// 配置身份认证
JWTOptions jwtOpt = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
builder.Services.AddJWTAuthentication(jwtOpt);


// 注册 Redis 缓存 
builder.Services.AddStackExchangeRedisCache(options =>
{
	// 连接 Redis 的服务器地址，默认 Redis 的服务器地址是 127.0.0.1
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

// 注册自定义中间件，（不需要了）
//app.UseMiddleware<JWTMiddleware>();

app.MapControllers();

app.Run();
