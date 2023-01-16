using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class OnlinePayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnlinePayment_Orders_OrderId",
                table: "OnlinePayment");

            migrationBuilder.DropForeignKey(
                name: "FK_OnlinePayment_WorkerAccounts_IssuerId",
                table: "OnlinePayment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OnlinePayment",
                table: "OnlinePayment");

            migrationBuilder.RenameTable(
                name: "OnlinePayment",
                newName: "OnlinePayments");

            migrationBuilder.RenameIndex(
                name: "IX_OnlinePayment_OrderId",
                table: "OnlinePayments",
                newName: "IX_OnlinePayments_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OnlinePayment_IssuerId",
                table: "OnlinePayments",
                newName: "IX_OnlinePayments_IssuerId");

            migrationBuilder.RenameIndex(
                name: "IX_OnlinePayment_IsSoftDeleted",
                table: "OnlinePayments",
                newName: "IX_OnlinePayments_IsSoftDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OnlinePayments",
                table: "OnlinePayments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OnlinePayments_Orders_OrderId",
                table: "OnlinePayments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnlinePayments_WorkerAccounts_IssuerId",
                table: "OnlinePayments",
                column: "IssuerId",
                principalTable: "WorkerAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnlinePayments_Orders_OrderId",
                table: "OnlinePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_OnlinePayments_WorkerAccounts_IssuerId",
                table: "OnlinePayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OnlinePayments",
                table: "OnlinePayments");

            migrationBuilder.RenameTable(
                name: "OnlinePayments",
                newName: "OnlinePayment");

            migrationBuilder.RenameIndex(
                name: "IX_OnlinePayments_OrderId",
                table: "OnlinePayment",
                newName: "IX_OnlinePayment_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OnlinePayments_IssuerId",
                table: "OnlinePayment",
                newName: "IX_OnlinePayment_IssuerId");

            migrationBuilder.RenameIndex(
                name: "IX_OnlinePayments_IsSoftDeleted",
                table: "OnlinePayment",
                newName: "IX_OnlinePayment_IsSoftDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OnlinePayment",
                table: "OnlinePayment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OnlinePayment_Orders_OrderId",
                table: "OnlinePayment",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnlinePayment_WorkerAccounts_IssuerId",
                table: "OnlinePayment",
                column: "IssuerId",
                principalTable: "WorkerAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
