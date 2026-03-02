var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var app = builder.Build();

var apiKey = builder.Configuration["OpenWeather:ApiKey"];
var baseUrl = "https://api.openweathermap.org/data/2.5";

app.MapGet("/health", () => "WeatherApi is running");

app.MapGet("/weather/{city}", async (string city, IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient();
    var url = $"{baseUrl}/weather?q={city}&appid={apiKey}&units=metric&lang=en";

    var response = await client.GetAsync(url);

    if (!response.IsSuccessStatusCode)
        return Results.NotFound(new { error = $"City '{city}' not found" });

    var data = await response.Content.ReadAsStringAsync();
    return Results.Content(data, "application/json");
});

app.MapGet("/forecast/{city}", async (string city, IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient();
    var url = $"{baseUrl}/forecast?q={city}&appid={apiKey}&units=metric&lang=en";

    var response = await client.GetAsync(url);

    if (!response.IsSuccessStatusCode)
        return Results.NotFound(new { error = $"City '{city}' not found" });

    var data = await response.Content.ReadAsStringAsync();
    return Results.Content(data, "application/json");
});

app.Run();