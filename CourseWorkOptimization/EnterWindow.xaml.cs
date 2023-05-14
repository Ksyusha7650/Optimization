using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CourseWorkOptimization;

/// <summary>
///     Interaction logic for EnterWindow.xaml
/// </summary>
public partial class EnterWindow : Window
{
    private readonly string? _password = ConfigurationManager.AppSettings["Password"];
    private Enter _enterUser = Enter.None;

    public EnterWindow()
    {
        InitializeComponent();
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        var check = sender as CheckBox;
        switch (check?.Name)
        {
            case "AdminCheckBox":
            {
                UserCheckBox.IsChecked = false;
                _enterUser = Enter.Admin;
                break;
            }
            case "UserCheckBox":
            {
                AdminCheckBox.IsChecked = false;
                _enterUser = Enter.User;
                break;
            }
        }
    }

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        _enterUser = Enter.None;
    }

    private void EnterButton_Click(object sender, RoutedEventArgs e)
    {
        switch (_enterUser)
        {
            case Enter.None:
                MessageBox.Show("Выберите пользователя");
                return;
            case Enter.Admin when PasswordBox.Password == _password:
                Hide();
                new MainWindow().Show();
                Close();
                break;
            case Enter.Admin:
                MessageBox.Show("Пароль неправильный!");
                PasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            case Enter.User:
                Hide();
                new MainWindow().Show();
                Close();
                break;
        }
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        PasswordBox.BorderBrush = new SolidColorBrush(Colors.Black);
    }
}