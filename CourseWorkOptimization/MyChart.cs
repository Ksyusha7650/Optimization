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

        var xx = ArrayBuilder.CreateVector(x0, x1, 100);
        var yy = ArrayBuilder.CreateVector(y0, y1, 100);
        var peaksData = ArrayBuilder.Evaluate(Peaks, xx, yy);
        var cs1 = new ContourSeries
        {
            Color = OxyColors.Orange,
            LabelBackground = OxyColors.White,
            ColumnCoordinates = yy,
            RowCoordinates = xx,
            ContourColors = OxyPalettes.Hot(500).Colors.ToArray(),
            Data = peaksData
        };

        MyModel.Axes.Add(new LinearColorAxis 
            { Position = AxisPosition.Right,
                Palette = OxyPalettes.Jet(100), 
                HighColor = OxyColors.Gray,
                LowColor = OxyColors.Black });

        var hms = new HeatMapSeries { X0 = y0, X1 = y1, Y0 = x0, Y1 = x1, Data = peaksData };
        MyModel.Series.Add(hms);
      
        var cs = new ContourSeries
        {
            StrokeThickness = 1,
            ContourColors = new[] { OxyColors.Black },
            ContourLevelStep = 0.5,
            FontSize = 0,
            LabelBackground = OxyColors.Undefined,
            ColumnCoordinates = yy,
            RowCoordinates = xx,
            Data = peaksData,
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

