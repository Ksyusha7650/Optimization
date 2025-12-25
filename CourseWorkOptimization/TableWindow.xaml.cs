using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseWorkOptimization
{
    /// <summary>
    /// Interaction logic for TableWindow.xaml
    /// </summary>
    public partial class TableWindow : Window
    {
        public TableWindow(List<Calculation> calculations, List<Calculation> calculations2, List<Calculation> calculationsBox, List<Calculation> calculationsGenetic = null)
        {
            InitializeComponent();
            SetTable(calculations, CalculationsDataGrid);
            SetTable(calculations2, Calculations2DataGrid);
            SetTable(calculationsBox, Calculations3DataGrid);
            if (calculationsGenetic != null && calculationsGenetic.Count > 0)
            {
                SetTable(calculationsGenetic, Calculations4DataGrid);
                LabelGenetic.Visibility = Visibility.Visible;
            }
        }

        private void SetTable(List<Calculation> calculations, DataGrid table)
        {
            var column = new DataGridTextColumn
            {
                Header = "T1, °С",
                Binding = new Binding("T1")
            };
            table.Columns.Add(column);
            column = new DataGridTextColumn
            {
                Header = "T2, °С",
                Binding = new Binding("T2")
            };
            table.Columns.Add(column);
            column = new DataGridTextColumn
            {
                Header = "Затраты на изготовление изделия, у.е.",
                Binding = new Binding("Value")
            };
            table.Columns.Add(column);
            foreach (var calculation in calculations)
                table.Items.Add(calculation);
        }
    }
}
