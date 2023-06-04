using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    /// <inheritdoc />
    public partial class Addedrefreshtokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => new { x.UserId, x.Id });
                    table.ForeignKey(
                        name: "FK_RefreshToken_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "Email", "PasswordHash", "SecurityStamp" },
                values: new object[] { "03d2186f-97aa-4022-a92b-e973f9074ff5", "user1@example.com", "AQAAAAIAAYagAAAAEE1fhVjZ7ScNqYG+NqlLqNKjSfRmhnqlNXJXxar3aztwa5AnBV4lidfV1yG3kDQ2oA==", "0ce7b237-8190-4f9e-864e-bf489f67da5a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9c445865-a24d-4233-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "Email", "PasswordHash", "SecurityStamp" },
                values: new object[] { "aba10d64-2f29-4377-9e9b-f963ba7bf13f", "user2@example.com", "AQAAAAIAAYagAAAAEBkuu+6p2ThH+IvsoLtBQKKOXKL/LWhob5E2POrv7rGXBHc3hKJEle6fKgLPxDfWoQ==", "2c204696-75fd-465e-8cfa-bbd82e36c9d4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "Email", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5799d9b6-0929-4673-9dc4-4c38b29e33f5", null, "AQAAAAIAAYagAAAAEBxhpfwzzCRfNZRV7g+j+ghBNaV/uuqExWrQcuqy4R58Kuk6kXQTUakq2UTZUaTq1g==", "0b8a3c4a-732d-4200-b8a8-d69279d8cc1d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9c445865-a24d-4233-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "Email", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b1810859-31d3-4c15-9630-08caa89c6d36", null, "AQAAAAIAAYagAAAAEBBBHMot9Y89XjQaoW/RlMUhVen7u6ENODd27ztHgSsa3z1LlsYENzf73iYSardBBg==", "bc1bec50-2706-4225-bc85-242838bc8a33" });
        }
    }
}
