using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaBox.Storing.Migrations
{
    public partial class _4rdmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderPizzas",
                table: "OrderPizzas");

            migrationBuilder.AddColumn<int>(
                name: "PizzaOrderId",
                table: "OrderPizzas",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderPizzas",
                table: "OrderPizzas",
                column: "PizzaOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPizzas_OrderId",
                table: "OrderPizzas",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderPizzas",
                table: "OrderPizzas");

            migrationBuilder.DropIndex(
                name: "IX_OrderPizzas_OrderId",
                table: "OrderPizzas");

            migrationBuilder.DropColumn(
                name: "PizzaOrderId",
                table: "OrderPizzas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderPizzas",
                table: "OrderPizzas",
                columns: new[] { "OrderId", "PizzaId" });
        }
    }
}
