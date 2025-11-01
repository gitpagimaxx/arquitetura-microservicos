using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using GeekShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Services;

public class ProductService(HttpClient httpClient) : IProductServices
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    public const string BasePath = "api/v1/Product";

    private void SetBearerToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<IEnumerable<ProductModel>> FindAll(string token)
    {
        SetBearerToken(token);

        var response = await _httpClient.GetAsync(BasePath);
        var products = await response.ReadContentAs<List<ProductModel>>();
        return products ?? [];
    }

    public async Task<ProductModel> FindById(long id, string token)
    {
        SetBearerToken(token);

        var response = await _httpClient.GetAsync($"{BasePath}/{id}");
        return await response.ReadContentAs<ProductModel>() ?? new ProductModel();
    }

    public async Task<ProductModel> Create(ProductModel model, string token)
    {
        SetBearerToken(token);

        var response = await _httpClient.PostAsJson(BasePath, model);

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<ProductModel>() ?? throw new Exception();
        else throw new Exception("Something went wrong when calling API");
    }

    public async Task<ProductModel> Update(ProductModel model, string token)
    {
        SetBearerToken(token);

        var response = await _httpClient.PutAsJson(BasePath, model);

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<ProductModel>() ?? throw new Exception();
        else throw new Exception("Something went wrong when calling API");
    }

    public async Task<bool> Delete(long id, string token)
    {
        SetBearerToken(token);

        var response = await _httpClient.DeleteAsync($"{BasePath}/{id}");
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>();
        else throw new Exception("Something went wrong when calling API");
    }
}
