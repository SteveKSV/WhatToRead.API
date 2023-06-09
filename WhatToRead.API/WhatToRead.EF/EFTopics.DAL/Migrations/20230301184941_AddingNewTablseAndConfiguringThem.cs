using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFTopics.BBL.Migrations
{
    /// <inheritdoc />
    public partial class AddingNewTablseAndConfiguringThem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Topics",
                newName: "TopicId");

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                });

            migrationBuilder.CreateTable(
                name: "PostBlogs",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostBlogs", x => new { x.PostId, x.TopicId });
                    table.ForeignKey(
                        name: "FK_PostBlogs_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostBlogs_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "TopicId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostBlogs_TopicId",
                table: "PostBlogs",
                column: "TopicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostBlogs");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "Topics",
                newName: "Id");
        }
    }
}
