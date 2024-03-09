using System;
using System.Windows;
using ChartDirector;
using OxyPlot;
using OxyPlot.Series;

namespace CourseWorkOptimization;

public partial class ChartsWindow3D : Window
{
    public ChartsWindow3D()
    {
        InitializeComponent();
        var charts = new Charts(Chart3D);
        charts.createChart(Chart3D);
    }
}