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
    private List<Calculation> calculations, calculations2, calculationsBox, calculationsGenetic;
    public LineSeries LineGradient, LineNesterov, LineBox, LineGenetic;

    public MyChart()
    {
        _algorithm = new Algorithm();
        calculations = new();
        calculations2 = new();
        calculationsBox = new();
        calculationsGenetic = new();
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
            LineGradient = new LineSeries
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
                var value = Peaks(t1, t2);
                if (!double.IsNaN(value))
                {
                    LineGradient.Points.Add(
                        new DataPoint(t1, t2));
                    calculations.Add(
                        new Calculation(t1, t2, Math.Round(value, 2)));
                }
             });
            var res = _algorithm.List.Last();
            SetLabel(LineGradient.Title, res, Math.Round(Peaks(res.FirstElement, res.SecondElement), 2));
            MyModel.Series.Add(LineGradient);
        }
        if (MainWindow.isSecondUsed)
        {
            _algorithm.Nesterov();
            LineNesterov = new LineSeries
            {
                StrokeThickness = 1,
                Color = OxyColors.Gold,
                Title = "Метод Нестерова",
                TextColor = OxyColors.Gold

            };
            _algorithm.ListNesterov.ForEach(x => {
                var t1 = Math.Round(x.FirstElement, 2);
                var t2 = Math.Round(x.SecondElement, 2);
                var value = Peaks(t1, t2);
                if (!double.IsNaN(value))
                {
                    LineNesterov.Points.Add(
                        new DataPoint(t1, t2));
                    calculations2.Add(
                        new Calculation(t1, t2, Math.Round(value, 2)));
                }
            });
            var res = _algorithm.ListNesterov.Last();
            SetLabel(LineNesterov.Title, res, Math.Round(Peaks(res.FirstElement, res.SecondElement), 2));
            MyModel.Series.Add(LineNesterov);
            
        }

        if (MainWindow.isBox)
        {
            double anst1 = 0, anst2 = 0;
            int countIter = 0;
            _algorithm.Box(ref anst1, ref anst2, ref countIter);
            LineBox = new LineSeries
            {
                StrokeThickness = 2,
                Color = OxyColors.Red,
                Title = "Метод Бокса",
                TextColor = OxyColors.Red

            };
            _algorithm.ListBox.ForEach(x => {
                var t1 = Math.Round(x.FirstElement, 2);
                var t2 = Math.Round(x.SecondElement, 2);
                var value = Peaks(t1, t2);
                if (!double.IsNaN(value))
                {
                    LineBox.Points.Add(
                        new DataPoint(t1, t2));
                    calculationsBox.Add(
                        new Calculation(t1, t2, Math.Round(value, 2)));
                }
            });
            if (_algorithm.ListBox.Count > 0)
            {
                var res = _algorithm.ListBox.Last();
                SetLabel(LineBox.Title, res, Math.Round(Peaks(res.FirstElement, res.SecondElement), 2));
                MyModel.Series.Add(LineBox);
            }
        }

        if (MainWindow.isGenetic)
        {
            _algorithm.GeneticAlgorithm();
            LineGenetic = new LineSeries
            {
                StrokeThickness = 1,
                Color = OxyColors.Magenta,
                Title = "Генетический алгоритм",
                TextColor = OxyColors.Magenta

            };
            _algorithm.ListGenetic.ForEach(x => {
                var t1 = Math.Round(x.FirstElement, 2);
                var t2 = Math.Round(x.SecondElement, 2);
                var value = Peaks(t1, t2);
                if (!double.IsNaN(value))
                {
                    LineGenetic.Points.Add(
                        new DataPoint(t1, t2));
                    calculationsGenetic.Add(
                        new Calculation(t1, t2, Math.Round(value, 2)));
                }
            });
            if (_algorithm.ListGenetic.Count > 0)
            {
                var res = _algorithm.ListGenetic.Last();
                SetLabel(LineGenetic.Title, res, Math.Round(Peaks(res.FirstElement, res.SecondElement), 2));
                MyModel.Series.Add(LineGenetic);
            }
        }
        new TableWindow(calculations, calculations2, calculationsBox, calculationsGenetic).Show();

    }
    public PlotModel MyModel { get; private set; }
    public string ResultText { get; private set; }

    private void SetLabel(string title, MyTuple result, double answer)
    {
        var T1 = Math.Round(result.FirstElement, 2);
        var T2 = Math.Round(result.SecondElement, 2);
        double costPerKg = 100.0;
        if (!double.IsNaN(answer))
        {
            double totalCost = Math.Round(answer * costPerKg, 2);
            ResultText += $"{title}: T1 = {T1} °C, T2 = {T2} °C, Суммарная себестоимость = {totalCost} у.е.\n";
        }
        else
        {
            ResultText += $"{title}: T1 = {T1} °C, T2 = {T2} °C, недопустимая точка\n";
        }
    }

}

