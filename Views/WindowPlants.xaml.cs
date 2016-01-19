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

namespace Front_End_WASP
{
    /// <summary>
    /// Логика взаимодействия для PlantsWindow.xaml
    /// </summary>
    /// 
    public partial class WindowPlants : Window, IPlants
    {
        public bool isAddedRow = false, isUpdatedRow = false;
        private Plant currentPlantForEdit = null;
        ObservableCollection<Plant> plantsCollection = new ObservableCollection<Plant>();
        public static ObservableCollection<string> typesOfPlants = new ObservableCollection<string>();


        public event EventHandler<EventArgs> deletePlant, insertPlant, updatePlant;

        public WindowPlants()
        {
            InitializeComponent();
            plantsCollection.CollectionChanged += plantsCollection_CollectionChanged;
            dataTable.InitializingNewItem += dataTable_InitializingNewItem;
            dataTable.RowEditEnding += dataTable_RowEditEnding;
            typesComboBox.ItemsSource = typesOfPlants;

        }

        public void updateTypesOfPlants(ObservableCollection<string> types)
        {
            typesComboBox.ItemsSource = types;
        }


        void plantsCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action.ToString() == "Remove")
            {
                foreach (Plant tmp in e.OldItems)
                {
                    deletePlant(tmp, EventArgs.Empty);
                }
            }
        }

        public void updatePlants(ObservableCollection<Plant> plants)
        {
            plantsCollection = plants;
            plantsCollection.CollectionChanged += plantsCollection_CollectionChanged;
            dataTable.ItemsSource = plantsCollection;
        }

        void dataTable_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            commitEdit();
            if (isAddedRow)
            {
                isAddedRow = false;
                dataTable.CommitEdit();
                insertPlant(dataTable.SelectedItem, EventArgs.Empty);
            }
            else
            {
                isUpdatedRow = true;
                currentPlantForEdit = (Plant)dataTable.SelectedItem;
            }
        }

        private void commitEdit()
        {
            if (currentPlantForEdit != null)
            {
                updatePlant(currentPlantForEdit, EventArgs.Empty);
                currentPlantForEdit = null;
            }
        }

        void dataTable_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            isAddedRow = true;
        }
        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dataTable.CommitEdit();
            dataTable.CommitEdit();
            commitEdit();
        }

    }
}
