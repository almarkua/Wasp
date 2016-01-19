using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Front_End_WASP
{
    interface IAddPlants
    {
        string file
        {
            get;
        }
        int countOfAddedPlants
        {
            get;
            set;
        }
        event EventHandler<EventArgs> fileIsSelected;
    }
}
