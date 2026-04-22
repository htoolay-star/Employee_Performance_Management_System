using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "hr");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "hr",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeProfiles",
                schema: "hr",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    StaffNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OtherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NRCNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Race = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BirthPlace = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PassportNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LabourRegistrationNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    PassportExpireDate = table.Column<DateOnly>(type: "date", nullable: true),
                    WorkPermitNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkPermitValidDate = table.Column<DateOnly>(type: "date", nullable: true),
                    WorkPermitExpireDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                schema: "hr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                schema: "hr",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DepartmentId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "hr",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teams_Departments_DepartmentId1",
                        column: x => x.DepartmentId1,
                        principalSchema: "hr",
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeContacts",
                schema: "hr",
                columns: table => new
                {
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ContactAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PermanentAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PermanentPhoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PresentPhoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InternalPhoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmergencyMobileNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RelationWithEmergencyContact = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeContacts", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_EmployeeContacts_EmployeeProfiles_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "EmployeeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeFamilyInfo",
                schema: "hr",
                columns: table => new
                {
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SpouseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SpouseNRCNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SpouseOccupation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FatherNRCNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FatherOccupation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeFamilyInfo", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_EmployeeFamilyInfo_EmployeeProfiles_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "EmployeeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePayrollInfo",
                schema: "hr",
                columns: table => new
                {
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PayType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateOfPayTypeChanged = table.Column<DateOnly>(type: "date", nullable: true),
                    CostAllocate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayByBacklog = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SSBStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SSCBNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ComplianceEarnedPoints = table.Column<int>(type: "int", nullable: true),
                    ComplianceBalancePoints = table.Column<int>(type: "int", nullable: true),
                    DateOfSalaryChanged = table.Column<DateOnly>(type: "date", nullable: true),
                    DateOfCurrencyChange = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePayrollInfo", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_EmployeePayrollInfo_EmployeeProfiles_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "EmployeeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                schema: "hr",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LevelId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Positions_Levels_LevelId",
                        column: x => x.LevelId,
                        principalSchema: "hr",
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, computedColumnSql: "upper([Email])", stored: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    IsFirstLogin = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    LockoutEndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastLoginDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEmployment",
                schema: "hr",
                columns: table => new
                {
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    ParentDepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    TeamId = table.Column<long>(type: "bigint", nullable: true),
                    PositionId = table.Column<long>(type: "bigint", nullable: false),
                    DirectManagerId = table.Column<long>(type: "bigint", nullable: true),
                    EmploymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StaffType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProbationMonth = table.Column<int>(type: "int", nullable: true),
                    DateOfAppointment = table.Column<DateOnly>(type: "date", nullable: true),
                    DateOfConfirmation = table.Column<DateOnly>(type: "date", nullable: true),
                    DateOfPromotion = table.Column<DateOnly>(type: "date", nullable: true),
                    DateOfTermination = table.Column<DateOnly>(type: "date", nullable: true),
                    DateOfTransfer = table.Column<DateOnly>(type: "date", nullable: true),
                    DateOfDemotion = table.Column<DateOnly>(type: "date", nullable: true),
                    DateOfTitleChange = table.Column<DateOnly>(type: "date", nullable: true),
                    Shift = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FingerPrintId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MobileAttendance = table.Column<bool>(type: "bit", nullable: false),
                    DateOfIncrement = table.Column<DateOnly>(type: "date", nullable: true),
                    ProductProject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEmployment", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_EmployeeEmployment_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "hr",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeEmployment_Departments_ParentDepartmentId",
                        column: x => x.ParentDepartmentId,
                        principalSchema: "hr",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeEmployment_EmployeeProfiles_DirectManagerId",
                        column: x => x.DirectManagerId,
                        principalSchema: "hr",
                        principalTable: "EmployeeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeEmployment_EmployeeProfiles_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "EmployeeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeEmployment_Positions_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "hr",
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeEmployment_Teams_TeamId",
                        column: x => x.TeamId,
                        principalSchema: "hr",
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRefreshTokens",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Token = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    JwtId = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code",
                schema: "hr",
                table: "Departments",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name",
                schema: "hr",
                table: "Departments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEmployment_DepartmentId",
                schema: "hr",
                table: "EmployeeEmployment",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEmployment_DirectManagerId",
                schema: "hr",
                table: "EmployeeEmployment",
                column: "DirectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEmployment_ParentDepartmentId",
                schema: "hr",
                table: "EmployeeEmployment",
                column: "ParentDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEmployment_PositionId",
                schema: "hr",
                table: "EmployeeEmployment",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEmployment_TeamId",
                schema: "hr",
                table: "EmployeeEmployment",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfiles_StaffNo",
                schema: "hr",
                table: "EmployeeProfiles",
                column: "StaffNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfiles_UserId",
                schema: "hr",
                table: "EmployeeProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Levels_Code",
                schema: "hr",
                table: "Levels",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_LevelId",
                schema: "hr",
                table: "Positions",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_Title",
                schema: "hr",
                table: "Positions",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_DepartmentId_Name",
                schema: "hr",
                table: "Teams",
                columns: new[] { "DepartmentId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_DepartmentId1",
                schema: "hr",
                table: "Teams",
                column: "DepartmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_auth_UserRefreshTokens_Token",
                schema: "auth",
                table: "UserRefreshTokens",
                column: "Token");

            migrationBuilder.CreateIndex(
                name: "IX_auth_UserRefreshTokens_UserId",
                schema: "auth",
                table: "UserRefreshTokens",
                column: "UserId")
                .Annotation("SqlServer:Include", new[] { "IsRevoked", "IsUsed" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedEmail",
                schema: "auth",
                table: "Users",
                column: "NormalizedEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                schema: "auth",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserGuid",
                schema: "auth",
                table: "Users",
                column: "UserGuid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeContacts",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "EmployeeEmployment",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "EmployeeFamilyInfo",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "EmployeePayrollInfo",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "UserRefreshTokens",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "Positions",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "Teams",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "EmployeeProfiles",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "Levels",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "auth");
        }
    }
}
