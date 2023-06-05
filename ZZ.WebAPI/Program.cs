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
	//����ѭ������
	option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ע�����
//builder.Services.AddScoped<IdentityService>();
builder.Services.AddScoped<IdentityRepository>();
builder.Services.AddScoped<CommonRepository>();
builder.Services.AddScoped<OfficeRepository>();
//builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
//builder.Services.AddScoped<ICommonRepository, CommonRepository>();
builder.Services.AddScoped<IDistributedCacheHelper, DistributedCacheHelper>();


// ע�� Filter
builder.Services.Configure<MvcOptions>(options =>
{
	options.Filters.Add<UnitOfWorkFilter>();
});


// ����
string[] url = new string[] { "http://localhost:8080" };
builder.Services.AddCors(option =>
{
	option.AddDefaultPolicy(nb =>
	{
		nb.WithOrigins(url).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
	});
});


// ע�����ݿ�������
builder.Services.AddDbContext<MyDbContext>(option =>
{
	string? conn = Environment.GetEnvironmentVariable("DefaultDB:ConnStr2");
	option.UseSqlServer(conn);
});


// ���ݼ���
builder.Services.AddDataProtection();

// ���� Identity
builder.Services.AddIdentityCore<User>(options =>
{
	// �ǲ��Ǳ��������֣�
	options.Password.RequireDigit = false;
	// �ǲ��Ǳ�����Сд��ĸ��
	options.Password.RequireLowercase = false;
	// �ǲ��Ǳ�����������ţ�
	options.Password.RequireNonAlphanumeric = false;
	// �ǲ��Ǳ����д�д��ĸ��
	options.Password.RequireUppercase = false;
	// ��ȡ���������������е���С���ȡ�Ĭ��Ϊ6��
	options.Password.RequiredLength = 6;
	// ��������ʱ������������ʽ�����ơ��������Ϊ DefaultEmailProvider�������Ӻ̣ܶ������ڶ�����֤�룻���������Ϊ  DefaultEmailProvider���������������������ӡ�
	options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
	// ��ȡ���������������ʻ�ȷ�ϵ����ʼ���ʹ�õ����Ƶ������ṩ����
	// �ʼ��ļ������ӣ����ɱȽϼ򵥵�����
	options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
}).AddRoles<Role>().AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders();

//IdentityBuilder idBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);

//idBuilder.AddEntityFrameworkStores<MyDbContext>()
//	// ���Ĭ�������ṩ��������Ϊ�������롢���ĵ����ʼ��������ơ��޸ĵ绰����Ĳ������������֤�������ɡ�
//	.AddDefaultTokenProviders()
//	// ���ӽ�ɫ������
//	.AddRoleManager<RoleManager<Role>>()
//	// �����û�����
//	.AddUserManager<UserManager<User>>();



//builder.Services.AddAuthorization();

// ��ȡappsettings.json�����ļ��е���Ϣ
// ��һ��������ǣ��ܹ�����ͨ�� IOptions<> ��ȡ��һ��������ʵ�����󣬶������̺�������ֵ��
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));
// ����JWT
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//{
//	var jwtSettings = builder.Configuration.GetSection("JWT").Get<JWTSettings>();

//	byte[] keyBytes = Encoding.UTF8.GetBytes(jwtSettings.SigningKey);

//	var SigningKey = new SymmetricSecurityKey(keyBytes);

//	options.TokenValidationParameters = new()
//	{
//		ValidateIssuer = false, // ��֤������
//		ValidateAudience = false, // ��֤Ӧ����
//		ValidateLifetime = true, // ��֤�Ƿ����
//		ValidateIssuerSigningKey = true, // ��֤ key
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

// �� OpenAPI �������ã�֮�� Swagger UI ���Ͻǻ�����һ��ȫ������ͷ Authorization ���ð�ť��
builder.Services.AddSwaggerGen(options =>
{
	var scheme = new OpenApiSecurityScheme()
	{
		// ˵����������Ϣ
		Description = "Authorization header.\r\nExample:'Bearer 123456abcde'",
		// һ���򵥵Ķ����������ڲ����ⲿ���ù淶�е����������
		Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Authorization" },
		// ����ġ���RFC7235�ж������Ȩͷ��ʹ�õ�HTTP��Ȩ���������ơ�
		Scheme = "oauth2",
		// ����ġ�Ҫʹ�õ�ͷ����ѯ��cookie���������ơ�
		Name = "Authorization",
		// ����ġ�API��Կ��λ�á�ȡֵΪquery��header �� cookie.
		In = ParameterLocation.Header,
		// ����ġ���ȫ���������͡�ȡֵΪapiKey��http��oauth2��openIdConnect��
		Type = SecuritySchemeType.ApiKey
	};
	// ���һ��������securityDefinitions�����������API����α������ġ����ɵ�Swagger
	options.AddSecurityDefinition("Authorization", scheme);
	var requirement = new OpenApiSecurityRequirement();
	requirement[scheme] = new List<string>();
	// ���ȫ�ְ�ȫ����
	options.AddSecurityRequirement(requirement);
});


// ע�� Redis ���� 
builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = "localhost";
	// ���û���� Key ǰ׺��
	options.InstanceName = "ZZ_";
});


// ��־
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

// ������ʾ�̬�ļ��������ṩ Web ��Ŀ¼����ļ�
// FileProvider��������Դ��λ���ļ�ϵͳ��
// RequestPath��ӳ�䵽��̬��Դ���������·����
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(configFilePath),
	RequestPath = "/image"
});

// ���ÿ��򣬷��� app.UseAuthorization() �м��ǰ
app.UseCors();

// �� app.UseAuthorization() ǰ��� app.UseAuthentication() �м��;
// ���Զ��û����������֤��Ȼ��Ż������û����ʰ�ȫ��Դ��
app.UseAuthentication();

// ������Ȩ�û����ʰ�ȫ��Դ����Ȩ�м��
app.UseAuthorization();

// ע���Զ����м��
app.UseMiddleware<JWTMiddleware>();

app.MapControllers();

app.Run();
