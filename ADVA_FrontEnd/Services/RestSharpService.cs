using ADVA_FrontEnd.Services.IServices;
using RestSharp;

namespace ADVA_FrontEnd.Services
{
    public class RestSharpService<T> : IRestSharpService<T> where T : class
    {
        private readonly RestClient _restClient;

        public RestSharpService()
        {
            _restClient = new RestClient();
        }


        public async Task<RestResponse> Delete(string url)
        {
            var request = new RestRequest(url, Method.Delete);

            // var response = await _restClient.DeleteAsync(request);
            var response = await _restClient.ExecuteAsync(request); // "ExecuteAsync" handling error default if occured

            if (!response.IsSuccessful)
            {
                Console.WriteLine($"ERROR: {response.ErrorException?.Message}");
            }
            return response;
        }

        public async Task<List<T>> GetAsync(string url)
        {

            var request = new RestRequest(url, Method.Get);

            //// one way 
            var response = await _restClient.ExecuteGetAsync<List<T>>(request);

            if (response.Data == null)
            {
                Console.WriteLine($"ERROR: {response.ErrorException?.Message}");
            }

            return response.Data!;
        }

        public async Task<T> GetByIdAsync(string url)
        {
            var request = new RestRequest(url, Method.Get);


            //// one way 
            var response = await _restClient.ExecuteGetAsync<T>(request);

            if (response.Data == null)
            {
                Console.WriteLine($"ERROR: {response.ErrorException?.Message}");
            }

            return response.Data!;
        }

        public async Task<RestResponse> PostAsync(string url, T data)
        {
            var request = new RestRequest(url, Method.Post);

            request.AddJsonBody(data);

            request.AddHeader("Accept", "application/json");

            return await _restClient.ExecutePostAsync(request);
        }

        public async Task<RestResponse> PostToDeleteCart(string url, int cartDetailsId)
        {
            var request = new RestRequest(url, Method.Post);

            request.AddBody(cartDetailsId);

            request.AddHeader("Accept", "application/json");

            return await _restClient.ExecutePostAsync(request);
        }

        public async Task<T> UpdateAsync(string url, T data)
        {
            var request = new RestRequest(url, Method.Put);

            request.AddJsonBody(data);
            request.AddHeader("Accept", "application/json");

            var response = await _restClient.ExecutePutAsync<T>(request);

            if (!response.IsSuccessful)
            {
                Console.WriteLine($"ERROR: {response.ErrorException?.Message}");
            }


            return response.Data!;
        }
    }
}
