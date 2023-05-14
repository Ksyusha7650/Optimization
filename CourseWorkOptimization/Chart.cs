using System;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace CourseWorkOptimization;

public class MyChart
{
    private Algorithm _algorithm;
    public MyChart()
    {
        _algorithm = new Algorithm();
        MyModel = new PlotModel { Title = "ContourSeries" };
        MyModel.Axes.Add(new OxyPlot.Axes.LinearColorAxis
        {
            Position = OxyPlot.Axes.AxisPosition.Right,
            Palette = OxyPalettes.Jet(100),
            HighColor = OxyColors.Gray,
            LowColor = OxyColors.Black
        });
        double x0 = -3;
        double x1 = 3;
        double y0 = -2;
        double y1 = 6;
//generate values
        Func<double, double, double> peaks = (x, y) => _algorithm.GetS(x, y);
        var xx = ArrayBuilder.CreateVector(x0, x1, 10);
        var yy = ArrayBuilder.CreateVector(y0, y1, 10);
        var peaksData = ArrayBuilder.Evaluate(peaks, xx, yy);
        var cs = new ContourSeries
        {
            Color = OxyColors.Orange,
            LabelBackground = OxyColors.White,
            ColumnCoordinates = yy,
            RowCoordinates = xx,
            ContourColors = OxyPalettes.Hot(500).Colors.ToArray(),
            Data = peaksData
        };
        OxyPlot.Series.HeatMapSeries heatmap = new OxyPlot.Series.HeatMapSeries {
            Data = peaksData,
            X0 = x0,
            X1 = x1,
            Y0 = y0,
            Y1 = y1
        };
        MyModel.Series.Add(heatmap);
    }
    public PlotModel MyModel { get; private set; }
}

