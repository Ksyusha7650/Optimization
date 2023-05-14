using System.Windows;
using System.Windows.Controls;

namespace CourseWorkOptimization;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Charts _charts;
    public MainWindow()
    {
        InitializeComponent();
        _charts = new Charts(Chart3D);
        _charts.CreateChart(Chart);
        _charts.createChart(Chart3D);
    }

    private void T1TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
    }
}