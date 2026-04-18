using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;
    private const string F1ApiBase = "https://api.jolpi.ca/ergast/f1";

    public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public IActionResult Index()
    {
        Console.WriteLine("HomeController Index action called");
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    

    // // API endpoints
    // public async Task<IActionResult> Races()
    // {
    //     var response = await _httpClient.GetAsync($"{F1ApiBase}/");
    //     var content = await response.Content.ReadAsStringAsync();
    //     var data = JsonSerializer.Deserialize<F1ApiResponse>(content);
    //     return Ok(data?.MRData.RaceTable.Races);
    // }

    // public async Task<IActionResult> RacesByYear(string year)
    // {
    //     var response = await _httpClient.GetAsync($"{F1ApiBase}/{year}/");
    //     var content = await response.Content.ReadAsStringAsync();
    //     var data = JsonSerializer.Deserialize<F1ApiResponse>(content);
    //     return Ok(data?.MRData.RaceTable.Races);
    // }

    // public async Task<IActionResult> CircuitsByYear(string year)
    // {
    //     var response = await _httpClient.GetAsync($"{F1ApiBase}/{year}/circuits/");
    //     var content = await response.Content.ReadAsStringAsync();
    //     var data = JsonSerializer.Deserialize<F1ApiResponse>(content);
    //     return Ok(data?.MRData.RaceTable);
    // }
}
