using Microsoft.AspNetCore.Mvc;
using NabihniElectricity.Models;
using System.Text.Json;

namespace NabihniElectricity.Controllers
{
    public class WebApiController : Controller
    {
        private readonly HttpClient _httpClient;

        public WebApiController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> GetProducts()
        {
            var json = await _httpClient.GetStringAsync("https://fakestoreapi.com/products");

            var products = JsonSerializer.Deserialize<List<ApiProduct>>
(
    json,
    new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    }
);
            return Json(products);
        }
    }
}