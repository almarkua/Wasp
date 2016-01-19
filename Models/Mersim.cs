using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Front_End_WASP
{
    class Mersim
    {
        private string file;
        public  List<double> totalCapacity, peakLoad, minimumLoad, maintenanceSpace, reserveCapacity, totalGeneration, energyDemand, unservedEnergy, energyBalance, lossOfLoadProbability, energyPumped,probality;
        public List<int> period, year, hydrocondition;
        public  List<DataTable> thermalData, hydroData;

        public Mersim(string tmpFile)
        {
            totalCapacity = new List<double>();
            peakLoad = new List<double>();
            minimumLoad = new List<double>();
            maintenanceSpace = new List<double>();
            reserveCapacity = new List<double>();
            totalGeneration = new List<double>();
            energyDemand = new List<double>();
            unservedEnergy = new List<double>();
            energyBalance = new List<double>();
            lossOfLoadProbability = new List<double>();
            energyPumped = new List<double>();
            period = new List<int>();
            year = new List<int>();
            hydrocondition = new List<int>();
            probality = new List<double>();
            thermalData = new List<DataTable>();
            hydroData = new List<DataTable>();
            file = tmpFile;
        }

        public Mersim(IDataReader totalData,IDataReader thermalDataReader,IDataReader hydroDataReader) {
            totalCapacity = new List<double>();
            peakLoad = new List<double>();
            minimumLoad = new List<double>();
            maintenanceSpace = new List<double>();
            reserveCapacity = new List<double>();
            totalGeneration = new List<double>();
            energyDemand = new List<double>();
            unservedEnergy = new List<double>();
            energyBalance = new List<double>();
            lossOfLoadProbability = new List<double>();
            energyPumped = new List<double>();
            period = new List<int>();
            year = new List<int>();
            hydrocondition = new List<int>();
            probality = new List<double>();
            thermalData = new List<DataTable>();
            hydroData = new List<DataTable>();
            
            totalData.Read();
            totalCapacity.Add(Convert.ToDouble(totalData.GetValue(0)));
            peakLoad.Add(Convert.ToDouble(totalData.GetValue(1)));
            minimumLoad.Add(Convert.ToDouble(totalData.GetValue(2)));
            maintenanceSpace.Add(Convert.ToDouble(totalData.GetValue(3)));
            reserveCapacity.Add(Convert.ToDouble(totalData.GetValue(4)));
            totalGeneration.Add(Convert.ToDouble(totalData.GetValue(5)));
            energyDemand.Add(Convert.ToDouble(totalData.GetValue(6)));
            unservedEnergy.Add(Convert.ToDouble(totalData.GetValue(7)));
            energyBalance.Add(Convert.ToDouble(totalData.GetValue(8)));
            lossOfLoadProbability.Add(Convert.ToDouble(totalData.GetValue(9)));
            energyPumped.Add(Convert.ToDouble(totalData.GetValue(10)));
            
            DataTable thermalTable=createThermalTable();
            DataTable hydroTable=createHydroTable();
            while (thermalDataReader.Read()) {
                thermalTable.Rows.Add(thermalDataReader.GetValue(0),
                    thermalDataReader.GetValue(1),
                    thermalDataReader.GetValue(2),
                    thermalDataReader.GetValue(3),
                    thermalDataReader.GetValue(4),
                    thermalDataReader.GetValue(5),
                    thermalDataReader.GetValue(6),
                    thermalDataReader.GetValue(7),
                    thermalDataReader.GetValue(8),
                    thermalDataReader.GetValue(9),
                    thermalDataReader.GetValue(10),
                    thermalDataReader.GetValue(11),
                    thermalDataReader.GetValue(12),
                    thermalDataReader.GetValue(13),
                    thermalDataReader.GetValue(14));
            }
            while (hydroDataReader.Read()) {
                hydroTable.Rows.Add(hydroDataReader.GetValue(0),
                    hydroDataReader.GetValue(1),
                    hydroDataReader.GetValue(2),
                    hydroDataReader.GetValue(3),
                    hydroDataReader.GetValue(4),
                    hydroDataReader.GetValue(5),
                    hydroDataReader.GetValue(6),
                    hydroDataReader.GetValue(7),
                    hydroDataReader.GetValue(8),
                    hydroDataReader.GetValue(9),
                    hydroDataReader.GetValue(10),
                    hydroDataReader.GetValue(11),
                    hydroDataReader.GetValue(12),
                    hydroDataReader.GetValue(13),
                    hydroDataReader.GetValue(14));
            }
            thermalData.Add(thermalTable);
            hydroData.Add(hydroTable);
        }

        public void readFile()
        {
            List<string> fileLines = File.ReadAllLines(file).ToList<string>();
            for (int i = 0; i < fileLines.Count; i++)
            {
                if (fileLines[i].Trim().IndexOf("HYDROPLANTS OPERATIONAL SUMMARY") >= 0)
                {
                    DataTable hydroTable = createHydroTable();
                    DataTable thermalTable = createThermalTable();
                    
                    string[] tmpHeaderData = deleteEmpty(fileLines[i-2].Trim().Split(' '));
                    period.Add(Convert.ToInt32(tmpHeaderData[1].Replace('.', ',')));
                    year.Add(Convert.ToInt32(tmpHeaderData[4].Replace('.', ',')));
                    tmpHeaderData = deleteEmpty(fileLines[i-1].Trim().Split(' '));
                    hydrocondition.Add(Convert.ToInt32(tmpHeaderData[1].Replace('.', ',')));
                    probality.Add(Convert.ToDouble(tmpHeaderData[3].Replace('.', ',')));

                    i += 5;
                    for (int j = i; j < i + 2; j++)
                    {
                        string[] tmp = deleteEmpty(fileLines[j].Trim().Split(' '));
                        double[] tmpDouble=new double[tmp.Length];
                        for (int k = 1; k < tmp.Length; k++)
                        {
                            Double.TryParse(tmp[k], out tmpDouble[k]);
                        }
                        hydroTable.Rows.Add(tmp[1], tmpDouble[2], tmpDouble[3], tmpDouble[4], tmpDouble[5], tmpDouble[6],
                            tmpDouble[7], tmpDouble[8], tmpDouble[9], tmpDouble[10], tmpDouble[11], tmpDouble[12],
                            tmpDouble[13], tmpDouble[14], tmpDouble[15]);
                    }

                    i += 8;
                    while (fileLines[i].Trim().IndexOf("TOTALS") < 0)
                    {
                        string[] tmp = deleteEmpty(fileLines[i].Trim().Split(' '));
                        thermalTable.Rows.Add(tmp[1],
                            Convert.ToDouble(tmp[2].Replace('.', ',')),
                            Convert.ToDouble(tmp[3].Replace('.', ',')),
                            Convert.ToDouble(tmp[4].Replace('.', ',')),
                            Convert.ToDouble(tmp[5].Replace('.', ',')),
                            Convert.ToDouble(tmp[6].Replace('.', ',')),
                            Convert.ToDouble(tmp[7].Replace('.', ',')),
                            Convert.ToDouble(tmp[8].Replace('.', ',')),
                            Convert.ToDouble(tmp[9].Replace('.', ',')),
                            Convert.ToDouble(tmp[10].Replace('.', ',')),
                            Convert.ToDouble(tmp[11].Replace('.', ',')),
                            Convert.ToDouble(tmp[12].Replace('.', ',')),
                            Convert.ToDouble(tmp[13].Replace('.', ',')),
                            Convert.ToDouble(tmp[14].Replace('.', ',')),
                            Convert.ToDouble(tmp[15].Replace('.', ',')));
                        i++;
                    }

                    i += 17;
                    string[] tmpTotals = deleteEmpty(fileLines[i].Trim().Split(' '));
                    totalCapacity.Add(Convert.ToDouble(tmpTotals[3].Replace('.', ',')));
                    totalGeneration.Add(Convert.ToDouble(tmpTotals[7].Replace('.', ',')));

                    i++;
                    tmpTotals = deleteEmpty(fileLines[i].Trim().Split(' '));
                    peakLoad.Add(Convert.ToDouble(tmpTotals[3].Replace('.', ',')));
                    energyDemand.Add(Convert.ToDouble(tmpTotals[7].Replace('.', ',')));

                    i++;
                    tmpTotals = deleteEmpty(fileLines[i].Trim().Split(' '));
                    minimumLoad.Add(Convert.ToDouble(tmpTotals[3].Replace('.', ',')));
                    unservedEnergy.Add(Convert.ToDouble(tmpTotals[7].Replace('.', ',')));

                    i++;
                    tmpTotals = deleteEmpty(fileLines[i].Trim().Split(' '));
                    maintenanceSpace.Add(Convert.ToDouble(tmpTotals[3].Replace('.', ',')));
                    energyBalance.Add(Convert.ToDouble(tmpTotals[7].Replace('.', ',')));

                    i++;
                    tmpTotals = deleteEmpty(fileLines[i].Trim().Split(' '));
                    reserveCapacity.Add(Convert.ToDouble(tmpTotals[3].Replace('.', ',')));
                    lossOfLoadProbability.Add(Convert.ToDouble(tmpTotals[7].Replace('.', ',')));

                    i++;
                    tmpTotals = deleteEmpty(fileLines[i].Trim().Split(' '));
                    energyPumped.Add(Convert.ToDouble(tmpTotals[3].Replace('.', ',')));

                    thermalData.Add(thermalTable);
                    hydroData.Add(hydroTable);
                }
            }
        }

        private string[] deleteEmpty(string[] tmp)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i] != "")
                {
                    result.Add(tmp[i]);
                }
            }
            return result.ToArray();

        }

        private DataTable createHydroTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("number_of_units", typeof(double));
            table.Columns.Add("lord_pos_pl", typeof(double));
            table.Columns.Add("u", typeof(double));
            table.Columns.Add("base_capacity", typeof(double));
            table.Columns.Add("peak_capacity", typeof(double));
            table.Columns.Add("total_capacity", typeof(double));
            table.Columns.Add("base_energy", typeof(double));
            table.Columns.Add("peak_energy", typeof(double));
            table.Columns.Add("total_energy", typeof(double));
            table.Columns.Add("peak_mineng", typeof(double));
            table.Columns.Add("energe_splliled", typeof(double));
            table.Columns.Add("energy_shortage", typeof(double));
            table.Columns.Add("o&m_local", typeof(double));
            table.Columns.Add("capacity_factor", typeof(double));
            return table;
        }

        private DataTable createThermalTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("plant_name", typeof(string));
            table.Columns.Add("number_of_units", typeof(double));
            table.Columns.Add("unit_base", typeof(double));
            table.Columns.Add("total_capacity", typeof(double));
            table.Columns.Add("plant_capacity", typeof(double));
            table.Columns.Add("base_energy", typeof(double));
            table.Columns.Add("peak_energy", typeof(double));
            table.Columns.Add("total_energy", typeof(double));
            table.Columns.Add("fuel_domestic", typeof(double));
            table.Columns.Add("fuel_foreign", typeof(double));
            table.Columns.Add("fuel_total", typeof(double));
            table.Columns.Add("o&m_domestic", typeof(double));
            table.Columns.Add("main_probality", typeof(double));
            table.Columns.Add("for", typeof(double));
            table.Columns.Add("capacity_factor", typeof(double));
            return table;
        }
    }
}
