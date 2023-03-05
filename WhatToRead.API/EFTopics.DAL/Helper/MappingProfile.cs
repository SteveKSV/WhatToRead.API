using AutoMapper;
using EFTopics.DAL.Dtos;
using EFTopics.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTopics.DAL.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, PostDto>();
            CreateMap<Topic, TopicDto>();
            CreateMap<PostDto, Post>();
            CreateMap<TopicDto, Topic>();
        }
    }
}
