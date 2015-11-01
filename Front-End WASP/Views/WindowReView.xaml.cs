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

namespace Front_End_WASP
{
    /// <summary>
    /// Логика взаимодействия для WindowReView.xaml
    /// </summary>
    public partial class WindowReView : Window, IReView
    {
        bool isToCompare = false;
        public WindowReView()
        {
            InitializeComponent();
            listOfStudy.SelectionMode = SelectionMode.Single;
            buttonShowStudy.Content = "Переглянути";
            btnChangeMode.Content = "Режим порівняння";
            btnDeleteStudy.Visibility = Visibility.Visible;
        }

        public WindowReView(bool isToCompare)
        {
            InitializeComponent();
            this.isToCompare = true;
            listOfStudy.SelectionMode = SelectionMode.Extended;
            buttonShowStudy.Content = "Порівняти";
            btnChangeMode.Content = "Режим перегляду";
            btnDeleteStudy.Visibility = Visibility.Collapsed;
        }

        public event EventHandler<EventArgs> selectItemE;
        public event EventHandler<EventArgs> deletedItemE;

        public int selectedItemId
        {
            get
            {
                Study tmp = (Study)listOfStudy.SelectedValue;
                return Convert.ToInt32(tmp.id);
            }
            set
            {
                if (listOfStudy.Items.Count > 0) listOfStudy.SelectedIndex = value;
            }
        }

        public List<Study> selectedStudies
        {
            get
            {
                List<Study> result = new List<Study>();
                foreach (Study tmp in listOfStudy.SelectedItems)
                {
                    result.Add(tmp);
                }
                return result;
            }
        }

        public string yearsOfStudy
        {
            set
            {
                labelYearsOfStudy.Content = value;
            }
        }

        public string periodsOfStudy
        {
            set
            {
                labelCountOfPeriods.Content = value;
            }
        }

        public string hydroconditionsOfStudy
        {
            set
            {
                labelCountOfHydrocondition.Content = value;
            }
        }

        public void fillListView(List<Study> studies)
        {
            listOfStudy.ItemsSource = studies;
        }

        private void listOfStudy_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (listOfStudy.SelectedIndex >= 0)
                selectItemE(this, EventArgs.Empty);
        }

        private void buttonShowStudy_Click_1(object sender, RoutedEventArgs e)
        {
            if (isToCompare)
            {
                if (listOfStudy.SelectedIndex >= 0)
                {
                    List<int> selectedStudiesId = new List<int>();
                    foreach (Study tmp in selectedStudies)
                    {
                        selectedStudiesId.Add(tmp.intId);
                    }
                    WindowCompare view = new WindowCompare();
                    PresenterCompare pr = new PresenterCompare(view, selectedStudiesId);
                    view.Owner = this;
                    view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    view.Show();
                }
            }
            else
            {
                if (listOfStudy.SelectedIndex >= 0)
                {
                    Study tmp = (Study)listOfStudy.SelectedValue;
                    WindowStudy wstudy = new WindowStudy(selectedItemId, tmp.name, tmp.date);
                    wstudy.Owner = this;
                    wstudy.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    wstudy.Show();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Оберіть дослідження для перегляду", "Помилка!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }

        private void btnChangeMode_Click_1(object sender, RoutedEventArgs e)
        {
            if (!isToCompare)
            {
                this.isToCompare = true;
                listOfStudy.SelectionMode = SelectionMode.Extended;
                buttonShowStudy.Content = "Порівняти";
                btnChangeMode.Content = "Режим перегляду";
                btnDeleteStudy.Visibility = Visibility.Collapsed;
            }
            else {
                this.isToCompare = false;
                listOfStudy.SelectionMode = SelectionMode.Single;
                buttonShowStudy.Content = "Переглянути";
                btnChangeMode.Content = "Режим порівняння";
                btnDeleteStudy.Visibility = Visibility.Visible;
            }
            
        }

        private void btnDeleteStudy_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ви дійсно бажаєте видалити дослідження?", "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                deletedItemE(this, EventArgs.Empty);
            }
        }
    }
}
