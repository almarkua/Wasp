using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Front_End_WASP
{
    public class Study
    {
        private int _id;
        private string _name;
        private DateTime _date;

        public string date
        {
            get
            {
                return _date.ToString("dd-MM-yyyy");
            }
        }

        public string id
        {
            get
            {
                return _id.ToString();
            }
        }

        public int intId
        {
            get
            {
                return _id;
            }
        }

        public string name
        {
            get
            {
                return _name;
            }
        }


        public Study(int id, string name, DateTime date)
        {
            this._id = id;
            this._name = name;
            this._date = date;
        }
    } 
}
