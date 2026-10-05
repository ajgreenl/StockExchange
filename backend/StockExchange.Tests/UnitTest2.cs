using System.Text.Json;

namespace StockExchange.Tests;

public class UnitTest2
{
    [Fact]
    public void CalculateDailyData_EmptyIntervals()
    {
        var timestamps = JsonDocument.Parse(
            "[1756209600, 1756213200, 1756216800, 1756223200, 1756223700]"
        ).RootElement;

        var lows = JsonDocument.Parse(
            "[100.0, null, 110.0, 120.0, 130.0]"
        ).RootElement;

        var highs = JsonDocument.Parse(
            "[110.0, null, 120.0, 130.0, 140.0]"
        ).RootElement;

        var volumes = JsonDocument.Parse(
            "[1000, null, 2000, 3000, 4000]"
        ).RootElement;

        var result = Program.CalculateDailyData(
            timestamps,
            lows,
            highs,
            volumes);

        Assert.Single(result);

        Assert.Equal(115.0, result[0].LowAverage);
        Assert.Equal(125.0, result[0].HighAverage);
        Assert.Equal(10000, result[0].Volume);
    }
}