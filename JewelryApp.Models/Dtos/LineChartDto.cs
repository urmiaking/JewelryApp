namespace JewelryApp.Models.Dtos;

public class LineChartDto
{
    public string[] XAxisValues { get; set; }
    public List<Line> Data { get; set; }
}

public class Line
{
    public string Name { get; set; }
    public double[] Data { get; set; }
}