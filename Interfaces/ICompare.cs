using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Front_End_WASP
{
    interface ICompare
    {
        event EventHandler<EventArgs> changeType;
        event EventHandler<EventArgs> changeFilters;
        void updateDataTable(System.Data.DataTable dataTable);
        void updateTypesOfBlocks(List<string> types);
        ListChoiceVM listCheckBoxObj {
            get;
        }
        int minYear { get; set; }
        int maxYear { get; set; }
        int currentMinYear { get; }
        int currentMaxYear { get; }
        void refreshFilters(int yearFor, int yearTo, List<string> types);
        string mode { get; }
    }
}
