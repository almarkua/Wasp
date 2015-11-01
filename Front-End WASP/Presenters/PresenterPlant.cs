using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.ObjectModel;

namespace Front_End_WASP
{
    class PresenterPlant
    {
        private Model model;
        private IPlants view;

        public PresenterPlant(IPlants view)
        {
            model = new Model();
            this.view = view;
            this.view.deletePlant += OnDeletePlant;
            this.view.insertPlant += OnInsertPlant;
            this.view.updatePlant += OnUpdatePlant;
            view.updatePlants(getPlants());
            view.updateTypesOfPlants(getTypesOfPlants());
        }

        public ObservableCollection<Plant> getPlants()
        {
            ObservableCollection<Plant> plants = new ObservableCollection<Plant>();
            IDataReader tmpPlants = model.getPlants();
            while (tmpPlants.Read())
            {
                plants.Add(new Plant(tmpPlants.GetValue(0).ToString(), tmpPlants.GetValue(1).ToString(), tmpPlants.GetValue(2).ToString()));
            }
            return plants;
        }

        public ObservableCollection<string> getTypesOfPlants()
        {
            ObservableCollection<string> result = new ObservableCollection<string>();
            IDataReader tmpTypes = model.getTypesOfPlants();
            while (tmpTypes.Read())
            {
                result.Add(tmpTypes.GetString(0));
            }
            return result;
        }

        private void OnUpdatePlant(object sender, EventArgs e)
        {
            Plant currentPlant = (Plant)sender;
            model.updatePlant(currentPlant);
        }

        private void OnInsertPlant(object sender, EventArgs e)
        {
            Plant currentPlant = (Plant)sender;
            model.insertPlant(currentPlant);
        }

        private void OnDeletePlant(object sender, EventArgs e)
        {
            model.deletePlant((Plant)sender);
        }
    }
}
