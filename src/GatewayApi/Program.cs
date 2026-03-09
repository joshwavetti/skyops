var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("WeatherApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["WeatherApi:BaseUrl"] ?? "http://localhost:5292");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5151", "http://134.112.164.114")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

app.MapGet("/health", () => "GatewayApi is running");

app.MapGet("/weather/{city}", async (string city, IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("WeatherApi");
    var response = await client.GetAsync($"/weather/{city}");

    if (!response.IsSuccessStatusCode)
        return Results.NotFound(new { error = $"City '{city}' not found" });

    var data = await response.Content.ReadAsStringAsync();
    return Results.Content(data, "application/json");
});

app.MapGet("/forecast/{city}", async (string city, IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("WeatherApi");
    var response = await client.GetAsync($"/forecast/{city}");

    if (!response.IsSuccessStatusCode)
        return Results.NotFound(new { error = $"City '{city}' not found" });

    var data = await response.Content.ReadAsStringAsync();
    return Results.Content(data, "application/json");
});

app.Run();