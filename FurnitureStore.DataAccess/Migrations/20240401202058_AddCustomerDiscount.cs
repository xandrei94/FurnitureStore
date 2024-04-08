using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnitureStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomersDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiscountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersDiscount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomersDiscount_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomersDiscount_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomersDiscount_CustomerId",
                table: "CustomersDiscount",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersDiscount_DiscountId",
                table: "CustomersDiscount",
                column: "DiscountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomersDiscount");
        }
    }
}
