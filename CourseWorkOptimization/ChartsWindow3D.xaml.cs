using System;
using System.Windows;
using ChartDirector;
using OxyPlot;
using OxyPlot.Series;

namespace CourseWorkOptimization;

public partial class ChartsWindow3D : Window
{
    private Charts _charts;

    public ChartsWindow3D()
    {
        InitializeComponent();
        
        try
        {
            if (Chart3D == null)
            {
                MessageBox.Show("Ошибка инициализации графика", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            _charts = new Charts(Chart3D);
            
            // Устанавливаем начальное состояние чекбоксов с проверкой на null
            if (CheckBoxGradient != null)
                CheckBoxGradient.IsChecked = MainWindow.isFirstUsed;
            if (CheckBoxNesterov != null)
                CheckBoxNesterov.IsChecked = MainWindow.isSecondUsed;
            if (CheckBoxBox != null)
                CheckBoxBox.IsChecked = MainWindow.isBox;
            if (CheckBoxGenetic != null)
                CheckBoxGenetic.IsChecked = MainWindow.isGenetic;
            
            // Скрываем чекбоксы для методов, которые не были вычислены
            if (CheckBoxGradient != null)
                CheckBoxGradient.Visibility = MainWindow.isFirstUsed ? Visibility.Visible : Visibility.Collapsed;
            if (CheckBoxNesterov != null)
                CheckBoxNesterov.Visibility = MainWindow.isSecondUsed ? Visibility.Visible : Visibility.Collapsed;
            if (CheckBoxBox != null)
                CheckBoxBox.Visibility = MainWindow.isBox ? Visibility.Visible : Visibility.Collapsed;
            if (CheckBoxGenetic != null)
                CheckBoxGenetic.Visibility = MainWindow.isGenetic ? Visibility.Visible : Visibility.Collapsed;
            
            UpdateChart();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при инициализации 3D графика: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        UpdateChart();
    }

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        UpdateChart();
    }

    private void UpdateChart()
    {
        try
        {
            if (_charts == null || Chart3D == null) return;
            
            if (CheckBoxGradient != null)
                _charts.ShowGradient = CheckBoxGradient.IsChecked == true;
            if (CheckBoxNesterov != null)
                _charts.ShowNesterov = CheckBoxNesterov.IsChecked == true;
            if (CheckBoxBox != null)
                _charts.ShowBox = CheckBoxBox.IsChecked == true;
            if (CheckBoxGenetic != null)
                _charts.ShowGenetic = CheckBoxGenetic.IsChecked == true;
            
            _charts.createChart(Chart3D);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при обновлении графика: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}