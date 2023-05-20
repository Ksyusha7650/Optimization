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
        MyModel = new PlotModel { Title = "2D график" };
       MyModel.Axes.Add(new LinearColorAxis
       {
           Position = AxisPosition.Right,
           Palette = OxyPalettes.Jet(100),
           HighColor = OxyColors.Gray,
           LowColor = OxyColors.Black
       });
        double x0 = -3;
        double x1 = 3;
        double y0 = -2;
        double y1 = 6;
        double Peaks(double x, double y) => _algorithm.GetS(x, y);

        var xx = ArrayBuilder.CreateVector(x0, x1, 0.1);
        var yy = ArrayBuilder.CreateVector(y0, y1, 0.1);
        var peaksData = ArrayBuilder.Evaluate(Peaks, xx, yy);
        var hms = new HeatMapSeries { X0 = x0, X1 = x1, Y0 = y0, Y1 = y1, Data = peaksData };
        MyModel.Series.Add(hms);
        var cs = new ContourSeries
        {
            StrokeThickness = 1,
            ContourColors = new[] { OxyColors.Black },
            FontSize = 0,
            LabelBackground = OxyColors.Undefined,
            ColumnCoordinates = xx,
            RowCoordinates = yy,
            Data = peaksData,
        };
        MyModel.Series.Add(cs);
        _algorithm.Calculate();
        _algorithm.Nesterov();
        var line = new LineSeries
        {
            StrokeThickness = 1,
            Color = OxyColors.Cyan,
        };
        _algorithm.List.ForEach(x=>
        line.Points.Add(
            new DataPoint(x.FirstElement, x.SecondElement)));
        MyModel.Series.Add(line);
        var line2 = new LineSeries
        {
            StrokeThickness = 1,
            Color = OxyColors.Gold,
            
        };
        _algorithm.ListNesterov.ForEach(x =>
            line2.Points.Add(
                new DataPoint(x.FirstElement, x.SecondElement)));
        MyModel.Series.Add(line2);
        
    }
    public PlotModel MyModel { get; private set; }
}

