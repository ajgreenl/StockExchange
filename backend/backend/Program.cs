using System.Text.Json;

 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient("YahooFinance", client =>
{
    client.BaseAddress = new Uri("https://query1.finance.yahoo.com");
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVite", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowVite");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/api/stocks/{symbol}", async (string symbol, IHttpClientFactory httpClientFactory) => 
{
    var stockClient =  httpClientFactory.CreateClient("YahooFinance");
try{
    var response = await stockClient.GetAsync($"/v8/finance/chart/{symbol}?range=1mo&interval=15m");

        if (!response.IsSuccessStatusCode)
        {
            return Results.Problem($"Yahoo Finance returned probelm {response.StatusCode}, statuscode: (int)response.StatusCode");
        }

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var json = await JsonDocument.ParseAsync(stream);

        var root = json.RootElement;

        // You will build StockDay objects here.
        var stocks = new List<StockDay>();

        var result = root.GetProperty("chart").GetProperty("result")[0];

        var quote = result.GetProperty("indicators").GetProperty("quote")[0];
        var lows = quote.GetProperty("low");
        var highs = quote.GetProperty("high");
        var volumes = quote.GetProperty("volume");

        if (!result.TryGetProperty("timestamp", out var timestampsElement))
        {
            return Results.NotFound(new { message = $"No timestamp data found for symbol '{symbol}'." });
        }

       

        var dailyData = new Dictionary<DateOnly, (List<double> lows,
                                          List<double> highs,
                                          long volume)>();

        var i = 0;
        foreach ( var tsElement in timestampsElement.EnumerateArray())
        {
            // Skip missing Yahoo Finance values
            if (lows[i].ValueKind == JsonValueKind.Null || highs[i].ValueKind == JsonValueKind.Null ||
                volumes[i].ValueKind == JsonValueKind.Null) {
            continue;
        }

        long unixTime = tsElement.GetInt64();

        var date = DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
        
        var day = DateOnly.FromDateTime(date);

        double low = lows[i].GetDouble();
        double high = highs[i].GetDouble();
        long volume = volumes[i].GetInt64();

        if (!dailyData.ContainsKey(day)){
            dailyData[day] = (
                new List<double>(),
                new List<double>(),
                0
            );
        }

        dailyData[day].lows.Add(low);
        dailyData[day].highs.Add(high);
        
        dailyData[day] = (
        dailyData[day].lows,
        dailyData[day].highs,
        dailyData[day].volume + volume
        );
        i++;
        }
        
        
        foreach (var day in dailyData) {
            double lowAverage = day.Value.lows.Average();
            double highAverage = day.Value.highs.Average();

            stocks.Add(
            new StockDay(
                day.Key.ToString("yyyy-MM-dd"),
                lowAverage,
                highAverage,
                day.Value.volume
            ));
        }
        return Results.Ok(stocks);
        }
    
    catch(HttpRequestException ex)
    {
        return Results.Problem(ex.Message, statusCode: 500);
    }
});


app.Run();

record StockDay(
    string Day,
    double LowAverage,
    double HighAverage,
    long Volume
);