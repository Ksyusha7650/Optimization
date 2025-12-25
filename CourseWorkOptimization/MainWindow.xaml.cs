using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace CourseWorkOptimization;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public static bool isFirstUsed, isSecondUsed, isBox, isGenetic;
    public static double alpha;
    public MainWindow()
    {
        InitializeComponent();
        var customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        Thread.CurrentThread.CurrentCulture = customCulture;
        var admin = new AdminWindow();
        admin.ReadFromFile();
        MainWindow.isFirstUsed = admin.isFirstUsed;
        MainWindow.isSecondUsed = admin.isSecondUsed;
        MainWindow.isBox = admin.isBoxUsed;
        MainWindow.isGenetic = admin.isGeneticUsed;
    }
    
    private void CreateChart(object sender, RoutedEventArgs e)
    {
        var is2DChart = (sender as Button)?.Name == "Create2DChartButton";
        if (is2DChart)
        {
            if (double.TryParse(AlphaTextBox.Text, out alpha) && alpha is < 1 and > 0)
                new ChartsWindow().Show();
            else MessageBox.Show("Коэффициент пропорциональности не в правильном формате!");
        }
        else
        {
            new ChartsWindow3D().Show();
        }
    }

    private void Exit(object sender, RoutedEventArgs e)
    {
        Hide();
        new EnterWindow().Show();
        Close();
    }
}