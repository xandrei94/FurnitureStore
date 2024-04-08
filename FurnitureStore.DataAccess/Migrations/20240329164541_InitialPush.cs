using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FurnitureStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialPush : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAvailableForPurchase = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "ImageUrl", "IsAvailableForPurchase", "ListPrice", "Manufacturer", "Name", "Price", "ProductId", "Supplier" },
                values: new object[,]
                {
                    { 1, "Chairs", "High-back executive office chair with ergonomic design", "", true, 250.0, "Manufacturer X", "Executive Office Chair", 199.99000000000001, "CH001", "Supplier A" },
                    { 2, "Tables", "Large rectangular conference table with wooden finish", "", true, 800.0, "Manufacturer Y", "Conference Table", 649.99000000000001, "TB002", "Supplier B" },
                    { 3, "Shelves", "Tall wooden bookshelf with adjustable shelves", "", true, 180.0, "Manufacturer Z", "Bookshelf", 149.99000000000001, "SH003", "Supplier C" },
                    { 4, "Workstations", "Compact computer workstation with keyboard tray", "", true, 300.0, "Manufacturer W", "Computer Workstation", 249.99000000000001, "WS004", "Supplier D" },
                    { 5, "Chairs", "Mesh-back office chair with lumbar support", "", true, 150.0, "Manufacturer Q", "Mesh Office Chair", 119.98999999999999, "CH005", "Supplier E" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
