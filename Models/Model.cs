using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace Front_End_WASP
{
    class Model
    {
        public Model() { 
            
        }

        public void addNewStudy(string name, DateTime date,Mersim mersimObj) {
            try
            {
                string tmpDate = date.ToString("yyyy-M-d");
                string query = String.Format("insert into study (name,date) values('{0}','{1}');", name, date.ToString("yyyy-M-d"));
                DB.getInstance().executeNonQuery(query);
                
                IDataReader result = DB.getInstance().execute("select @@IDENTITY from study");
                result.Read();
                object tmp = result.GetValue(0);
                int identityStudy = int.Parse(tmp.ToString());

                for (int i = 0; i < mersimObj.thermalData.Count; i++) {
                    query = String.Format(@"insert into scenario (total_capacity,peak_load,minimum_load,maintenance_space,reserve_capacity,total_generation,energy_demand,unserved_energy,energy_balance,loss_of_load,energy_pumped,period,year,id_study,hydrocondition,probality) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}');", mersimObj.totalCapacity[i].ToString().Replace(',', '.'), mersimObj.peakLoad[i].ToString().Replace(',', '.'), mersimObj.minimumLoad[i].ToString().Replace(',', '.'), mersimObj.maintenanceSpace[i].ToString().Replace(',', '.'), mersimObj.reserveCapacity[i].ToString().Replace(',', '.'), mersimObj.totalGeneration[i].ToString().Replace(',', '.'), mersimObj.energyDemand[i].ToString().Replace(',', '.'), mersimObj.unservedEnergy[i].ToString().Replace(',', '.'), mersimObj.energyBalance[i].ToString().Replace(',', '.'), mersimObj.lossOfLoadProbability[i].ToString().Replace(',', '.'), mersimObj.energyPumped[i].ToString().Replace(',', '.'), mersimObj.period[i], mersimObj.year[i], identityStudy, mersimObj.hydrocondition[i].ToString().Replace(',', '.'), mersimObj.probality[i].ToString().Replace(',', '.'));
                    DB.getInstance().executeNonQuery(query);
                    result = DB.getInstance().execute("select @@IDENTITY from scenario");
                    result.Read();
                    int identityScenario = int.Parse(result.GetValue(0).ToString());
                    for (int j=0;j<mersimObj.thermalData[i].Rows.Count;j++) {
                        query = String.Format("insert into thermal_plants (id_scenario,plant_name,number_of_units,unit_base,total_capacity,plant_capacity,base_energy,peak_energy,total_energy,fuel_domestic,fuel_foreign,fuel_total,om_domestic,main_probality,for_cell,capacity_factor) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}');", identityScenario, mersimObj.thermalData[i].Rows[j].Field<string>(0), int.Parse(mersimObj.thermalData[i].Rows[j].Field<double>(1).ToString()), mersimObj.thermalData[i].Rows[j].Field<double>(2).ToString().Replace(',', '.'), mersimObj.thermalData[i].Rows[j].Field<double>(3).ToString().Replace(',', '.'), mersimObj.thermalData[i].Rows[j].Field<double>(4).ToString().Replace(',', '.'), mersimObj.thermalData[i].Rows[j].Field<double>(5).ToString().Replace(',', '.'), mersimObj.thermalData[i].Rows[j].Field<double>(6).ToString().Replace(',', '.'), mersimObj.thermalData[i].Rows[j].Field<double>(7).ToString().Replace(',', '.'), mersimObj.thermalData[i].Rows[j].Field<double>(8).ToString().Replace(',', '.'), mersimObj.thermalData[i].Rows[j].Field<double>(9).ToString().Replace(',', '.'), mersimObj.thermalData[i].Rows[j].Field<double>(10).ToString().Replace(',', '.'), mersimObj.thermalData[i].Rows[j].Field<double>(11).ToString().Replace(',', '.'), mersimObj.thermalData[i].Rows[j].Field<double>(12).ToString().Replace(',', '.'), mersimObj.thermalData[i].Rows[j].Field<double>(13).ToString().Replace(',', '.'), mersimObj.thermalData[i].Rows[j].Field<double>(14).ToString().Replace(',', '.'));
                        DB.getInstance().executeNonQuery(query);
                    }
                    for (int j = 0; j < mersimObj.hydroData[i].Rows.Count; j++)
                    {
                        query = String.Format("insert into hydro_plants (idscenario,name,number_of_units,lord_pos_pl,u,base_capacity,peak_capacity,total_capacity,base_energy,peak_energy,total_energy,peak_mineng,energy_spilled,energy_shortage,om_local,capacity_factor) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}');", identityScenario, mersimObj.hydroData[i].Rows[j].Field<string>(0), mersimObj.hydroData[i].Rows[j].Field<double>(1).ToString().Replace(',', '.'), mersimObj.hydroData[i].Rows[j].Field<double>(2).ToString().Replace(',', '.'), mersimObj.hydroData[i].Rows[j].Field<double>(3).ToString().Replace(',', '.'), mersimObj.hydroData[i].Rows[j].Field<double>(4).ToString().Replace(',', '.'), mersimObj.hydroData[i].Rows[j].Field<double>(5).ToString().Replace(',', '.'), mersimObj.hydroData[i].Rows[j].Field<double>(6).ToString().Replace(',', '.'), mersimObj.hydroData[i].Rows[j].Field<double>(7).ToString().Replace(',', '.'), mersimObj.hydroData[i].Rows[j].Field<double>(8).ToString().Replace(',', '.'), mersimObj.hydroData[i].Rows[j].Field<double>(9).ToString().Replace(',', '.'), mersimObj.hydroData[i].Rows[j].Field<double>(10).ToString().Replace(',', '.'), mersimObj.hydroData[i].Rows[j].Field<double>(11).ToString().Replace(',', '.'), mersimObj.hydroData[i].Rows[j].Field<double>(12).ToString().Replace(',', '.'), mersimObj.hydroData[i].Rows[j].Field<double>(13).ToString().Replace(',', '.'), mersimObj.hydroData[i].Rows[j].Field<double>(14).ToString().Replace(',', '.'));
                        DB.getInstance().executeNonQuery(query);
                    }
                }

                result = DB.getInstance().execute("select count(*) from hydro_plants");
                result.Read();
                tmp = result.GetValue(0);

                return;
            }
            catch (Exception ex) {
                MessageBox.Show("Помилка додавання нового дослідження в базу даних!"+ex.Message);
            }
        }

        public IDataReader getYearsForStudy(int idStudy) {
            return DB.getInstance().execute(String.Format("select distinct year from study,scenario where study.id=scenario.id_study and study.id='{0}'", idStudy));
        }

        public IDataReader getPeriodsForYear(int idStudy, int year) {
            return DB.getInstance().execute(String.Format("select distinct period from study,scenario where study.id=scenario.id_study and study.id='{0}' and year='{1}'", idStudy,year));
        }

        public IDataReader getStudyForId(int idStudy) {
            return DB.getInstance().execute(String.Format("select name,date from study where id='{0}'", idStudy));
        }

        public IDataReader getScanariosForStudy(int idStudy) {
            return DB.getInstance().execute(String.Format("select scenario.id,year,period,hydrocondition,probality,total_capacity,total_generation,peak_load,energy_demand,minimum_load,unserved_energy,maintenance_space,energy_balance,reserve_capacity,loss_of_load,energy_pumped from study,scenario where scenario.id_study=study.id and study.id='{0}'", idStudy));
        }

        internal IDataReader getHydroconditionsForPeriod(int idStudy, int year, int period)
        {
            return DB.getInstance().execute(String.Format("select distinct hydrocondition,probality from study,scenario where study.id=scenario.id_study and study.id='{0}' and year='{1}' and period='{2}'", idStudy, year,period));
        }

        internal IDataReader getScenario(int idStudy, int year, int period, int hydrocondition)
        {
            return DB.getInstance().execute(String.Format("select total_capacity,peak_load,minimum_load,maintenance_space,reserve_capacity,total_generation,energy_demand,unserved_energy,energy_balance,loss_of_load,energy_pumped from study,scenario where study.id=scenario.id_study and study.id='{0}' and year='{1}' and period='{2}' and hydrocondition='{3}'", idStudy, year, period,hydrocondition));
        }

        internal IDataReader getThermalDataForScenario(int idStudy, int year, int period, int hydroconditionId)
        {
            return DB.getInstance().execute(String.Format("select t.plant_name,t.number_of_units,t.unit_base,t.total_capacity,t.plant_capacity,t.base_energy,t.peak_energy,t.total_energy,t.fuel_domestic,t.fuel_foreign,t.fuel_total,t.om_domestic,t.main_probality,t.for_cell,t.capacity_factor from study,scenario,thermal_plants as t where study.id=scenario.id_study and study.id='{0}' and year='{1}' and period='{2}' and hydrocondition='{3}' and t.id_scenario=scenario.id", idStudy, year, period, hydroconditionId));
        }

        internal IDataReader getHydroDataForScenario(int idStudy, int year, int period, int hydroconditionId)
        {
            return DB.getInstance().execute(String.Format("select h.name,h.number_of_units,h.lord_pos_pl,h.u,h.base_capacity,h.peak_capacity,h.total_capacity,h.base_energy,h.peak_energy,h.total_energy,h.peak_mineng,h.energy_spilled,h.energy_shortage,h.om_local,h.capacity_factor from study,scenario,hydro_plants as h where study.id=scenario.id_study and study.id='{0}' and year='{1}' and period='{2}' and hydrocondition='{3}' and idscenario=scenario.id", idStudy, year, period, hydroconditionId));
        }

        internal IDataReader getStudies() {
            string query = "select id,name,date from study";
            return DB.getInstance().execute(query);
        }

        internal void getMinAndMaxPeriodOfStudy(int selectedId, out int minYear, out int maxYear)
        {
            string query = String.Format("select distinct min(year),max(year) from study,scenario where scenario.id_study={0}", selectedId);
            IDataReader tmp = DB.getInstance().execute(query);
            tmp.Read();
            minYear = Convert.ToInt32(tmp.GetValue(0));
            maxYear = Convert.ToInt32(tmp.GetValue(1));
        }

        internal string getMaxPeriodOfStudy(int selectedId)
        {
            string query = String.Format("select distinct max(period) from study,scenario where scenario.id_study={0}", selectedId);
            IDataReader tmp = DB.getInstance().execute(query);
            tmp.Read();
            return tmp.GetValue(0).ToString();
        }

        internal string getMaxHydroconditionOfStudy(int selectedId)
        {
            string query = String.Format("select distinct max(hydrocondition) from study,scenario where scenario.id_study={0}", selectedId);
            IDataReader tmp = DB.getInstance().execute(query);
            tmp.Read();
            return tmp.GetValue(0).ToString();
        }

        internal DataTable getPlantTypes()
        {
            string query = "select * from types_of_plants";
            return DB.getInstance().executeForUpdate(query);
        }

        internal void update(DataTable dt)
        {
            try
            {
                DB.getInstance().update(dt);
            }
            catch (Exception ex) {
                MessageBox.Show("Помилка внесення змін в базу даних. \n" + ex.Message);
            }
        }

        public IDataReader getPlants() {
            string query = "select p.id,p.full_name,p.typeName from plants as p";
            return DB.getInstance().execute(query);
        }

        public void insertPlant(Plant insertedPlant)
        {
            try
            {
                string query = String.Format("insert into plants (id,full_name,typeName) values ('{0}','{1}','{2}')", insertedPlant.shortName, insertedPlant.fullName, insertedPlant.idType);
                DB.getInstance().executeNonQuery(query);
            }
            catch (Exception ex) {
                MessageBox.Show("Помилка додавання станції в базу даних! ВВедено некоректні дані. \n" + ex.Message);
            }

        }

        public void updatePlant(Plant currentPlant) {
            try
            {
                string query = String.Format(@"update plants set id='{0}', full_name='{1}', typeName='{2}' where id='{3}'", currentPlant.shortName, currentPlant.fullName, currentPlant.idType, currentPlant.oldShortName);
                DB.getInstance().executeNonQuery(query);
            }
            catch (Exception ex) {
                MessageBox.Show("Помилка зміни даних! Введено некоректні дані. \n" + ex.Message);
            }
        }

        public void deletePlant(Plant tmp) {
            DB.getInstance().executeNonQuery(String.Format("delete from plants where id='{0}'", tmp.shortName));
        }

        public IDataReader getTypesOfPlants() {
            return DB.getInstance().execute(String.Format("select distinct id from types_of_plants"));
        }

        public void deleteStudy(int idStudy) {
            DB.getInstance().executeNonQuery(String.Format("delete from study where id='{0}'", idStudy));
        }

        internal bool checkPlantType(string typeName)
        {
            IDataReader tmp=DB.getInstance().execute(String.Format("select * from types_of_plants where id='{0}'", typeName));
            if (tmp.Read()) {
                return true;
            }
            return false;
        }

        internal void addPlantType(string typeName, string caption)
        {
            try
            {
                DB.getInstance().executeNonQuery(String.Format("insert into types_of_plants (id,caption) values ('{0}','{1}')", typeName, caption));
            }
            catch (Exception ex) {
                MessageBox.Show("Помилка додавання! Введено некоректні дані. \n" + ex.Message);
            }
        }

        public bool checkPlant(string shortName) {
            IDataReader tmp = DB.getInstance().execute(String.Format("select * from plants where id='{0}'", shortName));
            if (tmp.Read()) {
                return true;
            }
            return false;
        }

        public void addPlant(string shortName,string fullName, string idType) {
            try
            {
                DB.getInstance().executeNonQuery(String.Format("insert into plants (id, full_name,typeName) values ('{0}','{1}','{2}')", shortName, fullName, idType));
            }
            catch (Exception ex) {
                MessageBox.Show("Помилка додавання! Введено некоректні дані. \n" + ex.Message);
            }
        }
    }
}
