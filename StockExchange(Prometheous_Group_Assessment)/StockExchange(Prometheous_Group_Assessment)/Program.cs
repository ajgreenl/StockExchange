using System.Text.Json;
using System.Text.Json.Nodes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient("YahooFinance", client =>
{
    client.BaseAddress = new Uri("https:/finance.yahoo.com");
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/stocks/{symbols}", async (string symbol, IHttpClientFactory httpClientFactory) =>
{
    var stockClient =  httpClientFactory.CreateClient("Yahoo Finance");
try{
    var response = await stockClient.GetAsync($"/v8/finance/chart/{symbol}1mo&interval=15m");

        if (!response.IsSuccessStatusCode)
        {
            return Results.Problem($"Yahoo Finance returned probelm {response.StatusCode}, statuscode: (int)response.StatusCode");
        }

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonNode.Parse(content);
        return Results.Ok(json);
    }
    catch(HttpRequestException ex)
    {
        return Results.Problem(ex.Message, statusCode: 500);
    }
});

app.Run();

