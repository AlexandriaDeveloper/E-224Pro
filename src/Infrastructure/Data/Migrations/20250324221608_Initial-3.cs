using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubsidiaryJournals_Accounts_AccountId",
                table: "SubsidiaryJournals");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SubAccount");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "SubsidiaryJournals",
                newName: "SubAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_SubsidiaryJournals_AccountId",
                table: "SubsidiaryJournals",
                newName: "IX_SubsidiaryJournals_SubAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubsidiaryJournals_SubAccount_SubAccountId",
                table: "SubsidiaryJournals",
                column: "SubAccountId",
                principalTable: "SubAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubsidiaryJournals_SubAccount_SubAccountId",
                table: "SubsidiaryJournals");

            migrationBuilder.RenameColumn(
                name: "SubAccountId",
                table: "SubsidiaryJournals",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_SubsidiaryJournals_SubAccountId",
                table: "SubsidiaryJournals",
                newName: "IX_SubsidiaryJournals_AccountId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SubAccount",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SubsidiaryJournals_Accounts_AccountId",
                table: "SubsidiaryJournals",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
