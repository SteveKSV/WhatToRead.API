using AutoMapper;
using BlazorApp.EF.Models;
using EFTopics.BBL.Dtos;
using EFWhatToRead_DAL.Params;
using Newtonsoft.Json;
using System.Text;

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
    }
}
