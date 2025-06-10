using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixSubSidaryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubsidiaryJournals_Collages_CollageId",
                table: "SubsidiaryJournals");

            migrationBuilder.DropForeignKey(
                name: "FK_SubsidiaryJournals_Funds_FundId",
                table: "SubsidiaryJournals");

            migrationBuilder.DropIndex(
                name: "IX_SubsidiaryJournals_CollageId",
                table: "SubsidiaryJournals");

            migrationBuilder.DropIndex(
                name: "IX_SubsidiaryJournals_FundId",
                table: "SubsidiaryJournals");

            migrationBuilder.DropColumn(
                name: "AccountItem",
                table: "SubsidiaryJournals");

            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "SubsidiaryJournals");

            migrationBuilder.DropColumn(
                name: "CollageId",
                table: "SubsidiaryJournals");

            migrationBuilder.DropColumn(
                name: "FundId",
                table: "SubsidiaryJournals");

            migrationBuilder.DropColumn(
                name: "TransactionSide",
                table: "SubsidiaryJournals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountItem",
                table: "SubsidiaryJournals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountType",
                table: "SubsidiaryJournals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CollageId",
                table: "SubsidiaryJournals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FundId",
                table: "SubsidiaryJournals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionSide",
                table: "SubsidiaryJournals",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SubsidiaryJournals_CollageId",
                table: "SubsidiaryJournals",
                column: "CollageId");

            migrationBuilder.CreateIndex(
                name: "IX_SubsidiaryJournals_FundId",
                table: "SubsidiaryJournals",
                column: "FundId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubsidiaryJournals_Collages_CollageId",
                table: "SubsidiaryJournals",
                column: "CollageId",
                principalTable: "Collages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubsidiaryJournals_Funds_FundId",
                table: "SubsidiaryJournals",
                column: "FundId",
                principalTable: "Funds",
                principalColumn: "Id");
        }
    }
}
