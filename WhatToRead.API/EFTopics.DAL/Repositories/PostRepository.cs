using EFTopics.BBL.Data;
using EFTopics.BBL.Entities;
using EFWhatToRead_DAL.Entities;
using EFWhatToRead_DAL.Repositories.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWhatToRead_DAL.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(ApplicationContext _dbcontext) : base(_dbcontext)
        {
        }

        public async Task<List<PostWithTopics>> GetAllPostsWithTopics()
        {
            var result = _dbContext.Posts
                .Include(p => p.PostBlogs)
                .ThenInclude(pb => pb.Topic)
                .Select(p => new PostWithTopics
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Slug = p.Slug,
                    Views = p.Views,
                    Image = p.Image,
                    Body = p.Body,
                    Published = p.Published,
                    Created_At = p.Created_At.GetValueOrDefault(),
                    Updated_At = p.Updated_At.GetValueOrDefault(),
                    Topics = p.PostBlogs.Select(pb => new Topic
                    {
                        TopicId = pb.Topic.TopicId,
                        Name = pb.Topic.Name
                    }).ToList()
                });

            return await result.ToListAsync();
        }

        public async Task<PostWithTopics?> GetPostByIdWithTopics(int postId)
        {
            var post = await _dbContext.Posts
                .Include(p => p.PostBlogs)
                .ThenInclude(pb => pb.Topic)
                .FirstOrDefaultAsync(p => p.PostId == postId);

            if (post == null) return null;

            var result = new PostWithTopics
            {
                PostId = post.PostId,
                Title = post.Title,
                Slug = post.Slug,
                Views = post.Views,
                Image = post.Image,
                Body = post.Body,
                Published = post.Published,
                Created_At = post.Created_At ?? default,
                Updated_At = post.Updated_At ?? default,
                Topics = post.PostBlogs.Select(pb => new Topic
                {
                    TopicId = pb.Topic.TopicId,
                    Name = pb.Topic.Name
                }).ToList()
            };

            return result;
        }
    }
}
