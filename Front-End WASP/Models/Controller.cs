using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Front_End_WASP
{
    class Controller
    {
        public Controller() { }

        public List<int> getYearsForStudy(int idStudy)
        {
            List<int> result = new List<int>();
            Model model = new Model();
            IDataReader data = model.getYearsForStudy(idStudy);
            while (data.Read())
            {
                result.Add(Convert.ToInt32(data.GetValue(0).ToString()));
            }
            return result;
        }

        public List<int> getPeriodsForYear(int idStudy, int year)
        {
            List<int> result = new List<int>();
            Model model = new Model();
            IDataReader data = model.getPeriodsForYear(idStudy, year);
            while (data.Read())
            {
                result.Add(Convert.ToInt32(data.GetValue(0).ToString()));
            }
            return result;
        }

        public List<string> getHydroconditionsForPeriod(int idStudy, int year, int period)
        {
            List<string> result = new List<string>();
            Model model = new Model();
            IDataReader data = model.getHydroconditionsForPeriod(idStudy, year, period);
            while (data.Read())
            {
                result.Add(data.GetValue(0).ToString() + " | " + data.GetValue(1).ToString() + "%");
            }
            return result;
        }

        public Mersim getDataForHydrocondition(int idStudy, int year, int period, string hydrocondition)
        {
            Model model = new Model();
            int hydroconditionId = getHydroconditionId(hydrocondition);
            IDataReader dataTotal = model.getScenario(idStudy, year, period, hydroconditionId);
            IDataReader dataThermal = model.getThermalDataForScenario(idStudy, year, period, hydroconditionId);
            IDataReader dataHydro = model.getHydroDataForScenario(idStudy, year, period, hydroconditionId);
            Mersim mersimOne = new Mersim(dataTotal, dataThermal, dataHydro);
            return mersimOne;
        }

        private int getHydroconditionId(string hydrocondition)
        {
            string tmp = "";
            int counter = 0;
            while (hydrocondition[counter] != '|')
            {
                tmp += hydrocondition[counter];
                counter++;
            }
            return Convert.ToInt32(tmp.Trim());
        }

        public DataTable getPlantsTypes()
        {
            Model model = new Model();
            return model.getPlantTypes();
        }

        public void update(DataTable dt)
        {
            Model model = new Model();
            model.update(dt);
        }
    }
}
