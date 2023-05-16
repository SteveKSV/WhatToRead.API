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
    public class TopicsRepository : GenericRepository<Topic>, ITopicsRepository
    {
        public TopicsRepository(ApplicationContext _dbcontext) : base(_dbcontext) { }

        public async Task<List<TopicsWithPost>> GetAllTopicsWithPosts()
        {
            var topics = await _dbContext.Topics.ToListAsync();
            var result = new List<TopicsWithPost>();
            foreach (var topic in topics)
            {
                await _dbContext.Entry(topic)
                    .Collection(t => t.PostBlogs)
                    .LoadAsync();

                foreach (var postBlog in topic.PostBlogs)
                {
                    await _dbContext.Entry(postBlog)
                        .Reference(pb => pb.Post)
                        .LoadAsync();
                }

                result.Add(new TopicsWithPost
                {
                    TopicId = topic.TopicId,
                    Name = topic.Name,
                    Posts = topic.PostBlogs.Select(pb => new Post
                    {
                        PostId = pb.Post.PostId,
                        Title = pb.Post.Title,
                        Slug = pb.Post.Slug,
                        Views = pb.Post.Views,
                        Image = pb.Post.Image,
                        Body = pb.Post.Body,
                        Published = pb.Post.Published,
                        Created_At = pb.Post.Created_At,
                        Updated_At = pb.Post.Updated_At
                    })
                });
            }

            return result;
        }

        public async Task<TopicsWithPost?> GetTopicByIdWithPosts(int topicId)
        {
            var topic = await _dbContext.Topics.FindAsync(topicId);
            

            await _dbContext.Entry(topic)
                .Collection(t => t.PostBlogs)
                .LoadAsync();

            foreach (var postBlog in topic.PostBlogs)
            {
                await _dbContext.Entry(postBlog)
                    .Reference(pb => pb.Post)
                    .LoadAsync();
            }

            var result = new TopicsWithPost()
            {
                TopicId = topic.TopicId,
                Name = topic.Name,
                Posts = topic.PostBlogs.Select(pb => new Post
                {
                    PostId = pb.Post.PostId,
                    Title = pb.Post.Title,
                    Slug = pb.Post.Slug,
                    Views = pb.Post.Views,
                    Image = pb.Post.Image,
                    Body = pb.Post.Body,
                    Published = pb.Post.Published,
                    Created_At = pb.Post.Created_At,
                    Updated_At = pb.Post.Updated_At
                })
            };
            return result;
        }

        public async Task<int> GetTotalItems()
        {

            int count = await _dbContext.Topics.CountAsync();
            return count;

        }
    }
}
