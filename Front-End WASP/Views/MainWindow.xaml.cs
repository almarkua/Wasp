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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using Front_End_WASP.Views;
namespace Front_End_WASP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void addNewStudyItem_Click_1(object sender, RoutedEventArgs e)
        {
            WindowAddNewStudy view = new WindowAddNewStudy();
            view.Title = "Додавання нового дослідження";
            view.Owner = this;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            view.ShowDialog();
        }

        private void exitItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void viewStudyItem_Click_1(object sender, EventArgs e) {
            WindowReView window = new WindowReView();
            window.Title = "Список досліджень";
            PresenterReView presenter = new PresenterReView(window);
            window.Owner = this;
            window.ShowDialog();
        }

        void compareStudyItem_Click_1(object sender, EventArgs e) {
            WindowReView window = new WindowReView(true);
            window.Title = "Список досліджень";
            PresenterReView presenter = new PresenterReView(window);
            window.Owner = this;
            window.ShowDialog();
        }

        void blockTypesListItem_Click(object sender, EventArgs e) {
            WindowPlantsTypes ptw = new WindowPlantsTypes();
            ptw.Title = "Список типів енергоблоків";
            ptw.Owner = this;
            ptw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ptw.ShowDialog();
        }

        void blockListItem_Click(object sender, EventArgs e) {
            WindowPlants pw = new WindowPlants();
            pw.Title = "Список енергоблоків";
            pw.Owner = this;
            pw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            PresenterPlant pres = new PresenterPlant(pw);
            pw.ShowDialog();
        }

        void addNewBlocksItem_Click(object sender, EventArgs e) {
            WindowAddPlants view = new WindowAddPlants();
            view.Title = "Додавання енергоблоків з файлу";
            view.Owner = this;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            PresenterAddPlants presenter = new PresenterAddPlants(view);
            view.ShowDialog();
        }

        
    }
}
