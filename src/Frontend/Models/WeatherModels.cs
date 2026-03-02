namespace Frontend.Models;

public class WeatherResponse
{
    public string Name { get; set; } = "";
    public MainData Main { get; set; } = new();
    public List<WeatherDescription> Weather { get; set; } = new();
    public WindData Wind { get; set; } = new();
    public SysData Sys { get; set; } = new();
}

public class ForecastResponse
{
    public List<ForecastItem> List { get; set; } = new();
}

public class ForecastItem
{
    public MainData Main { get; set; } = new();
    public List<WeatherDescription> Weather { get; set; } = new();
    public string Dt_txt { get; set; } = "";
}

public class MainData
{
    public double Temp { get; set; }
    public double Feels_like { get; set; }
    public int Humidity { get; set; }
}

public class WeatherDescription
{
    public string Main { get; set; } = "";
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "";
}

public class WindData
{
    public double Speed { get; set; }
}

public class SysData
{
    public string Country { get; set; } = "";
}