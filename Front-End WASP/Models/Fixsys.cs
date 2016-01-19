using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Front_End_WASP
{
    class Fixsys
    {
        string file;

        public Fixsys(string file) {
            this.file = file;
        }

        public  List<string> getPlants (){ 
            List<string> result=new List<string>();
            string[] allFileLines = File.ReadAllLines(file);
            bool start = false;
            for (int i = 0; i < allFileLines.Length; i++) {
                if (allFileLines[i].IndexOf("NGROUPLM IPNLT PNLTLOLP PNLTENS") >= 0) {
                    break;
                }
                if (start) {
                    result.Add(allFileLines[i].Trim().Split(' ')[0]);
                    continue;
                }
                if (allFileLines[i].IndexOf("NAME SETS  MW     MW") >= 0)
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
