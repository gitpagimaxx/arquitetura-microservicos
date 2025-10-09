using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GeekShopping.Web.Utils;

public static class HttpClientExtensions
{
    private static readonly MediaTypeHeaderValue _contentType = new("application/json");
    private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
    private readonly static Encoding _encoding = Encoding.UTF8;

    public static async Task<T?> ReadContentAs<T>(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

        var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return JsonSerializer.Deserialize<T>(dataAsString, _options);
    }

    public static Task<HttpResponseMessage> PostAsJson<T>(this HttpClient httpClient, string url, T item)
        => httpClient.PostAsync(url, new StringContent(JsonSerializer.Serialize(item), _encoding, _contentType));

    public static Task<HttpResponseMessage> PutAsJson<T>(this HttpClient httpClient, string url, T item)
        => httpClient.PutAsync(url, new StringContent(JsonSerializer.Serialize(item), _encoding, _contentType));

    public static Task<HttpResponseMessage> DeleteAsJson<T>(this HttpClient httpClient, string url, T item)
    {
        var json = JsonSerializer.Serialize(item);
        var content = new StringContent(json, _encoding, _contentType);

        var request = new HttpRequestMessage(HttpMethod.Delete, url)
        {
            Content = content
        };

        return httpClient.SendAsync(request);
    }
}
