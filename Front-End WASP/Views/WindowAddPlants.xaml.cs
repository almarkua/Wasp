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
using System.Windows.Forms;

namespace Front_End_WASP
{
    /// <summary>
    /// Логика взаимодействия для WindowAddPlants.xaml
    /// </summary>
    public partial class WindowAddPlants : Window, IAddPlants
    {
        OpenFileDialog opf = new OpenFileDialog();
        public string file
        {
            get;
            private set;
        }
        public int countOfAddedPlants
        {
            get;
            set;
        }
        public event EventHandler<EventArgs> fileIsSelected;

        public WindowAddPlants()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            opf.Multiselect = false;
            opf.Filter = "Fix/Var sys файл|fixsys.rep;varsys.rep";
            opf.FileOk += opf_FileOk;
            opf.ShowDialog();
        }

        void opf_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            txtBoxFile.Text = opf.FileName;
            file = opf.FileName;
        }

        void btnAdd_Click(object sender, EventArgs e) {
            fileIsSelected(this, EventArgs.Empty);
            System.Windows.MessageBox.Show("Додаванн успішно завершено! Додано " + countOfAddedPlants + " типових блоків.", "Додавання завершено", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

    }
}
