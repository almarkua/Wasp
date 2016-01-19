using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Front_End_WASP
{
    class PresenterAddPlants {
        private bool isVarsys = false;
        private IAddPlants view;
        private List<string> plantsToAdding;
        
        
        public PresenterAddPlants(IAddPlants view) {
            this.view = view;
            view.fileIsSelected += OnSelectFile;
        }

        public void OnSelectFile(object sender, EventArgs e) {
            if (view.file.ToLower().IndexOf("varsys") >= 0)
            {
                isVarsys = true;
            }
            if (!isVarsys)
            {
                Fixsys fixsysModel = new Fixsys(view.file);
                plantsToAdding = fixsysModel.getPlants();
            }
            else {
                Varsys varsysModel = new Varsys(view.file);
                plantsToAdding = varsysModel.getPlants();
            }
            plantsToAdding.Add("HYDR");
            plantsToAdding.Add("PUMP");
            if (plantsToAdding != null && plantsToAdding.Count > 0) {
                Model model = new Model();
                if (!model.checkPlantType("Невизначений тип"))
                {
                    model.addPlantType("Невизначений тип","Невизначений тип");
                }
                foreach (string plantShortName in plantsToAdding) {
                    if (!model.checkPlant(plantShortName)) {
                        model.addPlant(plantShortName, plantShortName, "Невизначений тип");
                        view.countOfAddedPlants++;
                    }
                }
            }

        }


    }
}
