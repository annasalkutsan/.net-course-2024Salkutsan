using System.Text;
using BankSystem.App.Dto;
using Newtonsoft.Json;

namespace BankSystem.App.Services;

public class CurrencyService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "api_key";

    public CurrencyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> ConvertCurrencyAsync(decimal amount, string fromCurrency, string toCurrency, CancellationToken cancellationToken)
    {
        var requestUriBuilder = new StringBuilder("https://www.amdoren.com/api/currency.php?");
        requestUriBuilder.Append("api_key=").Append(_apiKey)
            .Append("&from=").Append(fromCurrency)
            .Append("&to=").Append(toCurrency)
            .Append("&amount=").Append(amount);

        string requestUri = requestUriBuilder.ToString();

        HttpResponseMessage response = await _httpClient.GetAsync(requestUri, cancellationToken);

        // успешность запроса
        response.EnsureSuccessStatusCode();

        // ответ от сервера
        string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        var currencyResponse = JsonConvert.DeserializeObject<CurrencyApiResponse>(responseBody);

        if (currencyResponse.Error != 0)
        {
            throw new Exception($"Ошибка API: {currencyResponse.Error}");
        }

        return currencyResponse.Amount;
    }
}