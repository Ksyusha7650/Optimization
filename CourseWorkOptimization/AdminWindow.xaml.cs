using SciChart.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CourseWorkOptimization;

/// <summary>
///     Interaction isUsed for EnterWindow.xaml
/// </summary>
public partial class AdminWindow : Window
{
    public bool isFirstUsed, isSecondUsed, isBoxUsed, isGeneticUsed;
    public AdminWindow()
    {
        InitializeComponent();
        ReadFromFile();
        SetUpMethodsComboBox();
    }

    public void ReadFromFile()
    {
        var reader = File.OpenText("../../../Resources/Methods.csv");
        reader.ReadLine();
        var count = 0;
        while (reader.ReadLine() is { } line)
        {
            var array = line.Split(";").ToArray();
            var text = array[0];
            var isUsed = array[1] is "да" ? true : false;
            var checkBox =
            new CheckBox()
            {
                Content = text,
                IsChecked = isUsed
            };
            // Маппинг методов по названию
            if (text == "Метод градиентного спуска")
            {
                isFirstUsed = isUsed;
            }
            else if (text == "Метод Нестерова")
            {
                isSecondUsed = isUsed;
            }
            else if (text == "Бокс")
            {
                isBoxUsed = isUsed;
            }
            else if (text == "Генетический алгоритм")
            {
                isGeneticUsed = isUsed;
            }

            count++;
            MethodsStackPanel.Children.Add(checkBox);
        }
        reader.Close();
    }

    private void Add(object sender, RoutedEventArgs e)
    {
        var text = MethodTextBox.Text;
        var isUsed = IsUsedCheckBox.IsChecked;
        MethodsStackPanel.Children.Add(
            new CheckBox()
            {
                Content = text,
                IsChecked = isUsed
            });
        SetUpMethodsComboBox();
    }

    private void Send(object sender, RoutedEventArgs e)
    {
        var writer = File.CreateText("../../../Resources/Methods.csv");
        writer.WriteLine("Метод;Используется?");
        foreach (CheckBox element in MethodsStackPanel.Children)
        {
            var line = element.Content + ";" + ((bool)element.IsChecked ? "да" : "нет");
            writer.WriteLine(line);
            
        }
        writer.Close();
        Hide();
        new EnterWindow().Show();
        Close();
    }

    private void MethodTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var textbox = sender as TextBox;
        AddButton.IsEnabled = textbox.Text.Length > 0;
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var method = MethodsComboBox.SelectedItem.ToString();
        var list = MethodsStackPanel.Children.ToEnumerable();
        foreach(CheckBox element in MethodsStackPanel.Children)
        {
            if (element.Content == method)
            {
                MethodsStackPanel.Children.Remove(element);
                SetUpMethodsComboBox();
                return;
            }
        }
    }

    private void SetUpMethodsComboBox()
    {
        MethodsComboBox.Items.Clear();
        foreach (CheckBox element in MethodsStackPanel.Children)
        {
            MethodsComboBox.Items.Add(element.Content);
        }
    }

    private void MethodsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = sender as ComboBox;
        DeleteButton.IsEnabled = comboBox.SelectedIndex > 1 ? true : false;
    }
}