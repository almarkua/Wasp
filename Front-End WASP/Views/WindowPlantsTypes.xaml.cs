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
using System.Data;

namespace Front_End_WASP.Views
{
    /// <summary>
    /// Логика взаимодействия для WindowPlantsTypes.xaml
    /// </summary>
    public partial class WindowPlantsTypes : Window
    {
        public WindowPlantsTypes()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            Controller ct = new Controller();
            dataTable.ItemsSource = ct.getPlantsTypes().DefaultView;
            dataTable.Columns[0].Width = 150;
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.dataTable.EndInit();
            Controller ct = new Controller();
            ct.update(((DataView)dataTable.ItemsSource).Table);
        }
    }
}
