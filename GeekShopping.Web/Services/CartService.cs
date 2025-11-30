using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using GeekShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Services;

public class CartService(HttpClient httpClient) : ICartService
{
    private readonly HttpClient _httpClient = httpClient;
    public const string BasePath = "api/v1/Cart";

    public async Task<CartViewModel> FindCartByUserId(string userId, string token)
    {
        SetBearerToken(token);

        var response = await _httpClient.GetAsync($"{BasePath}/find-cart/{userId}");
        CartViewModel? cart = await response.ReadContentAs<CartViewModel>();
        return cart ?? new CartViewModel();
    }

    public async Task<CartViewModel> AddItemToCart(CartViewModel cart, string token)
    {
        SetBearerToken(token);

        var response = await _httpClient.PostAsJson($"{BasePath}/add-cart", cart);

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<CartViewModel>() ?? throw new Exception();
        else 
            throw new Exception("Something went wrong when calling API");
    }

    public async Task<CartViewModel> UpdateCart(CartViewModel cart, string token)
    {
        SetBearerToken(token);

        var response = await _httpClient.PutAsJson($"{BasePath}/update-cart", cart);

        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<CartViewModel>() ?? throw new Exception();
        else 
            throw new Exception("Something went wrong when calling API");
    }

    public async Task<bool> RemoveFromCart(long cartId, string token)
    {
        SetBearerToken(token);

        var response = await _httpClient.DeleteAsync($"{BasePath}/remove-cart/{cartId}");
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>();
        else throw new Exception("Something went wrong when calling API");
    }

    public async Task<bool> ApplyCoupon(CartViewModel cart, string coupon, string token)
    {
        SetBearerToken(token);

        throw new NotImplementedException();
    }

    public async Task<bool> RemoveCoupon(string userId, string token)
    {
        SetBearerToken(token);

        throw new NotImplementedException();
    }

    public async Task<bool> ClearCoupon(string userId, string token)
    {
        SetBearerToken(token);

        throw new NotImplementedException();
    }

    public async Task<CartViewModel> Checkout(CartHeaderViewModel cartHeader, string token)
    {
        SetBearerToken(token);

        throw new NotImplementedException();
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
