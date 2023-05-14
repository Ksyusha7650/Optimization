using System;
using System.Windows;
using OxyPlot;
using OxyPlot.Series;

namespace CourseWorkOptimization;

public partial class ChartsWindow : Window
{
    public PlotModel MyModel { get; set; }

    public ChartsWindow()
    {
       
      //  MyModel
        InitializeComponent();
        /*var model = new PlotModel { Title = "Example 1" };
        model.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));*/
        
        /**/


    }
}