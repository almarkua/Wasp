using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Controls;

namespace Front_End_WASP
{
    class PresenterCompare
    {
        private ICompare view;
        private List<int> selectedPlantsId;
        private DataTable thermalData, hydroData, studyData, scenarioData;
        private DataSet allData;

        public PresenterCompare(ICompare view, List<int> selectedPlants)
        {
            this.view = view;
            view.minYear = 2009;
            view.maxYear = 2014;
            this.view.refreshFilters(view.minYear, view.maxYear, getTypes());
            this.view.changeType += OnChangeType;
            this.view.changeFilters += OnChangeType;
            selectedPlantsId = selectedPlants;
            OnChangeType("Загальна генерація", EventArgs.Empty);
            fillDataTable();
        }

        private void fillDataTable()
        {
            string thermalSql =
                "select thermal_plants.* from thermal_plants,scenario where scenario.id=thermal_plants.id_scenario";
            foreach (int i in selectedPlantsId)
            {
                thermalSql += " and scenario.id_study=" + i.ToString() + " ";
            }
            //thermalData = DB.getInstance().exec(thermalSql);
            string hydroSql =
                "select hydro_plants.* from hydro_plants,scenario where scenario.id=hydro_plants.idscenario";
            foreach (int i in selectedPlantsId)
            {
                hydroSql += " and scenario.id_study=" + i.ToString() + " ";
            }
            //hydroData = DB.getInstance().exec(hydroSql);
            
            string studySql =
                "select * from study";
            /*string scenationSql = 
            //studyData = DB.getInstance().exec(studySql);
            DB.getInstance()
                .execMoreQuery(new string[] {thermalSql, hydroSql, studySql}, new string[] {"thermal", "hydro", "study"});
             * */
        }

        private DataTable getFiltereDataTable()
        {
            DataTable result=new DataTable();

            return result;
        }

        void OnChangeType(object sender, EventArgs e)
        {
            switch (view.mode) {
                case "по типах блоків":
                    {
                        onTypesCompare(sender, e);
                        break;
                    }
                case "по сценаріях":
                    {
                        onScenariesCommpare(sender, e);
                        break;
                    }   
            }   
        }

        private void onTypesCompare(object sender, EventArgs e)
        {
            string query = String.Format("select st.name as \"Назва дослідження\", sce.year as \"Рік\",typ.id as \"Тип блоків\",sum(");
            switch (sender.ToString())
            {
                case "Загальна генерація":
                    {
                        query += "th.total_energy";
                        break;
                    }
                case "Витрати (без будівництва)":
                    {
                        query += "th.fuel_total+th.om_domestic";
                        break;
                    }
                case "Потужність":
                    {
                    query += "th.plant_capacity";
                    break;
                    }
            }
            query += ") as \"Загалом\" from plants as pl, scenario as sce, study as st, thermal_plants as th, types_of_plants as typ";
            for (int i = 0; i < selectedPlantsId.Count; i++)
            {
                if (i == 0)
                {
                    query += String.Format(" where (st.id={0}", selectedPlantsId[i]);
                }
                else
                {
                    query += String.Format(" or st.id={0}", selectedPlantsId[i]);
                }
            }

            query += ") and sce.id_study=st.id and th.id_scenario=sce.id and th.plant_name=pl.id and pl.typeName=typ.id";
            query += " and sce.year>=" + view.currentMinYear.ToString() + " and sce.year<=" + view.currentMaxYear.ToString();
            List<string> unselecteedTypes = view.listCheckBoxObj.GetUnselectedItems().ToList<string>();
            for (int i = 0; i < unselecteedTypes.Count; i++)
            {
                if (i == 0)
                {
                    query += String.Format(" and (pl.typeName!='{0}'", unselecteedTypes[i]);
                }
                else
                {
                    query += String.Format(" and pl.typeName!='{0}'", unselecteedTypes[i]);
                }
            }
            if (unselecteedTypes.Count > 0)
            {
                query += ") ";
            }
            query += " group by st.name,sce.year,typ.id";


            query += "\n union all \n";

            query += String.Format("select st.name as \"Назва дослідження\", sce.year as \"Рік\",typ.id as \"Тип блоків\",sum(");
            switch (sender.ToString())
            {
                case "Загальна генерація":
                    {
                        query += "hp.total_energy";
                        break;
                    }
                case "Витрати (без будівництва)":
                    {
                        query += "hp.om_local";
                        break;
                    }
                case "Потужність":
                    {
                        query += "hp.total_capacity";
                        break;
                    }
            }
            query += ") as \"Загалом\" from plants as pl, scenario as sce, study as st, hydro_plants as hp, types_of_plants as typ";
            for (int i = 0; i < selectedPlantsId.Count; i++)
            {
                if (i == 0)
                {
                    query += String.Format(" where (st.id={0}", selectedPlantsId[i]);
                }
                else
                {
                    query += String.Format(" or st.id={0}", selectedPlantsId[i]);
                }
            }
            query += ") and sce.id_study=st.id and hp.idscenario=sce.id and hp.name=pl.id and pl.typeName=typ.id";
            query += " and sce.year>=" + view.currentMinYear.ToString() + " and sce.year<=" + view.currentMaxYear.ToString();
            unselecteedTypes = view.listCheckBoxObj.GetUnselectedItems().ToList<string>();
            for (int i = 0; i < unselecteedTypes.Count; i++)
            {
                if (i == 0)
                {
                    query += String.Format(" and (pl.typeName!='{0}'", unselecteedTypes[i]);
                }
                else
                {
                    query += String.Format(" and pl.typeName!='{0}'", unselecteedTypes[i]);
                }
            }
            if (unselecteedTypes.Count > 0)
            {
                query += ") ";
            }
            query += " group by st.name,sce.year,typ.id";
            query += " order by sce.year ASC, typ.id ASC, st.name ASC";
            view.updateDataTable(DB.getInstance().exec(query));
        }

