using System.Text.Json;

namespace StockExchange.Tests;

public class UnitTest1
{
    [Fact]
    public void CalculateDailyData_CalculatesAverageAndVolume()
    {
        var timestamps = JsonDocument.Parse(
            "[1756209600, 1756213200, 1756216800]"
        ).RootElement;

        var lows = JsonDocument.Parse(
            "[100.14567, 110.14567, 120.14567]"
        ).RootElement;

        var highs = JsonDocument.Parse(
            "[110.14567, 120.14567, 130.14567]"
        ).RootElement;

        var volumes = JsonDocument.Parse(
            "[1000, 2000, 3000]"
        ).RootElement;

        var result = Program.CalculateDailyData(
            timestamps,
            lows,
            highs,
            volumes);

        Assert.Single(result);

        Assert.Equal(110.1457, result[0].LowAverage);
        Assert.Equal(120.1457, result[0].HighAverage);
        Assert.Equal(6000, result[0].Volume);
    }
}