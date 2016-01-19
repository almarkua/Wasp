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
using Front_End_WASP.Interfaces;

namespace Front_End_WASP.Views
{
    /// <summary>
    /// Логика взаимодействия для WindowAddNewStudy.xaml
    /// </summary>
    public partial class WindowAddNewStudy : Window, IAddStudy
    {
        private System.Windows.Forms.OpenFileDialog fileDialog;
        public string date
        {
            get;
            private set;
        }

        public string name
        {
            get;
            private set;
        }

        public string fileName
        {
            get;
            private set;
        }

        public WindowAddNewStudy()
        {
            InitializeComponent();
            fileDialog = new System.Windows.Forms.OpenFileDialog();
            datePicker.SelectedDateChanged += datePicker_SelectedDateChanged;
            datePicker.Text = DateTime.Now.Date.ToString();
            txtBoxFileName.IsReadOnly = true;
            txtBoxFileName.GotFocus += txtBoxFileName_GotFocus;
        }

        void txtBoxFileName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (fileName==null || fileName==String.Empty)btnOpen_Click(null, EventArgs.Empty);
        }

        void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            date = datePicker.SelectedDate.Value.Date.ToString();
            setBtnAddVisible();
        }

        void btnOpen_Click(object sender, EventArgs e) {
            fileDialog.Multiselect = false;
            fileDialog.Filter = "Файл звіту|*.rep|Текстовий документ|*.txt";
            fileDialog.FileOk += fileDialog_FileOk;
            fileDialog.ShowDialog();
        }

        void fileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            fileName = fileDialog.FileName;
            txtBoxFileName.Text = fileDialog.FileName;
            setBtnAddVisible();
        }

        void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        void txtBoxName_TextChanged(object sender, EventArgs e) {
            name = txtBoxName.Text;
            setBtnAddVisible();
        }

        void btnAdd_Click(object sender, EventArgs e) {
            Mersim mersim = new Mersim(fileDialog.FileName);
            mersim.readFile();
            Model tmpModel = new Model();
            tmpModel.addNewStudy(name, Convert.ToDateTime(date), mersim);
            this.Close();
            MessageBox.Show("Дослідженя успішно додано!","Додавання завершено",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        void setBtnAddVisible() {
            if (name!=null && date!=null && fileName!=null && name.Trim().Length > 0 && date.Trim().Length > 0 && fileName.Length > 0)
            {
                btnAdd.IsEnabled = true;
            }
            else {
                btnAdd.IsEnabled = false;
            }
        }
    }
}
