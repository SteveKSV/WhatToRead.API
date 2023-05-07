using AutoMapper;
using EFTopics.DAL.Dtos;
using EFTopics.DAL.Entities;
using EFWhatToRead_BBL.Dtos;
using EFWhatToRead_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
