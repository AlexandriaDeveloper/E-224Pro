using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixSubSidary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "SubsidiaryJournals",
                newName: "Debit");

            migrationBuilder.AddColumn<decimal>(
                name: "Credit",
                table: "SubsidiaryJournals",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credit",
                table: "SubsidiaryJournals");

            migrationBuilder.RenameColumn(
                name: "Debit",
                table: "SubsidiaryJournals",
                newName: "Amount");
        }
    }
}
