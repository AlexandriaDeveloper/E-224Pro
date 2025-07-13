using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixUserAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "UserAccounts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "UserAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "UserAccounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "TempId",
                table: "UserAccounts",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_AppUserId",
                table: "UserAccounts",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_AspNetUsers_AppUserId",
                table: "UserAccounts",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_AspNetUsers_AppUserId",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_AppUserId",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "UserAccounts");
        }
    }
}
