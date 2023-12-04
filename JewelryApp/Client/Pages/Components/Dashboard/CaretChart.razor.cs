using JewelryApp.Common.Enums;
using JewelryApp.Models.Dtos.Common;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.Dashboard;

public partial class CaretChart
{
    [Parameter]
    public CaretChartType DefaultMode { get; set; }

    private ChartOptions options = new();
    public List<ChartSeries> Series = new();
    //{
    //    new ChartSeries { FullName = "طلای 18 عیار", Data = new double[] { 90, 79, 72, 69, 62, 62, 55 } },
    //    new ChartSeries { FullName = "طلای 24 عیار", Data = new double[] { 35, 41, 35, 51, 49, 62, 69 } },
    //};
    public string[] XAxisLabels = { "شنبه", "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنجشنبه", "جمعه" };

    protected override void OnInitialized()
    {
        options.YAxisFormat = "c2";
        base.OnInitialized();
    }

    private async Task LoadChart(CaretChartType caretChartType)
    {
        var chartDto = await GetAsync<LineChartDto>($"/api/Price/GetCaretData?caretChartType={caretChartType}");

        if (chartDto is not null)
        {
            XAxisLabels = chartDto.XAxisValues;

            Series.Clear();
            foreach (var item in chartDto.Data)
            {
                Series.Add(new ChartSeries { Data = item.Data, Name = item.Name });
            }

            StateHasChanged();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadChart(DefaultMode);
        await base.OnInitializedAsync();
    }

    private async Task OnClickMenu(CaretChartType caretChartType)
    {
        await LoadChart(caretChartType);
        StateHasChanged();
    }
}
