using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LatinMedia.DataLayer.Migrations
{
    public partial class mig_Wallet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "walletTypes",
                columns: table => new
                {
                    TypeID = table.Column<int>(nullable: false),
                    TypeTitle = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_walletTypes", x => x.TypeID);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    WalletID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TypeID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    IsPay = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.WalletID);
                    table.ForeignKey(
                        name: "FK_Wallets_walletTypes_TypeID",
                        column: x => x.TypeID,
                        principalTable: "walletTypes",
                        principalColumn: "TypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wallets_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_TypeID",
                table: "Wallets",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserID",
                table: "Wallets",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "walletTypes");
        }
    }
}
