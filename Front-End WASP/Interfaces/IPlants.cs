using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Front_End_WASP
{
    interface IPlants
    {
        event EventHandler<EventArgs> deletePlant;
        event EventHandler<EventArgs> insertPlant;
        event EventHandler<EventArgs> updatePlant;

        void updatePlants(ObservableCollection<Plant> plants);
        void updateTypesOfPlants(ObservableCollection<string> types);
    }
}
