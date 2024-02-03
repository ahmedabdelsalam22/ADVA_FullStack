using RestSharp;

namespace ADVA_FrontEnd.Services.IServices
{
    public interface IRestSharpService<T> where T : class
    {
        Task<T> UpdateAsync(string url, T data);
        Task<RestResponse> PostAsync(string url, T data);
        Task<List<T>> GetAsync(string url);
        Task<T> GetByIdAsync(string url);
        Task<RestResponse> Delete(string url);
    }
}
