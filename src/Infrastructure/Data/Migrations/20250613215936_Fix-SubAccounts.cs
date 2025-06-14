using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixSubAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubAccount_Accounts_AccountId",
                table: "SubAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_SubsidiaryJournals_SubAccount_SubAccountId",
                table: "SubsidiaryJournals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubAccount",
                table: "SubAccount");

            migrationBuilder.RenameTable(
                name: "SubAccount",
                newName: "SubAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_SubAccount_AccountId",
                table: "SubAccounts",
                newName: "IX_SubAccounts_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubAccounts",
                table: "SubAccounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubAccounts_Accounts_AccountId",
                table: "SubAccounts",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubsidiaryJournals_SubAccounts_SubAccountId",
                table: "SubsidiaryJournals",
                column: "SubAccountId",
                principalTable: "SubAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubAccounts_Accounts_AccountId",
                table: "SubAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_SubsidiaryJournals_SubAccounts_SubAccountId",
                table: "SubsidiaryJournals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubAccounts",
                table: "SubAccounts");

            migrationBuilder.RenameTable(
                name: "SubAccounts",
                newName: "SubAccount");

            migrationBuilder.RenameIndex(
                name: "IX_SubAccounts_AccountId",
                table: "SubAccount",
                newName: "IX_SubAccount_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubAccount",
                table: "SubAccount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubAccount_Accounts_AccountId",
                table: "SubAccount",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubsidiaryJournals_SubAccount_SubAccountId",
                table: "SubsidiaryJournals",
                column: "SubAccountId",
                principalTable: "SubAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
