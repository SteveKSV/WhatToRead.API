using AutoMapper;
using EFTopics.BBL.Dtos;
using EFTopics.BBL.Entities;
using EFWhatToRead_BBL.Dtos;
using EFWhatToRead_DAL.Entities;

namespace EFWhatToRead_BBL.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<TopicDto, Topic>().ReverseMap();
            CreateMap<PostDto, Post>().ReverseMap();
            CreateMap<RefreshTokenDto, RefreshToken>().ReverseMap();
            CreateMap<TopicsWithPostsDto, TopicsWithPost>().ReverseMap();
            CreateMap<PostWithTopicsDto, PostWithTopics>().ReverseMap();
        }
    }
}
