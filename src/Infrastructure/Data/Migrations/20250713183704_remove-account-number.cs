using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class removeaccountnumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempId",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "SubsidiaryJournals");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "SubAccounts");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "GeneralJournal");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "Funds");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "Dailies");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "Collages");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "Accounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TempId",
                table: "UserAccounts",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TempId",
                table: "SubsidiaryJournals",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TempId",
                table: "SubAccounts",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TempId",
                table: "GeneralJournal",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TempId",
                table: "Funds",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TempId",
                table: "Forms",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TempId",
                table: "Dailies",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TempId",
                table: "Collages",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TempId",
                table: "AuditLogs",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Accounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "TempId",
                table: "Accounts",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: true);
        }
    }
}
