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
using System.Windows.Controls.DataVisualization.Charting;
using System.Collections.ObjectModel;

namespace Front_End_WASP
{
    /// <summary>
    /// Логика взаимодействия для WindowChart.xaml
    /// </summary>
    public partial class WindowChart : Window
    {
        Dictionary<string, Dictionary<string, ObservableCollection<Point>>> pointsDict = new Dictionary<string, Dictionary<string, ObservableCollection<Point>>>();
        Dictionary<string, ObservableCollection<Point>> pointsDictForStudy = new Dictionary<string, ObservableCollection<Point>>();
        DataTable dataTable;
        bool isCutVersionOfDataTable = true;

        public WindowChart(DataTable data)
        {
            InitializeComponent();
            this.dataTable = data;
            setDictionary();
            drawCollumnChart();
        }

        private void setDictionary()
        {
            if (dataTable.Columns.IndexOf("Тип блоків") > 0)
            {
                isCutVersionOfDataTable = false;
                for (int i = 0; i < dataTable.Rows.Count; ++i)
                {
                    if (!pointsDict.ContainsKey(dataTable.Rows[i].Field<string>("Назва дослідження")))
                    {
                        pointsDict.Add(dataTable.Rows[i].Field<string>("Назва дослідження"), new Dictionary<string, ObservableCollection<Point>>());
                    }
                    if (!pointsDict[dataTable.Rows[i].Field<string>("Назва дослідження")].ContainsKey(dataTable.Rows[i].Field<string>("Тип блоків")))
                    {
                        pointsDict[dataTable.Rows[i].Field<string>("Назва дослідження")].Add(dataTable.Rows[i].Field<string>("Тип блоків"), new ObservableCollection<Point>());
                    }
                    pointsDict[dataTable.Rows[i].Field<string>("Назва дослідження")][dataTable.Rows[i].Field<string>("Тип блоків")].Add(new Point(dataTable.Rows[i].Field<int>("Рік"), dataTable.Rows[i].Field<double>("Загалом")));
                }
            }
            else {
                for (int i = 0; i < dataTable.Rows.Count; ++i)
                {
                    if (!pointsDictForStudy.ContainsKey(dataTable.Rows[i].Field<string>("Назва дослідження")))
                    {
                        pointsDictForStudy.Add(dataTable.Rows[i].Field<string>("Назва дослідження"), new ObservableCollection<Point>());
                    }
                    pointsDictForStudy[dataTable.Rows[i].Field<string>("Назва дослідження")].Add(new Point(dataTable.Rows[i].Field<int>("Рік"), dataTable.Rows[i].Field<double>("Загалом")));
                }
            }
        }

        private void drawGraph()
        {
            if (!isCutVersionOfDataTable)
            {
                List<string> studyKeys = pointsDict.Keys.ToList<string>();
                foreach (string studyKey in studyKeys)
                {
                    List<string> typeKeys = pointsDict[studyKey].Keys.ToList<string>();
                    foreach (string typeKey in typeKeys)
                    {
                        LineSeries newChart = new LineSeries();
                        newChart.DependentValuePath = "Y";
                        newChart.IndependentValuePath = "X";
                        newChart.Title = studyKey + " | " + typeKey;
                        newChart.ItemsSource = pointsDict[studyKey][typeKey];
                        newChart.Refresh();
                        Charts.Series.Add(newChart);
                    }
                }
            }
            else {
                List<string> studyKeys = pointsDictForStudy.Keys.ToList<string>();
                foreach (string studyKey in studyKeys)
                {
                    LineSeries newChart = new LineSeries();
                    newChart.DependentValuePath = "Y";
                    newChart.IndependentValuePath = "X";
                    newChart.Title = studyKey;
                    newChart.ItemsSource = pointsDictForStudy[studyKey];
                    newChart.Refresh();
                    Charts.Series.Add(newChart);
                }
            }
        }

        void drawCollumnChart() {
            if (!isCutVersionOfDataTable)
            {
                List<string> studyKeys = pointsDict.Keys.ToList<string>();
                foreach (string studyKey in studyKeys)
                {
                    List<string> typeKeys = pointsDict[studyKey].Keys.ToList<string>();
                    foreach (string typeKey in typeKeys)
                    {
                        ColumnSeries newChart = new ColumnSeries();
                        newChart.DependentValuePath = "Y";
                        newChart.IndependentValuePath = "X";
                        newChart.Title = studyKey + " | " + typeKey;
                        newChart.ItemsSource = pointsDict[studyKey][typeKey];
                        newChart.Refresh();
                        Charts.Series.Add(newChart);
                    }
                }
            }
            else
            {
                List<string> studyKeys = pointsDictForStudy.Keys.ToList<string>();
                foreach (string studyKey in studyKeys)
                {
                    ColumnSeries newChart = new ColumnSeries();
                    newChart.DependentValuePath = "Y";
                    newChart.IndependentValuePath = "X";
                    newChart.Title = studyKey;
                    newChart.ItemsSource = pointsDictForStudy[studyKey];
                    newChart.Refresh();
                    Charts.Series.Add(newChart);
                }
            }
        }
    }
}
