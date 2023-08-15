using MudBlazor;

namespace JewelryApp.Client.Pages;

public partial class Index
{
    private ChartOptions options = new();
    public List<ChartSeries> Series = new()
    {
        new ChartSeries { Name = "طلای 18 عیار", Data = new double[] { 90, 79, 72, 69, 62, 62, 55 } },
        new ChartSeries { Name = "طلای 24 عیار", Data = new double[] { 35, 41, 35, 51, 49, 62, 69 } },
    };
    public string[] XAxisLabels = { "شنبه", "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنجشنبه", "جمعه" };

    protected override void OnInitialized()
    {
        options.YAxisFormat = "c2";
        base.OnInitialized();
    }

    void OnClickMenu(InterpolationOption interpolationOption)
    {
        options.InterpolationOption = interpolationOption;
        StateHasChanged();
    }
}

