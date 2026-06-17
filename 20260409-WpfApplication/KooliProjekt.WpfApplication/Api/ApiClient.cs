using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KooliProjekt.WpfApplication
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
            try
            {
                var url = _baseUrl + "List?page=" + page + "&pageSize=" + pageSize;
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                using var response = await _client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<OperationResult<PagedResult<Employee>>>(body);
            }
            catch (Exception ex)
            {
                var result = new OperationResult<PagedResult<Employee>>();
                result.AddError(ex.Message);
                return result;
            }
        }

        public async Task<OperationResult> Save(Employee list)
        {
            try
            {
                var url = _baseUrl + "Save";
                using var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = JsonContent.Create(list)
                };            
                using var response = await _client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<OperationResult>(body);
            }
            catch (Exception ex)
            {
                var result = new OperationResult();
                result.AddError(ex.Message);
                return result;
            }
        }

        public async Task<OperationResult> Delete(int id)
        {
            try
            {
                var url = _baseUrl + "Delete";
                using var request = new HttpRequestMessage(HttpMethod.Delete, url)
                {
                    Content = JsonContent.Create(new { id = id })
                };
                using var response = await _client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<OperationResult>(body);
            }
            catch (Exception ex)
            {
                var result = new OperationResult();
                result.AddError(ex.Message);
                return result;
            }
        }
    }
}
