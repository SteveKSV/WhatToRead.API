using EFTopics.BBL.Entities;
using EFWhatToRead_DAL.Seeding;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTopics.BBL.Seeding
{
    public class PostSeeder : ISeeder<Post>
    {
        private readonly List<Post> posts = new()
        {
            new Post
            {
                PostId = 3,
                Title = "A new book about rockets",
                Slug = "a-new-book-about-rockets",
                Views = 5000,
                Image = "rocketsBook.jpg",
                Body = "Content about how people are making rockets for science",
                Published = false,
                Created_At = new DateTime(2022, 2, 3),
                Updated_At = new DateTime(2022, 2, 3),
            },
             new Post
            {
                PostId = 4,
                Title = "A new book about movies",
                Slug = "a-new-book-about-movies",
                Views = 5000,
                Image = "moviesBook.jpg",
                Body = "Content about how people are making movies",
                Published = true,
                Created_At = new DateTime(2018, 3, 17),
                Updated_At = new DateTime(2022, 3, 2),
            },
              new Post
            {
                PostId = 5,
                Title = "A new movie",
                Slug = "a-new-book-movie",
                Views = 5000,
                Image = "movie.png",
                Body = "A new movie_content",
                Published = false,
                Created_At = new DateTime(2023, 1, 1),
                Updated_At = new DateTime(2023, 3, 2),
            }

        };

        public void Seed(EntityTypeBuilder<Post> builder)
        {
            builder.HasData(posts);
        }
    }
}
