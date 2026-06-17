using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using KooliProjekt.WindowsForms;

namespace KooliProjekt.WindowsForms.Api
{
    public class ApiClient : IApiClient
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public ApiClient()
        {
            _baseUrl = "http://localhost:5086/api/Employees/";
            _client = new HttpClient();
        }

        public async Task<OperationResult<PagedResult<Employee>>> List(int page, int pageSize)
        {
            var url = _baseUrl + "List?page=" + page + "&pageSize=" + pageSize;

            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<OperationResult<PagedResult<Employee>>>(content, options);
        }

        public async Task<OperationResult> Save(Employee list)
        {
            var url = _baseUrl + "Save";

            var response = await _client.PostAsJsonAsync(url, list);
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<OperationResult>(content, options);
        }

        public async Task<OperationResult> Delete(int id)
        {
            var url = _baseUrl + "Delete";

            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = id })
            };

            using var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<OperationResult>(content, options);
        }
    }
}
