using EFTopics.DAL.Dtos;
using Newtonsoft.Json;

namespace BlazorApp.EF.Services
{
    public class TopicService
    {
        private HttpClient _httpClient;

        public TopicService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TopicDto>> GetTopicsAsync()
        {
            var url = _httpClient.BaseAddress;
            var response = await _httpClient.GetAsync($"{url}/Topic");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<TopicDto>>(json);
        }

        public async Task<TopicDto> GetTopicByIdAsync(int id)
        {
            var url = _httpClient.BaseAddress;
            var response = await _httpClient.GetAsync($"{url}/Topic/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TopicDto>(json);
        }
    }
}
