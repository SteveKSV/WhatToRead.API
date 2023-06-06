using AutoMapper;
using BlazorApp.EF.Models;
using EFTopics.BBL.Dtos;
using EFWhatToRead_DAL.Params;
using Newtonsoft.Json;
using System.Text;
using WhatToRead.API.AdoNet.BBL.Dtos;

namespace BlazorApp.EF.Services
{
    public class PostService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        public PostService(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<List<PostDto>> GetPostsAsync(PageModel pagination)
        {
            var url = _httpClient.BaseAddress;
            var response = await _httpClient.GetAsync($"{url}/Post?pageNumber={pagination.PageNumber}&pageSize={pagination.PageSize}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PostDto>>(json);
        }
        public async Task CreatePostAsync(Post post)
        {
            var postToSerialise = _mapper.Map<Post, PostDto>(post);
            var json = JsonConvert.SerializeObject(postToSerialise);
            var postToSend = new StringContent(json, Encoding.UTF8, "application/json");
            var url = _httpClient.BaseAddress;
            await _httpClient.PostAsync($"{url}/Post", postToSend);
        }

        public async Task<int> GetTotalPostCountAsync()
        {
            var url = _httpClient.BaseAddress;
            var response = await _httpClient.GetAsync($"{url}/Post/GetTotalPostCount");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Int32>(json);
        }

        public async Task UpdatePostAsync(int postId, Post post)
        {
            var url = _httpClient.BaseAddress;
            var postToSend = _mapper.Map<Post, PostDto>(post);
            var response = await _httpClient.PutAsJsonAsync($"{url}/Post/{postId}", postToSend);
            response.EnsureSuccessStatusCode();
        }
        public async Task<Post> GetPostByIdAsync(int postId)
        {
            var url = _httpClient.BaseAddress;
            var response = await _httpClient.GetAsync($"{url}/Post/{postId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return _mapper.Map<PostDto, Post>(JsonConvert.DeserializeObject<PostDto>(json));
        }

        public async Task DeletePostAsync(int postId)
        {
            var url = _httpClient.BaseAddress;
            var response = await _httpClient.DeleteAsync($"{url}/Post/{postId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
