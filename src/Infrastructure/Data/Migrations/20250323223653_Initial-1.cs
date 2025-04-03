using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TempId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    AffectedColumns = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PrimaryKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TempId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Collages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CollageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TempId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dailies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DailyDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DailyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountItem = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TempId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dailies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FundName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FundCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollageId = table.Column<int>(type: "int", nullable: false),
                    TempId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FormName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CollageId = table.Column<int>(type: "int", nullable: true),
                    FundId = table.Column<int>(type: "int", nullable: true),
                    Num224 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Num55 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TotalDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DailyId = table.Column<int>(type: "int", nullable: false),
                    AuditorName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TempId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forms_Collages_CollageId",
                        column: x => x.CollageId,
                        principalTable: "Collages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Forms_Dailies_DailyId",
                        column: x => x.DailyId,
                        principalTable: "Dailies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Forms_Funds_FundId",
                        column: x => x.FundId,
                        principalTable: "Funds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GeneralJournal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AccountType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TempId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralJournal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralJournal_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralJournal_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubsidiaryJournals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FormDetailsId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CollageId = table.Column<int>(type: "int", nullable: true),
                    FundId = table.Column<int>(type: "int", nullable: true),
                    TransactionSide = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountItem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TempId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubsidiaryJournals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubsidiaryJournals_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubsidiaryJournals_Collages_CollageId",
                        column: x => x.CollageId,
                        principalTable: "Collages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubsidiaryJournals_Funds_FundId",
                        column: x => x.FundId,
                        principalTable: "Funds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubsidiaryJournals_GeneralJournal_FormDetailsId",
                        column: x => x.FormDetailsId,
                        principalTable: "GeneralJournal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Forms_CollageId",
                table: "Forms",
                column: "CollageId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_DailyId",
                table: "Forms",
                column: "DailyId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_FundId",
                table: "Forms",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralJournal_AccountId",
                table: "GeneralJournal",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralJournal_FormId",
                table: "GeneralJournal",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_SubsidiaryJournals_AccountId",
                table: "SubsidiaryJournals",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SubsidiaryJournals_CollageId",
                table: "SubsidiaryJournals",
                column: "CollageId");

            migrationBuilder.CreateIndex(
                name: "IX_SubsidiaryJournals_FormDetailsId",
                table: "SubsidiaryJournals",
                column: "FormDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_SubsidiaryJournals_FundId",
                table: "SubsidiaryJournals",
                column: "FundId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "SubsidiaryJournals");

            migrationBuilder.DropTable(
                name: "GeneralJournal");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.DropTable(
                name: "Collages");

            migrationBuilder.DropTable(
                name: "Dailies");

            migrationBuilder.DropTable(
                name: "Funds");
        }
    }
}
