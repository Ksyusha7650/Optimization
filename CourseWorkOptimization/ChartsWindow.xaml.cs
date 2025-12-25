using System;
using System.Windows;
using OxyPlot;
using OxyPlot.Series;

namespace CourseWorkOptimization;

public partial class ChartsWindow : Window
{
    public PlotModel MyModel { get; set; }
    private MyChart _myChart;

    public ChartsWindow()
    {
        InitializeComponent();
        _myChart = (MyChart)DataContext;
        MyModel = _myChart.MyModel;
        
        // Устанавливаем начальное состояние чекбоксов в зависимости от того, какие методы были вычислены
        CheckBoxGradient.IsChecked = MainWindow.isFirstUsed && _myChart.LineGradient != null;
        CheckBoxNesterov.IsChecked = MainWindow.isSecondUsed && _myChart.LineNesterov != null;
        CheckBoxBox.IsChecked = MainWindow.isBox && _myChart.LineBox != null;
        CheckBoxGenetic.IsChecked = MainWindow.isGenetic && _myChart.LineGenetic != null;
        
        // Скрываем чекбоксы для методов, которые не были вычислены
        CheckBoxGradient.Visibility = MainWindow.isFirstUsed && _myChart.LineGradient != null ? Visibility.Visible : Visibility.Collapsed;
        CheckBoxNesterov.Visibility = MainWindow.isSecondUsed && _myChart.LineNesterov != null ? Visibility.Visible : Visibility.Collapsed;
        CheckBoxBox.Visibility = MainWindow.isBox && _myChart.LineBox != null ? Visibility.Visible : Visibility.Collapsed;
        CheckBoxGenetic.Visibility = MainWindow.isGenetic && _myChart.LineGenetic != null ? Visibility.Visible : Visibility.Collapsed;
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        UpdateSeriesVisibility();
    }

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        UpdateSeriesVisibility();
    }

    private void UpdateSeriesVisibility()
    {
        if (_myChart == null) return;

        // Обновляем видимость серий в зависимости от состояния чекбоксов
        if (_myChart.LineGradient != null)
        {
            _myChart.LineGradient.IsVisible = CheckBoxGradient.IsChecked == true;
        }
        if (_myChart.LineNesterov != null)
        {
            _myChart.LineNesterov.IsVisible = CheckBoxNesterov.IsChecked == true;
        }
        if (_myChart.LineBox != null)
        {
            _myChart.LineBox.IsVisible = CheckBoxBox.IsChecked == true;
        }
        if (_myChart.LineGenetic != null)
        {
            _myChart.LineGenetic.IsVisible = CheckBoxGenetic.IsChecked == true;
        }

        // Обновляем график
        MyModel.InvalidatePlot(true);
    }
}