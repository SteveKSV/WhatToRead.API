﻿// <auto-generated />
using System;
using EFTopics.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFTopics.DAL.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230308140814_NewSeeders")]
    partial class NewSeeders
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EFTopics.DAL.Entities.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime2");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Published")
                        .HasColumnType("bit");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated_At")
                        .HasColumnType("datetime2");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("PostId");

                    b.ToTable("Posts");

                    b.HasData(
                        new
                        {
                            PostId = 3,
                            Body = "Content about how people are making rockets for science",
                            Created_At = new DateTime(2022, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Image = "rocketsBook.jpg",
                            Published = false,
                            Slug = "a-new-book-about-rockets",
                            Title = "A new book about rockets",
                            Updated_At = new DateTime(2022, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Views = 5000
                        },
                        new
                        {
                            PostId = 4,
                            Body = "Content about how people are making movies",
                            Created_At = new DateTime(2018, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Image = "moviesBook.jpg",
                            Published = true,
                            Slug = "a-new-book-about-movies",
                            Title = "A new book about movies",
                            Updated_At = new DateTime(2022, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Views = 5000
                        },
                        new
                        {
                            PostId = 5,
                            Body = "A new movie_content",
                            Created_At = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Image = "movie.png",
                            Published = false,
                            Slug = "a-new-book-movie",
                            Title = "A new movie",
                            Updated_At = new DateTime(2023, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Views = 5000
                        });
                });

            modelBuilder.Entity("EFTopics.DAL.Entities.PostBlog", b =>
                {
                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.HasKey("PostId", "TopicId");

                    b.HasIndex("TopicId");

                    b.ToTable("PostBlogs", (string)null);

                    b.HasData(
                        new
                        {
                            PostId = 3,
                            TopicId = 1
                        },
                        new
                        {
                            PostId = 4,
                            TopicId = 1
                        },
                        new
                        {
                            PostId = 5,
                            TopicId = 2
                        });
                });

            modelBuilder.Entity("EFTopics.DAL.Entities.Topic", b =>
                {
                    b.Property<int>("TopicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TopicId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("TopicId");

                    b.ToTable("Topics");

                    b.HasData(
                        new
                        {
                            TopicId = 1,
                            Name = "Books"
                        },
                        new
                        {
                            TopicId = 2,
                            Name = "Movie"
                        });
                });

            modelBuilder.Entity("EFTopics.DAL.Entities.PostBlog", b =>
                {
                    b.HasOne("EFTopics.DAL.Entities.Post", "Post")
                        .WithMany("PostBlogs")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFTopics.DAL.Entities.Topic", "Topic")
                        .WithMany("PostBlogs")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("EFTopics.DAL.Entities.Post", b =>
                {
                    b.Navigation("PostBlogs");
                });

            modelBuilder.Entity("EFTopics.DAL.Entities.Topic", b =>
                {
                    b.Navigation("PostBlogs");
                });
#pragma warning restore 612, 618
        }
    }
}
