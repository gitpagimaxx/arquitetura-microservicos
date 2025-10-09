using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using GeekShopping.Web.Utils;

namespace GeekShopping.Web.Services;

public class ProductService(HttpClient httpClient) : IProductServices
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    public const string BasePath = "api/v1/Product";

    public async Task<IEnumerable<ProductModel>> FindAll()
    {
        var response = await _httpClient.GetAsync(BasePath);
        var products = await response.ReadContentAs<List<ProductModel>>();
        return products ?? [];
    }

    public async Task<ProductModel> FindById(long id)
    {
        var response = await _httpClient.GetAsync($"{BasePath}/{id}");
        return await response.ReadContentAs<ProductModel>() ?? new ProductModel();
    }

    public async Task<ProductModel> Create(ProductModel model)
    {
        var response = await _httpClient.PostAsJson(BasePath, model);

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<ProductModel>() ?? throw new Exception();
        else throw new Exception("Something went wrong when calling API");
    }

    public async Task<ProductModel> Update(ProductModel model)
    {
        var response = await _httpClient.PutAsJson(BasePath, model);

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<ProductModel>() ?? throw new Exception();
        else throw new Exception("Something went wrong when calling API");
    }


    public async Task<bool> Delete(long id)
    {
        var response = await _httpClient.DeleteAsync($"{BasePath}/{id}");
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>();
        else throw new Exception("Something went wrong when calling API");
    }
}
