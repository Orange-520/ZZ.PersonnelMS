using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZZ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "语言能力",
                table: "T_Resumes",
                newName: "SchoolOfGraduation");

            migrationBuilder.RenameColumn(
                name: "紧急联系人号码",
                table: "T_Resumes",
                newName: "RelationshipWith");

            migrationBuilder.RenameColumn(
                name: "紧急联系人",
                table: "T_Resumes",
                newName: "ProfessionalSkill");

            migrationBuilder.RenameColumn(
                name: "现居住地",
                table: "T_Resumes",
                newName: "Major");

            migrationBuilder.RenameColumn(
                name: "求职方式",
                table: "T_Resumes",
                newName: "JoinUsStep");

            migrationBuilder.RenameColumn(
                name: "毕业院校",
                table: "T_Resumes",
                newName: "LanguageCompetence");

            migrationBuilder.RenameColumn(
                name: "应聘环节",
                table: "T_Resumes",
                newName: "JobHuntingMode");

            migrationBuilder.RenameColumn(
                name: "学历",
                table: "T_Resumes",
                newName: "CurrentEducation");

            migrationBuilder.RenameColumn(
                name: "兴趣特长",
                table: "T_Resumes",
                newName: "InterestsAndTalents");

            migrationBuilder.RenameColumn(
                name: "健康状况",
                table: "T_Resumes",
                newName: "HealthCondition");

            migrationBuilder.RenameColumn(
                name: "专业技能",
                table: "T_Resumes",
                newName: "EmergencyContactPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "专业",
                table: "T_Resumes",
                newName: "EmergencyContact");

            migrationBuilder.RenameColumn(
                name: "与其关系",
                table: "T_Resumes",
                newName: "CurrentAddress");

            migrationBuilder.RenameColumn(
                name: "转正日期",
                table: "T_Records",
                newName: "DateOfConfirmationAfterProbation");

            migrationBuilder.RenameColumn(
                name: "身份证复印件",
                table: "T_Records",
                newName: "TypeOfSocialSecurity");

            migrationBuilder.RenameColumn(
                name: "购买社保类型",
                table: "T_Records",
                newName: "SocialSecurityCardNumber");

            migrationBuilder.RenameColumn(
                name: "语言能力",
                table: "T_Records",
                newName: "SchoolOfGraduation");

            migrationBuilder.RenameColumn(
                name: "紧急联系人号码",
                table: "T_Records",
                newName: "RelationshipWith");

            migrationBuilder.RenameColumn(
                name: "紧急联系人",
                table: "T_Records",
                newName: "ProfessionalSkill");

            migrationBuilder.RenameColumn(
                name: "离职日期",
                table: "T_Records",
                newName: "LeaveTime");

            migrationBuilder.RenameColumn(
                name: "社保卡号",
                table: "T_Records",
                newName: "IdCardPicture");

            migrationBuilder.RenameColumn(
                name: "相片",
                table: "T_Records",
                newName: "Avatar");

            migrationBuilder.RenameColumn(
                name: "用工性质",
                table: "T_Records",
                newName: "NatureOfEmployment");

            migrationBuilder.RenameColumn(
                name: "现居住地",
                table: "T_Records",
                newName: "Major");

            migrationBuilder.RenameColumn(
                name: "求职方式",
                table: "T_Records",
                newName: "JobHuntingMode");

            migrationBuilder.RenameColumn(
                name: "毕业院校",
                table: "T_Records",
                newName: "LanguageCompetence");

            migrationBuilder.RenameColumn(
                name: "学历",
                table: "T_Records",
                newName: "CurrentEducation");

            migrationBuilder.RenameColumn(
                name: "兴趣特长",
                table: "T_Records",
                newName: "InterestsAndTalents");

            migrationBuilder.RenameColumn(
                name: "入职时间",
                table: "T_Records",
                newName: "EntryTime");

            migrationBuilder.RenameColumn(
                name: "健康状况",
                table: "T_Records",
                newName: "HealthCondition");

            migrationBuilder.RenameColumn(
                name: "专业技能",
                table: "T_Records",
                newName: "EmergencyContactPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "专业",
                table: "T_Records",
                newName: "EmergencyContact");

            migrationBuilder.RenameColumn(
                name: "与其关系",
                table: "T_Records",
                newName: "CurrentAddress");

            migrationBuilder.RenameColumn(
                name: "学历",
                table: "T_EducationHistorys",
                newName: "LearningStyle");

            migrationBuilder.RenameColumn(
                name: "学位授予时间",
                table: "T_EducationHistorys",
                newName: "DegreeCreateTime");

            migrationBuilder.RenameColumn(
                name: "学位",
                table: "T_EducationHistorys",
                newName: "Degree");

            migrationBuilder.RenameColumn(
                name: "学习方式",
                table: "T_EducationHistorys",
                newName: "CurrentEducation");

            migrationBuilder.RenameColumn(
                name: "专业",
                table: "T_EducationHistorys",
                newName: "Major");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SchoolOfGraduation",
                table: "T_Resumes",
                newName: "语言能力");

            migrationBuilder.RenameColumn(
                name: "RelationshipWith",
                table: "T_Resumes",
                newName: "紧急联系人号码");

            migrationBuilder.RenameColumn(
                name: "ProfessionalSkill",
                table: "T_Resumes",
                newName: "紧急联系人");

            migrationBuilder.RenameColumn(
                name: "Major",
                table: "T_Resumes",
                newName: "现居住地");

            migrationBuilder.RenameColumn(
                name: "LanguageCompetence",
                table: "T_Resumes",
                newName: "毕业院校");

            migrationBuilder.RenameColumn(
                name: "JoinUsStep",
                table: "T_Resumes",
                newName: "求职方式");

            migrationBuilder.RenameColumn(
                name: "JobHuntingMode",
                table: "T_Resumes",
                newName: "应聘环节");

            migrationBuilder.RenameColumn(
                name: "InterestsAndTalents",
                table: "T_Resumes",
                newName: "兴趣特长");

            migrationBuilder.RenameColumn(
                name: "HealthCondition",
                table: "T_Resumes",
                newName: "健康状况");

            migrationBuilder.RenameColumn(
                name: "EmergencyContactPhoneNumber",
                table: "T_Resumes",
                newName: "专业技能");

            migrationBuilder.RenameColumn(
                name: "EmergencyContact",
                table: "T_Resumes",
                newName: "专业");

            migrationBuilder.RenameColumn(
                name: "CurrentEducation",
                table: "T_Resumes",
                newName: "学历");

            migrationBuilder.RenameColumn(
                name: "CurrentAddress",
                table: "T_Resumes",
                newName: "与其关系");

            migrationBuilder.RenameColumn(
                name: "TypeOfSocialSecurity",
                table: "T_Records",
                newName: "身份证复印件");

            migrationBuilder.RenameColumn(
                name: "SocialSecurityCardNumber",
                table: "T_Records",
                newName: "购买社保类型");

            migrationBuilder.RenameColumn(
                name: "SchoolOfGraduation",
                table: "T_Records",
                newName: "语言能力");

            migrationBuilder.RenameColumn(
                name: "RelationshipWith",
                table: "T_Records",
                newName: "紧急联系人号码");

            migrationBuilder.RenameColumn(
                name: "ProfessionalSkill",
                table: "T_Records",
                newName: "紧急联系人");

            migrationBuilder.RenameColumn(
                name: "NatureOfEmployment",
                table: "T_Records",
                newName: "用工性质");

            migrationBuilder.RenameColumn(
                name: "Major",
                table: "T_Records",
                newName: "现居住地");

            migrationBuilder.RenameColumn(
                name: "LeaveTime",
                table: "T_Records",
                newName: "离职日期");

            migrationBuilder.RenameColumn(
                name: "LanguageCompetence",
                table: "T_Records",
                newName: "毕业院校");

            migrationBuilder.RenameColumn(
                name: "JobHuntingMode",
                table: "T_Records",
                newName: "求职方式");

            migrationBuilder.RenameColumn(
                name: "InterestsAndTalents",
                table: "T_Records",
                newName: "兴趣特长");

            migrationBuilder.RenameColumn(
                name: "IdCardPicture",
                table: "T_Records",
                newName: "社保卡号");

            migrationBuilder.RenameColumn(
                name: "HealthCondition",
                table: "T_Records",
                newName: "健康状况");

            migrationBuilder.RenameColumn(
                name: "EntryTime",
                table: "T_Records",
                newName: "入职时间");

            migrationBuilder.RenameColumn(
                name: "EmergencyContactPhoneNumber",
                table: "T_Records",
                newName: "专业技能");

            migrationBuilder.RenameColumn(
                name: "EmergencyContact",
                table: "T_Records",
                newName: "专业");

            migrationBuilder.RenameColumn(
                name: "DateOfConfirmationAfterProbation",
                table: "T_Records",
                newName: "转正日期");

            migrationBuilder.RenameColumn(
                name: "CurrentEducation",
                table: "T_Records",
                newName: "学历");

            migrationBuilder.RenameColumn(
                name: "CurrentAddress",
                table: "T_Records",
                newName: "与其关系");

            migrationBuilder.RenameColumn(
                name: "Avatar",
                table: "T_Records",
                newName: "相片");

            migrationBuilder.RenameColumn(
                name: "Major",
                table: "T_EducationHistorys",
                newName: "专业");

            migrationBuilder.RenameColumn(
                name: "LearningStyle",
                table: "T_EducationHistorys",
                newName: "学历");

            migrationBuilder.RenameColumn(
                name: "DegreeCreateTime",
                table: "T_EducationHistorys",
                newName: "学位授予时间");

            migrationBuilder.RenameColumn(
                name: "Degree",
                table: "T_EducationHistorys",
                newName: "学位");

            migrationBuilder.RenameColumn(
                name: "CurrentEducation",
                table: "T_EducationHistorys",
                newName: "学习方式");
        }
    }
}
