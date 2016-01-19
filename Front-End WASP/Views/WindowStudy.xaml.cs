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
    /// Логика взаимодействия для WindowsStudy.xaml
    /// </summary>
    public partial class WindowStudy : Window
    {
        private int idStudy;
        private Controller controller = new Controller();

        public WindowStudy(int tmpIdStudy, string name, string date)
        {
            InitializeComponent();
            idStudy = tmpIdStudy;
            this.Title = "Перегляд дослідження : " + name + " | " + date;
            fillWindow();
        }

        public void fillWindow()
        {
            List<int> years = controller.getYearsForStudy(idStudy);
            comboBoxYears.Items.Clear();
            foreach (int year in years)
            {
                comboBoxYears.Items.Add(year);
            }
            comboBoxYears.SelectedIndex = 0;
        }

        private void comboBoxYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<int> periods = controller.getPeriodsForYear(idStudy, Convert.ToInt32(comboBoxYears.SelectedValue.ToString()));
            comboBoxPeriods.Items.Clear();
            foreach (int period in periods)
            {
                comboBoxPeriods.Items.Add(period);
            }
            comboBoxPeriods.SelectedIndex = 0;
        }

        private void comboBoxHydroconditions_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxPeriods.SelectedIndex >= 0 && comboBoxHydroconditions.SelectedIndex >= 0)
            {
                Mersim mersimOne = controller.getDataForHydrocondition(idStudy, Convert.ToInt32(comboBoxYears.SelectedValue.ToString()), Convert.ToInt32(comboBoxPeriods.SelectedValue.ToString()), comboBoxHydroconditions.SelectedValue.ToString());
                setData(mersimOne);
            }
        }

        private void setData(Mersim mersimOne)
        {
            labelTotalCapacity.Content = mersimOne.totalCapacity[0];
            labelPeakLoad.Content = mersimOne.peakLoad[0];
            labelMinimumLoad.Content = mersimOne.minimumLoad[0];
            lableMaintenanceSpace.Content = mersimOne.maintenanceSpace[0];
            labelReserveCapacity.Content = mersimOne.reserveCapacity[0];
            labelTotalGeneration.Content = mersimOne.totalGeneration[0];
            labelEnergyDemand.Content = mersimOne.energyDemand[0];
            labelUnservedEnergy.Content = mersimOne.unservedEnergy[0];
            labelEnergyBalance.Content = mersimOne.energyBalance[0];
            labelLossOfLoad.Content = mersimOne.lossOfLoadProbability[0];
            labelEnergyPumped.Content = mersimOne.energyPumped[0];
            dataTableThermal.ItemsSource = mersimOne.thermalData[0].DefaultView;
            dataTableHydro.ItemsSource = mersimOne.hydroData[0].DefaultView;
        }

        private void comboBoxPeriods_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxPeriods.SelectedIndex >= 0)
            {
                List<string> hydroconditions = controller.getHydroconditionsForPeriod(idStudy, Convert.ToInt32(comboBoxYears.SelectedValue.ToString()), Convert.ToInt32(comboBoxPeriods.SelectedValue.ToString()));
                comboBoxHydroconditions.Items.Clear();
                foreach (string hydrocondition in hydroconditions)
                {
                    comboBoxHydroconditions.Items.Add(hydrocondition);
                }
                comboBoxHydroconditions.SelectedIndex = 0;
            }
        }

        private void expanderTotal_Expanded_1(object sender, RoutedEventArgs e)
        {
            dataTableThermal.Margin = new Thickness(10, 127, 10, 185);
        }

        private void expanderTotal_Collapsed_1(object sender, RoutedEventArgs e)
        {
            dataTableThermal.Margin = new Thickness(10, 127, 10, 25);
        }

        private void buttonExcelExport_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel File|*.xlsx";
            saveFileDialog.Title = "Зберегти як...";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != String.Empty)
            {
                System.Windows.MessageBox.Show(saveFileDialog.FileName);
                Excel.writeStudyToExcel(saveFileDialog.FileName, idStudy);
            }
        }

    }
}
