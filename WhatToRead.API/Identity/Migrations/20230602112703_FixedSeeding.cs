using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    /// <inheritdoc />
    public partial class FixedSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4d6c234e-4b2e-526g-86af-483e56fd8345", null, "User", "USER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5799d9b6-0929-4673-9dc4-4c38b29e33f5", "AQAAAAIAAYagAAAAEBxhpfwzzCRfNZRV7g+j+ghBNaV/uuqExWrQcuqy4R58Kuk6kXQTUakq2UTZUaTq1g==", "0b8a3c4a-732d-4200-b8a8-d69279d8cc1d" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "9c445865-a24d-4233-a6c6-9443d048cdb9", 0, "b1810859-31d3-4c15-9630-08caa89c6d36", null, false, "MyFirstName2", "MyLastName2", false, null, null, "MYUSER2", "AQAAAAIAAYagAAAAEBBBHMot9Y89XjQaoW/RlMUhVen7u6ENODd27ztHgSsa3z1LlsYENzf73iYSardBBg==", null, false, "bc1bec50-2706-4225-bc85-242838bc8a33", false, "MyUserName2" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "4d6c234e-4b2e-526g-86af-483e56fd8345", "9c445865-a24d-4233-a6c6-9443d048cdb9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4d6c234e-4b2e-526g-86af-483e56fd8345", "9c445865-a24d-4233-a6c6-9443d048cdb9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d6c234e-4b2e-526g-86af-483e56fd8345");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9c445865-a24d-4233-a6c6-9443d048cdb9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "80989b64-0673-41b8-894e-1f3cd7dbb3a0", "AQAAAAIAAYagAAAAEJS3MDTTRrziy5XLSBscDFHadGprNzSeKfazQndOXTNET0PfPsh9G1YSONrMLWWgsg==", "255320a3-5b7d-461c-836c-43824553da11" });
        }
    }
}
