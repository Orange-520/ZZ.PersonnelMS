using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZZ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentDepartmenId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Departments_T_Departments_ParentDepartmenId",
                        column: x => x.ParentDepartmenId,
                        principalTable: "T_Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "T_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ListModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Positions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Positions_T_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "T_Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_AskForLeaveApplys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplyType = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckState = table.Column<int>(type: "int", nullable: false),
                    CheckUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ListModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AskForLeaveApplys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_AskForLeaveApplys_T_Users_CheckUserId",
                        column: x => x.CheckUserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_AskForLeaveApplys_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_Certificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GetTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertificateNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Organization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Certificates_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_Notices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoticeType = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ListModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsTop = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Notices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Notices_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_HiringNeedApplys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    NeedPersonCount = table.Column<int>(type: "int", nullable: false),
                    HasPersonCount = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplyType = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckState = table.Column<int>(type: "int", nullable: false),
                    CheckUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ListModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_HiringNeedApplys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_HiringNeedApplys_T_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "T_Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_HiringNeedApplys_T_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "T_Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_HiringNeedApplys_T_Users_CheckUserId",
                        column: x => x.CheckUserId,
                        principalTable: "T_Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_HiringNeedApplys_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_Records",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    入职时间 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    相片 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    用工性质 = table.Column<int>(type: "int", nullable: false),
                    身份证复印件 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    转正日期 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    购买社保类型 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    社保卡号 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    离职日期 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NativePlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PoliticsStatus = table.Column<int>(type: "int", nullable: true),
                    毕业院校 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    学历 = table.Column<int>(type: "int", nullable: true),
                    专业 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    健康状况 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    语言能力 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    求职方式 = table.Column<int>(type: "int", nullable: false),
                    兴趣特长 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    专业技能 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    现居住地 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    紧急联系人 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    与其关系 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    紧急联系人号码 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Records_T_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "T_Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_Records_T_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "T_Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_Records_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoticeId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Roles_T_Notices_NoticeId",
                        column: x => x.NoticeId,
                        principalTable: "T_Notices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "T_Resumes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    SalaryExpectation = table.Column<decimal>(type: "money", nullable: true),
                    应聘环节 = table.Column<int>(type: "int", nullable: false),
                    JoinUsResult = table.Column<int>(type: "int", nullable: false),
                    HiringNeedApplyId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NativePlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PoliticsStatus = table.Column<int>(type: "int", nullable: true),
                    毕业院校 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    学历 = table.Column<int>(type: "int", nullable: true),
                    专业 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    健康状况 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    语言能力 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    求职方式 = table.Column<int>(type: "int", nullable: false),
                    兴趣特长 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    专业技能 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    现居住地 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    紧急联系人 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    与其关系 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    紧急联系人号码 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Resumes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Resumes_T_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "T_Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_Resumes_T_HiringNeedApplys_HiringNeedApplyId",
                        column: x => x.HiringNeedApplyId,
                        principalTable: "T_HiringNeedApplys",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_Resumes_T_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "T_Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_T_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "T_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_T_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "T_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_EducationHistorys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    学历 = table.Column<int>(type: "int", nullable: false),
                    专业 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    学位 = table.Column<int>(type: "int", nullable: true),
                    学位授予时间 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    学习方式 = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<float>(type: "real", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordId = table.Column<int>(type: "int", nullable: true),
                    ResumeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_EducationHistorys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_EducationHistorys_T_Records_RecordId",
                        column: x => x.RecordId,
                        principalTable: "T_Records",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_EducationHistorys_T_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "T_Resumes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_EducationHistorys_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_WorkHistorys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonalInformationId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DimissionCause = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BiggestGain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordId = table.Column<int>(type: "int", nullable: true),
                    ResumeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_WorkHistorys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_WorkHistorys_T_Records_RecordId",
                        column: x => x.RecordId,
                        principalTable: "T_Records",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_WorkHistorys_T_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "T_Resumes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_T_AskForLeaveApplys_CheckUserId",
                table: "T_AskForLeaveApplys",
                column: "CheckUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_AskForLeaveApplys_UserId",
                table: "T_AskForLeaveApplys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Certificates_UserId",
                table: "T_Certificates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Departments_ParentDepartmenId",
                table: "T_Departments",
                column: "ParentDepartmenId");

            migrationBuilder.CreateIndex(
                name: "IX_T_EducationHistorys_RecordId",
                table: "T_EducationHistorys",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_T_EducationHistorys_ResumeId",
                table: "T_EducationHistorys",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_T_EducationHistorys_UserId",
                table: "T_EducationHistorys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_HiringNeedApplys_CheckUserId",
                table: "T_HiringNeedApplys",
                column: "CheckUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_HiringNeedApplys_DepartmentId",
                table: "T_HiringNeedApplys",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_T_HiringNeedApplys_PositionId",
                table: "T_HiringNeedApplys",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_T_HiringNeedApplys_UserId",
                table: "T_HiringNeedApplys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Notices_UserId",
                table: "T_Notices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Positions_DepartmentId",
                table: "T_Positions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Records_DepartmentId",
                table: "T_Records",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Records_PositionId",
                table: "T_Records",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Records_UserId",
                table: "T_Records",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Resumes_DepartmentId",
                table: "T_Resumes",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Resumes_HiringNeedApplyId",
                table: "T_Resumes",
                column: "HiringNeedApplyId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Resumes_PositionId",
                table: "T_Resumes",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Roles_NoticeId",
                table: "T_Roles",
                column: "NoticeId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "T_Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "T_Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "T_Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_T_WorkHistorys_RecordId",
                table: "T_WorkHistorys",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_T_WorkHistorys_ResumeId",
                table: "T_WorkHistorys",
                column: "ResumeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "T_AskForLeaveApplys");

            migrationBuilder.DropTable(
                name: "T_Certificates");

            migrationBuilder.DropTable(
                name: "T_EducationHistorys");

            migrationBuilder.DropTable(
                name: "T_WorkHistorys");

            migrationBuilder.DropTable(
                name: "T_Roles");

            migrationBuilder.DropTable(
                name: "T_Records");

            migrationBuilder.DropTable(
                name: "T_Resumes");

            migrationBuilder.DropTable(
                name: "T_Notices");

            migrationBuilder.DropTable(
                name: "T_HiringNeedApplys");

            migrationBuilder.DropTable(
                name: "T_Positions");

            migrationBuilder.DropTable(
                name: "T_Users");

            migrationBuilder.DropTable(
                name: "T_Departments");
        }
    }
}
