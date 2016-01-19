using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Front_End_WASP
{
    class PresenterReView
    {
        private Model model = new Model();
        private IReView view;
        private int selectedId;

        public PresenterReView(IReView view) {
            this.view = view;
            this.view.fillListView(getListOfStuies());
            this.view.selectItemE += new EventHandler<EventArgs>(OnSelectItem);
            this.view.deletedItemE += new EventHandler<EventArgs>(OnDeleteItem);
            this.view.selectedItemId = 0;

        }

        private void OnSelectItem(object sender, EventArgs e) {
            selectedId = view.selectedItemId;
            if (selectedId >= 0) {
                int minYear, maxYear;
                model.getMinAndMaxPeriodOfStudy(selectedId, out minYear, out maxYear);
                view.yearsOfStudy = minYear.ToString() + " - " + maxYear.ToString();
                view.periodsOfStudy = model.getMaxPeriodOfStudy(selectedId);
                view.hydroconditionsOfStudy = model.getMaxHydroconditionOfStudy(selectedId);
            }
        }

        private void OnDeleteItem(object sender, EventArgs e) {
            model.deleteStudy(view.selectedItemId);
            this.view.fillListView(getListOfStuies());
        }

        public List<Study> getListOfStuies() {
            List<Study> result = new List<Study>();
            IDataReader data = model.getStudies();
            while (data.Read()) {
                int tmp = Convert.ToInt32(data.GetValue(0).ToString());
                result.Add(new Study(Convert.ToInt32(data.GetValue(0).ToString()), data.GetValue(1).ToString(), Convert.ToDateTime(data.GetValue(2))));
            }
            return result;
        }
    }
}