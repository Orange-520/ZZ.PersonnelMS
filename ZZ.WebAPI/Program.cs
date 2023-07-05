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
	//����ѭ������
	option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* ע�᷶Χ���� */
// ʲô�Ƿ�Χ�����Ǵ˴���������У���ͬ��ͨ�����캯��ע��Ķ��󣬶���ͬһ����
//builder.Services.AddScoped<IdentityService>();
builder.Services.AddScoped<IdentityRepository>();
builder.Services.AddScoped<DepartmentRepository>();
builder.Services.AddScoped<PositionRepository>();
builder.Services.AddScoped<JoinUsRepository>();
builder.Services.AddScoped<OfficeRepository>();
//builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
//builder.Services.AddScoped<ICommonRepository, CommonRepository>();
builder.Services.AddScoped<IDistributedCacheHelper, DistributedCacheHelper>();

// ע��JWT���ɷ���
builder.Services.AddScoped<IJWTService, JWTService>();


/* ע�� Filter */
builder.Services.Configure<MvcOptions>(options =>
{
	options.Filters.Add<UnitOfWorkFilter>();
});


// ����
string[] cors = builder.Configuration.GetSection("Cors").GetChildren().Select(e => e.Value).ToArray()!;
if (!cors.Any())
{
	Console.WriteLine("û���������������ʵĵ�ַ");
}
else
{
    foreach (var item in cors)
    {
        Console.WriteLine("CORS֮������ʵ���ַ��{0}", item);
    }
    builder.Services.AddCors(option =>
    {
        option.AddDefaultPolicy(nb =>
        {
            nb.WithOrigins(cors).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        });
    });
}



// ע�����ݿ�������
builder.Services.AddDbContext<MyDbContext>(option =>
{
	string? conn = Environment.GetEnvironmentVariable("DefaultDB:ConnStr2");
	option.UseSqlServer(conn);
});


// �����ݱ���������ӵ�������ע�������У��÷�������ڼ��ܻ����Ӧ�ó����е����ݡ�
// ͨ�����ַ�ʽ���������ɵ�ʹ�� Data Protection API �����ܺͱ���Ӧ�ó����е�������Ϣ��
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



// ��ȡappsettings.json�����ļ��е���Ϣ
// ��ʽһ������ʵ���࣬ʹ��ʵ�����Ӧ�����Ի�ȡ��
//JWTSettings jwtSettings = builder.Configuration.GetSection("JWT").Get<JWTSettings>();

// ��ʽ����ֱ�ӻ�ȡJwt��������ΪIssuer��������
//string Issuer = builder.Configuration["JWT:Issuer"];


// �����������Service Container��ע��һ����ΪJWTOptions�����ö�������������Ϣ��Դ��Ӧ�ó���������ļ���appsettings.json������ΪJWT�Ľڵ㡣
// �ڴ�����ʹ��IOptions<JWTOptions>����ע�뷽ʽ������Ҫ����JWTѡ��ĵط������ܹ���ȡ���������ļ��ж�ȡ�������
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));

// Ϊ Swagger ��������������ͷ
builder.Services.Configure<SwaggerGenOptions>(c =>
{
    c.AddAuthenticationHeader();
});

//// ��Ȩ��Ȩ����֤
//builder.Services.AddAuthorization(options =>
//{
//	// ������Ȩ
//	options.AddPolicy("AdminAndPersonnelInCharge", policy =>
//		policy.RequireRole("admin,personnelInCharge")
//	);
//});
// ��Ȩ��Ȩ����֤
builder.Services.AddAuthorization();


// ���������֤
JWTOptions jwtOpt = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
builder.Services.AddJWTAuthentication(jwtOpt);


// ע�� Redis ���� 
builder.Services.AddStackExchangeRedisCache(options =>
{
	// ���� Redis �ķ�������ַ��Ĭ�� Redis �ķ�������ַ�� 127.0.0.1
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

// ע���Զ����м����������Ҫ�ˣ�
//app.UseMiddleware<JWTMiddleware>();

app.MapControllers();

app.Run();
