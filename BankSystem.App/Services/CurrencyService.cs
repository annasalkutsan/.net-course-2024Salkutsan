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

    public async Task<decimal> ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
    {
        string requestUri = $"https://www.amdoren.com/api/currency.php?api_key={_apiKey}&from={fromCurrency}&to={toCurrency}&amount={amount}";

        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // успешность запроса
        response.EnsureSuccessStatusCode();

        // ответ от сервера
        string responseBody = await response.Content.ReadAsStringAsync();

        var currencyResponse = JsonConvert.DeserializeObject<CurrencyApiResponse>(responseBody);

        if (currencyResponse.Error != 0)
        {
            throw new Exception($"Ошибка API: {currencyResponse.Error}");
        }

        return currencyResponse.Amount;
    }
}