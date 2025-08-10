using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirocGanpati.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtiEveningTimes",
                columns: table => new
                {
                    ArtiEveningTimeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtiEveningTimes", x => x.ArtiEveningTimeId);
                });

            migrationBuilder.CreateTable(
                name: "ArtiMorningTimes",
                columns: table => new
                {
                    ArtiMorningTimeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtiMorningTimes", x => x.ArtiMorningTimeId);
                });

            migrationBuilder.CreateTable(
                name: "DateOfVisarjans",
                columns: table => new
                {
                    DateOfVisarjanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateOfVisarjans", x => x.DateOfVisarjanId);
                });

            migrationBuilder.CreateTable(
                name: "MandalAreas",
                columns: table => new
                {
                    AreaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MandalAreas", x => x.AreaId);
                });

            migrationBuilder.CreateTable(
                name: "OtpMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OtpCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MandalName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MandalDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    ArtiMorningTimeId = table.Column<int>(type: "int", nullable: false),
                    ArtiEveningTimeId = table.Column<int>(type: "int", nullable: false),
                    DateOfVisarjanId = table.Column<int>(type: "int", nullable: false),
                    RazorpayOrderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RazorpayPaymentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RazorpaySignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_ArtiEveningTimes_ArtiEveningTimeId",
                        column: x => x.ArtiEveningTimeId,
                        principalTable: "ArtiEveningTimes",
                        principalColumn: "ArtiEveningTimeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_ArtiMorningTimes_ArtiMorningTimeId",
                        column: x => x.ArtiMorningTimeId,
                        principalTable: "ArtiMorningTimes",
                        principalColumn: "ArtiMorningTimeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_DateOfVisarjans_DateOfVisarjanId",
                        column: x => x.DateOfVisarjanId,
                        principalTable: "DateOfVisarjans",
                        principalColumn: "DateOfVisarjanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_MandalAreas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "MandalAreas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                });

            migrationBuilder.CreateTable(
                name: "Mandals",
                columns: table => new
                {
                    MandalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MandalName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MandalSlug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MandalDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    ArtiMorningTimeId = table.Column<int>(type: "int", nullable: false),
                    ArtiEveningTimeId = table.Column<int>(type: "int", nullable: false),
                    DateOfVisarjanId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mandals", x => x.MandalId);
                    table.ForeignKey(
                        name: "FK_Mandals_ArtiEveningTimes_ArtiEveningTimeId",
                        column: x => x.ArtiEveningTimeId,
                        principalTable: "ArtiEveningTimes",
                        principalColumn: "ArtiEveningTimeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mandals_ArtiMorningTimes_ArtiMorningTimeId",
                        column: x => x.ArtiMorningTimeId,
                        principalTable: "ArtiMorningTimes",
                        principalColumn: "ArtiMorningTimeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mandals_DateOfVisarjans_DateOfVisarjanId",
                        column: x => x.DateOfVisarjanId,
                        principalTable: "DateOfVisarjans",
                        principalColumn: "DateOfVisarjanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mandals_MandalAreas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "MandalAreas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsMobileVerified = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    MandalId = table.Column<int>(type: "int", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Mandals_MandalId",
                        column: x => x.MandalId,
                        principalTable: "Mandals",
                        principalColumn: "MandalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    MandalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_Records_Mandals_MandalId",
                        column: x => x.MandalId,
                        principalTable: "Mandals",
                        principalColumn: "MandalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Records_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_RecordId",
                table: "Documents",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Mandals_AreaId",
                table: "Mandals",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mandals_ArtiEveningTimeId",
                table: "Mandals",
                column: "ArtiEveningTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Mandals_ArtiMorningTimeId",
                table: "Mandals",
                column: "ArtiMorningTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Mandals_DateOfVisarjanId",
                table: "Mandals",
                column: "DateOfVisarjanId");

            migrationBuilder.CreateIndex(
                name: "IX_Mandals_MandalSlug",
                table: "Mandals",
                column: "MandalSlug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mandals_UpdatedBy",
                table: "Mandals",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AreaId",
                table: "Payments",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ArtiEveningTimeId",
                table: "Payments",
                column: "ArtiEveningTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ArtiMorningTimeId",
                table: "Payments",
                column: "ArtiMorningTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DateOfVisarjanId",
                table: "Payments",
                column: "DateOfVisarjanId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_CreatedAt",
                table: "Records",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Records_MandalId",
                table: "Records",
                column: "MandalId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_UpdatedBy",
                table: "Records",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_MandalId",
                table: "Users",
                column: "MandalId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Mobile",
                table: "Users",
                column: "Mobile",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Records_RecordId",
                table: "Documents",
                column: "RecordId",
                principalTable: "Records",
                principalColumn: "RecordId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mandals_Users_UpdatedBy",
                table: "Mandals",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mandals_ArtiEveningTimes_ArtiEveningTimeId",
                table: "Mandals");

            migrationBuilder.DropForeignKey(
                name: "FK_Mandals_ArtiMorningTimes_ArtiMorningTimeId",
                table: "Mandals");

            migrationBuilder.DropForeignKey(
                name: "FK_Mandals_DateOfVisarjans_DateOfVisarjanId",
                table: "Mandals");

            migrationBuilder.DropForeignKey(
                name: "FK_Mandals_MandalAreas_AreaId",
                table: "Mandals");

            migrationBuilder.DropForeignKey(
                name: "FK_Mandals_Users_UpdatedBy",
                table: "Mandals");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "OtpMessages");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "ArtiEveningTimes");

            migrationBuilder.DropTable(
                name: "ArtiMorningTimes");

            migrationBuilder.DropTable(
                name: "DateOfVisarjans");

            migrationBuilder.DropTable(
                name: "MandalAreas");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Mandals");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
