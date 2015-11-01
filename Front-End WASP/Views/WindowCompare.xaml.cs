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
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Front_End_WASP
{
    /// <summary>
    /// Логика взаимодействия для WindowCompare.xaml
    /// </summary>
    public partial class WindowCompare : Window, ICompare
    {
        public event EventHandler<EventArgs> changeType = null;
        public event EventHandler<EventArgs> changeFilters = null;
        public ListChoiceVM listCheckBoxObj
        {
            get;
            private set;
        }

        public int minYear
        {
            get;
            set;
        }
        public int maxYear
        {
            get;
            set;
        }
        public int currentMinYear
        {
            get;
            private set;
        }
        public int currentMaxYear
        {
            get;
            private set;
        }

        public string mode
        {
            get;
            private set;
        }

        public WindowCompare()
        {
            InitializeComponent();
            typeComboBox.Items.Add("Загальна генерація");
            typeComboBox.Items.Add("Витрати (без будівництва)");
            typeComboBox.Items.Add("Потужність");
            mode = "по типах блоків";
            typeComboBox.SelectedValue = typeComboBox.Items[0];
            comboBoxMode.Items.Add("по типах блоків");
            comboBoxMode.Items.Add("по сценаріях");
            comboBoxMode.SelectedIndex = 0;
            txtBoxYearFor.Text = currentMinYear.ToString();
            txtBoxYearTo.Text = currentMaxYear.ToString();

        }

        public void updateTypesOfBlocks(List<string> types) {
            listBoxWitchCheckBox.Items.Clear();
            listCheckBoxObj = new ListChoiceVM(types);
            listBoxWitchCheckBox.ItemsSource = listCheckBoxObj.Values;
        }

        public void updateYears(int yearFor, int yearTo) {
            txtBoxYearFor.Text = yearFor.ToString();
            txtBoxYearFor_LostFocus_1(null, null);
            txtBoxYearTo.Text = yearTo.ToString();
            txtBoxYearTo_LostFocus_1(null, null);
        }

        public void refreshFilters(int yearFor, int yearTo, List<string> types)
        {
            updateYears(yearFor, yearTo);
            updateTypesOfBlocks(types);
        }

        public void updateDataTable(DataTable dataTable)
        {
            this.dataTable.ItemsSource = dataTable.DefaultView;
        }

        private void typeComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (changeType != null)
            {
                changeType(typeComboBox.SelectedValue, EventArgs.Empty);
            }
        }

        private void buttonChart_Click_1(object sender, RoutedEventArgs e)
        {
            WindowChart view = new WindowChart(((DataView)dataTable.ItemsSource).ToTable());
            view.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            changeFilters(typeComboBox.SelectedValue, EventArgs.Empty);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            foreach (var tmp in listCheckBoxObj.Values)
            {
                tmp.IsSelected = true;
            }
            updateYears(minYear, maxYear);
            changeFilters(typeComboBox.SelectedValue, EventArgs.Empty);
        }

        private void txtBoxYearFor_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            int oldCaretIndex = txtBoxYearFor.CaretIndex;
            for (int i = 0; i < txtBoxYearFor.Text.Length; i++)
            {
                if (txtBoxYearFor.Text.Length > 0 && !Char.IsDigit(txtBoxYearFor.Text[i]))
                {
                    txtBoxYearFor.Text = txtBoxYearFor.Text.Remove(i,1);
                    txtBoxYearFor.CaretIndex = oldCaretIndex-1;
                    break;
                }
            }
        }

        private void txtBoxYearFor_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (txtBoxYearFor.Text.Length > 0) {
                currentMinYear = int.Parse(txtBoxYearFor.Text);
                if (currentMinYear < minYear) {
                    currentMinYear=minYear;
                }
                if (currentMinYear > maxYear) {
                    currentMinYear = maxYear;
                }
                if (currentMaxYear > 0 && currentMinYear > currentMaxYear) {
                    currentMinYear = currentMaxYear;
                }
                txtBoxYearFor.Text = currentMinYear.ToString();
            }
        }

        private void txtBoxYearTo_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            int oldCaretIndex = txtBoxYearTo.CaretIndex;
            for (int i = 0; i < txtBoxYearTo.Text.Length; i++)
            {
                if (txtBoxYearTo.Text.Length > 0 && !Char.IsDigit(txtBoxYearTo.Text[i]))
                {
                    txtBoxYearTo.Text = txtBoxYearTo.Text.Remove(i, 1);
                    txtBoxYearTo.CaretIndex = oldCaretIndex - 1;
                    break;
                }
            }
        }

        private void txtBoxYearTo_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (txtBoxYearTo.Text.Length > 0)
            {
                currentMaxYear = int.Parse(txtBoxYearTo.Text);
                if (currentMaxYear < minYear)
                {
                    currentMaxYear = minYear;
                }
                if (currentMaxYear > maxYear)
                {
                    currentMaxYear = maxYear;
                }
                if (currentMinYear > 0 && currentMaxYear < currentMinYear)
                {
                    currentMaxYear = currentMinYear;
                }
                txtBoxYearTo.Text = currentMaxYear.ToString();
            }
        }

        private void comboBoxMode_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            mode = comboBoxMode.SelectedValue.ToString();
            if (changeType != null)
            {
                changeType(typeComboBox.SelectedValue, EventArgs.Empty);
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void buttonExcel_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel File|*.xlsx";
            saveFileDialog.Title = "Зберегти як...";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != String.Empty)
            {
                Excel.saveData(saveFileDialog.FileName, ((DataView)dataTable.ItemsSource).ToTable());
            }
        }



    }
}
