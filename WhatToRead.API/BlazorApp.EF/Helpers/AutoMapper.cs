using AutoMapper;
using BlazorApp.EF.Models;
using EFTopics.BBL.Dtos;
using WhatToRead.API.AdoNet.BBL.Dtos;

namespace BlazorApp.EF.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<BookByAuthorDTO, BookByAuthor>().ReverseMap();
            CreateMap<BookDTO, Book>().ReverseMap();
            CreateMap<PostDto, Post>().ReverseMap();
        }
    }
}
