using EFWhatToRead_BBL.Dtos;
using EFWhatToRead_DAL.Params;
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

        public async Task<List<TopicDto>> GetTopicsAsync(PageModel pagination)
        {
            var url = _httpClient.BaseAddress;
            var response = await _httpClient.GetAsync($"{url}/Topic?pageNumber={pagination.PageNumber}&pageSize={pagination.PageSize}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<TopicDto>>(json);
        }

        public async Task<int> GetTotalItemsAsync()
        {
            var url = _httpClient.BaseAddress;
            var response = await _httpClient.GetAsync($"{url}/Topic/Count");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(json);
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
