using System;
using System.Collections.Generic;
using System.Linq;
using ChartDirector;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;

namespace CourseWorkOptimization;

public class MyChart
{
    private Algorithm _algorithm;
    private List<Calculation> calculations, calculations2;

    public MyChart()
    {
        _algorithm = new Algorithm();
        calculations = new();
        calculations2 = new();
        MyModel = new PlotModel
        {
            Title = "2D график",
            IsLegendVisible = true
        };
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
        ResultText = "Результат:\n";
        if (MainWindow.isFirstUsed)
        {
            _algorithm.Calculate();
            var line = new LineSeries
            {
                StrokeThickness = 1,
                Color = OxyColors.Cyan,
                Title = "Метод градиентного спуска",
                TextColor = OxyColors.Cyan,
                FontSize = 15
            };
            _algorithm.List.ForEach(x => {
                var t1 = Math.Round(x.FirstElement, 2);
                var t2 = Math.Round(x.SecondElement, 2);
                line.Points.Add(
                    new DataPoint(t1, t2));
                calculations.Add(
                    new Calculation(t1, t2, Math.Round(Peaks(t1, t2), 2)));
             });
            var res = _algorithm.List.Last();
            SetLabel(line.Title, res, Math.Round(Peaks(res.FirstElement, res.SecondElement), 2));
            MyModel.Series.Add(line);
        }
        if (MainWindow.isSecondUsed)
        {
            _algorithm.Nesterov();
            var line2 = new LineSeries
            {
                StrokeThickness = 1,
                Color = OxyColors.Gold,
                Title = "Метод Нестерова",
                TextColor = OxyColors.Gold

            };
            _algorithm.ListNesterov.ForEach(x => {
                var t1 = Math.Round(x.FirstElement, 2);
                var t2 = Math.Round(x.SecondElement, 2);
                line2.Points.Add(
                    new DataPoint(t1, t2));
                calculations2.Add(
                    new Calculation(t1, t2, Math.Round(Peaks(t1, t2), 2)));
            });
            var res = _algorithm.ListNesterov.Last();
            SetLabel(line2.Title, res, Math.Round(Peaks(res.FirstElement, res.SecondElement), 2));
            MyModel.Series.Add(line2);
            
        }
        new TableWindow(calculations, calculations2).Show();

    }
    public PlotModel MyModel { get; private set; }
    public string ResultText { get; private set; }

    private void SetLabel(string title, MyTuple result, double answer)
    {
        var T1 = Math.Round(result.FirstElement, 2);
        var T2 = Math.Round(result.SecondElement, 2);
        ResultText += $"{title}: T1: {T1}, " +
            $"T2: {T2}, S: {answer * 100} у.е.\n";
    }
}

