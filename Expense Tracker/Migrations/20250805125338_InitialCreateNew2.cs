using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Expense_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateNew2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Icon", "Title", "Type" },
                values: new object[,]
                {
                    { 1, "💰", "Salary", "Income" },
                    { 2, "💼", "Freelance", "Income" },
                    { 3, "🍔", "Food", "Expense" },
                    { 4, "🚗", "Transportation", "Expense" },
                    { 5, "🛍️", "Shopping", "Expense" },
                    { 6, "💡", "Utilities", "Expense" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Date",
                table: "Transactions",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Title",
                table: "Categories",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Type",
                table: "Categories",
                column: "Type");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_Date",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Title",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Type",
                table: "Categories");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 6);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
