using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EFTopics.BBL.Migrations
{
    /// <inheritdoc />
    public partial class NewSeeders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "TopicId", "Name" },
                values: new object[] { 2, "Movie" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "PostId", "Body", "Created_At", "Image", "Published", "Slug", "Title", "Updated_At", "Views" },
                values: new object[,]
                {
                    { 3, "Content about how people are making rockets for science", new DateTime(2022, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "rocketsBook.jpg", false, "a-new-book-about-rockets", "A new book about rockets", new DateTime(2022, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 5000 },
                    { 4, "Content about how people are making movies", new DateTime(2018, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "moviesBook.jpg", true, "a-new-book-about-movies", "A new book about movies", new DateTime(2022, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 5000 },
                    { 5, "A new movie_content", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "movie.png", false, "a-new-book-movie", "A new movie", new DateTime(2023, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 5000 }
                });

            migrationBuilder.InsertData(
                table: "PostBlogs",
                columns: new[] { "PostId", "TopicId" },
                values: new object[,]
                {
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PostBlogs",
                keyColumns: new[] { "PostId", "TopicId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "PostBlogs",
                keyColumns: new[] { "PostId", "TopicId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "PostBlogs",
                keyColumns: new[] { "PostId", "TopicId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 5);

        }
    }
}
