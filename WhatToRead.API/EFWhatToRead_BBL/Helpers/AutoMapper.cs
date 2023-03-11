using AutoMapper;
using EFTopics.DAL.Dtos;
using EFTopics.DAL.Entities;
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
        }
    }
}
