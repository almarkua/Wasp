using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Front_End_WASP
{
    class Varsys
    {
        string file;

        public Varsys(string file) {
            this.file = file;
        }

        public  List<string> getPlants (){ 
            List<string> result=new List<string>();
            string[] allFileLines = File.ReadAllLines(file);
            bool start = false;
            for (int i = 0; i < allFileLines.Length; i++) {
                if (allFileLines[i].IndexOf("NGROUPLM   EMISNAME    MEASIND") >= 0)
                {
                    break;
                }
                if (start) {
                    result.Add(allFileLines[i].Trim().Split(' ')[0]);
                    continue;
                }
                if (allFileLines[i].IndexOf("NAME SETS  MW     MW     KCAL/ KWH") >= 0)
                {
                    start = true;
                    i++;
                    continue;
                }
            }
            return result;
        }
    }
}
