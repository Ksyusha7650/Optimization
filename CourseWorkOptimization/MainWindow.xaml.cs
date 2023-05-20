using System.Windows;
using System.Windows.Controls;

namespace CourseWorkOptimization;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void CreateChart(object sender, RoutedEventArgs e)
    {
        var is2DChart = (sender as Button)?.Name == "Create2DChartButton";
        if (is2DChart)
        {
            new ChartsWindow().Show();
        }
    }

    private void Exit(object sender, RoutedEventArgs e)
    {
        Hide();
        new EnterWindow().Show();
        Close();
    }
}