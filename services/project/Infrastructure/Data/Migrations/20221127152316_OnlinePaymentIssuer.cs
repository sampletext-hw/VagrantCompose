using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class OnlinePaymentIssuer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "IssuerId",
                table: "OnlinePayment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_OnlinePayment_IssuerId",
                table: "OnlinePayment",
                column: "IssuerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OnlinePayment_WorkerAccounts_IssuerId",
                table: "OnlinePayment",
                column: "IssuerId",
                principalTable: "WorkerAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnlinePayment_WorkerAccounts_IssuerId",
                table: "OnlinePayment");

            migrationBuilder.DropIndex(
                name: "IX_OnlinePayment_IssuerId",
                table: "OnlinePayment");

            migrationBuilder.DropColumn(
                name: "IssuerId",
                table: "OnlinePayment");
        }
    }
}
