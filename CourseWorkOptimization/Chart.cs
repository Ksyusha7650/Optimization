using System;
using System.Linq;
using ChartDirector;
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
      // MyModel.Axes.Add(new OxyPlot.Axes.LinearColorAxis
      // {
      //     Position = OxyPlot.Axes.AxisPosition.Right,
      //     Palette = OxyPalettes.Jet(100),
      //     HighColor = OxyColors.Gray,
      //     LowColor = OxyColors.Black
      // });
        double x0 = -3;
        double x1 = 3;
        double y0 = -2;
        double y1 = 6;
//generate values
double Peaks(double x, double y) => _algorithm.GetS(x, y);
Func<double, double, double> peaks = (x, y) => 3 * (1 - x) * (1 - x) *
    Math.Exp(-(x * x) - (y + 1) * (y + 1)) - 10 * (x / 5 - x * x * x - y * y * y * y * y) 
                                                * Math.Exp(-x * x - y * y) - 1.0 / 3 * Math.Exp(-(x + 1) * (x + 1) - y * y);

var xx = ArrayBuilder.CreateVector(x0, x1, 10);
        var yy = ArrayBuilder.CreateVector(y0, y1, 10);
        var peaksData = ArrayBuilder.Evaluate(Peaks, xx, yy);
        var peaker = ArrayBuilder.Evaluate(peaks, xx, yy);
        var cs1 = new ContourSeries
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
        MyModel.Axes.Add(new LinearColorAxis 
            { Position = AxisPosition.Right,
                Palette = OxyPalettes.Jet(500), 
                HighColor = OxyColors.Gray,
                LowColor = OxyColors.Black });

        /*var hms = new HeatMapSeries { X0 = x0, X1 = x1, Y0 = y0, Y1 = y1, Data = peaksData };
        MyModel.Series.Add(hms);*/
      
        var cs = new ContourSeries
        {
            StrokeThickness = 4,
            Color = OxyColors.Black,
            ContourColors = new[] { OxyColors.SeaGreen, OxyColors.RoyalBlue, OxyColors.IndianRed },
            FontSize = 0,
            ContourLevelStep = 1.0,
            LabelBackground = OxyColors.Undefined,
            ColumnCoordinates = yy,
            RowCoordinates = xx,
            Data = peaksData,
            RenderInLegend = false
        };
        MyModel.Series.Add(cs);
        var solutions = new ScatterSeries
        {
            MarkerType = MarkerType.Circle,
            MarkerFill = OxyColor.FromRgb(0xFF, 0, 0)
        };

        MyModel.Series.Add(solutions);

        MyModel.Series.Add(new ScatterSeries());
    }
    public PlotModel MyModel { get; private set; }
}

