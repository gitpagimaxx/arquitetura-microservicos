using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using GeekShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Services;

public class ProductService(HttpClient httpClient) : IProductService
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    public const string BasePath = "api/v1/Product";

    

    public async Task<IEnumerable<ProductViewModel>> FindAll(string token)
    {
        SetBearerToken(token);

        var response = await _httpClient.GetAsync(BasePath);
        var products = await response.ReadContentAs<List<ProductViewModel>>();
        return products ?? [];
    }

    public async Task<ProductViewModel> FindById(long id, string token)
    {
        SetBearerToken(token);

        var response = await _httpClient.GetAsync($"{BasePath}/{id}");
        return await response.ReadContentAs<ProductViewModel>() ?? new ProductViewModel();
    }

    public async Task<ProductViewModel> Create(ProductViewModel model, string token)
    {
        SetBearerToken(token);

        var response = await _httpClient.PostAsJson(BasePath, model);

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<ProductViewModel>() ?? throw new Exception();
        else throw new Exception("Something went wrong when calling API");
    }

    public async Task<ProductViewModel> Update(ProductViewModel model, string token)
    {
        SetBearerToken(token);

        var response = await _httpClient.PutAsJson(BasePath, model);

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<ProductViewModel>() ?? throw new Exception();
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

    private void SetBearerToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