        private void onScenariesCommpare(object sender, EventArgs e) {
            string query = String.Format("select st.name as name, sce.year as year,sum(");
            switch (sender.ToString())
            {
                case "Загальна генерація":
                    {
                        query += "th.total_energy";
                        break;
                    }
                case "Витрати (без будівництва)":
                    {
                        query += "th.fuel_total+th.om_domestic";
                        break;
                    }
                case "Потужність":
                    {
                        query += "th.plant_capacity";
                        break;
                    }
            }
            query += ") as total from plants as pl, scenario as sce, study as st, thermal_plants as th, types_of_plants as typ";
            for (int i = 0; i < selectedPlantsId.Count; i++)
            {
                if (i == 0)
                {
                    query += String.Format(" where (st.id={0}", selectedPlantsId[i]);
                }
                else
                {
                    query += String.Format(" or st.id={0}", selectedPlantsId[i]);
                }
            }

            query += ") and sce.id_study=st.id and th.id_scenario=sce.id and th.plant_name=pl.id and pl.typeName=typ.id";
            query += " and sce.year>=" + view.currentMinYear.ToString() + " and sce.year<=" + view.currentMaxYear.ToString();
            List<string> unselecteedTypes = view.listCheckBoxObj.GetUnselectedItems().ToList<string>();
            for (int i = 0; i < unselecteedTypes.Count; i++)
            {
                if (i == 0)
                {
                    query += String.Format(" and (typ.id!='{0}'", unselecteedTypes[i]);
                }
                else
                {
                    query += String.Format(" and typ.id!='{0}'", unselecteedTypes[i]);
                }
            }
            if (unselecteedTypes.Count > 0)
            {
                query += ") ";
            }
            query += " group by st.name,sce.year,st.id";


            query += "\n union all \n";

            query += String.Format("select st.name as name, sce.year as year,sum(");
            switch (sender.ToString())
            {
                case "Загальна генерація":
                    {
                        query += "hp.total_energy";
                        break;
                    }
                case "Витрати (без будівництва)":
                    {
                        query += "hp.om_local";
                        break;
                    }
                case "Потужність":
                    {
                        query += "hp.total_capacity";
                        break;
                    }
            }
            query += ") as total from plants as pl, scenario as sce, study as st, hydro_plants as hp, types_of_plants as typ";
            for (int i = 0; i < selectedPlantsId.Count; i++)
            {
                if (i == 0)
                {
                    query += String.Format(" where (st.id={0}", selectedPlantsId[i]);
                }
                else
                {
                    query += String.Format(" or st.id={0}", selectedPlantsId[i]);
                }
            }
            query += ") and sce.id_study=st.id and hp.idscenario=sce.id and hp.name=pl.id and pl.typeName=typ.id";
            query += " and sce.year>=" + view.currentMinYear.ToString() + " and sce.year<=" + view.currentMaxYear.ToString();
            unselecteedTypes = view.listCheckBoxObj.GetUnselectedItems().ToList<string>();
            for (int i = 0; i < unselecteedTypes.Count; i++)
            {
                if (i == 0)
                {
                    query += String.Format(" and (typ.id!='{0}'", unselecteedTypes[i]);
                }
                else
                {
                    query += String.Format(" and typ.id!='{0}'", unselecteedTypes[i]);
                }
            }
            if (unselecteedTypes.Count > 0)
            {
                query += ") ";
            }
            query += " group by st.name,sce.year,st.id";

            string subQuery = "select nt.name as \"Назва дослідження\", nt.year as \"Рік\",sum(nt.total) as \"Загалом\" from ( "+query;
            subQuery += " ) as nt group by nt.name, nt.year";
            subQuery += "\n order by nt.year ASC, nt.name ASC";
            view.updateDataTable(DB.getInstance().exec(subQuery));
        }

        private List<string> getTypes() {
            List<string> result = new List<string>();
            Model model = new Model();
            IDataReader reader=model.getTypesOfPlants();
            while (reader.Read()) {
                result.Add(reader.GetString(0));
            }
            return result;
        }

    }
}
