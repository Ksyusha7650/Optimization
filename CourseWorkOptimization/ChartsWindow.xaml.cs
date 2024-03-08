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
        InitializeComponent();
    }
}