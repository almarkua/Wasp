using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Front_End_WASP
{
    interface IReView
    {
        int selectedItemId { get; set; }
        List<Study> selectedStudies { get; }
        void fillListView(List<Study> studies);
        event EventHandler<EventArgs> selectItemE;
        event EventHandler<EventArgs> deletedItemE;
        string yearsOfStudy { set; }
        string periodsOfStudy { set; }
        string hydroconditionsOfStudy { set; }
    }
}
