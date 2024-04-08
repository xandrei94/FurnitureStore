using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnitureStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFromProductIdToProduceCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Products",
                newName: "ProductCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductCode",
                table: "Products",
                newName: "ProductId");
        }
    }
}
