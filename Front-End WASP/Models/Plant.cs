using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Front_End_WASP
{
    public class Plant
    {
        private string _shortName,_oldShortName;
        private string _fullName;
        private string _idType;

        public string shortName
        {
            get
            {
                return _shortName;
            }
            set
            {
                if (value.Length > 0)
                {
                    if (shortName != null)
                    {
                        oldShortName = _shortName;
                        _shortName = value;
                    }
                    else
                    {
                        oldShortName = value;
                        _shortName = value;
                    }
                }
            }
        }
        public string oldShortName {
            get {
                return _oldShortName;
            }
            set {
                if (value.Length > 0) {
                    _oldShortName = value;
                }
            }
        }

        public string fullName {
            get {
                return _fullName;
            }
            set {
                if (value.Length > 0) _fullName = value;
            }
        }

        public string idType {
            get {
                return _idType;
            }
            set {
                if (_idType == null)
                {
                    _idType = value;
                }
                else {
                    _idType = value;
                }
            }

        }

        public Plant(string shortName, string fullName, string idType) {
            this._shortName = shortName;
            this._oldShortName = shortName;
            this._fullName = fullName;
            this._idType = idType;
        }

        public Plant() { }
    }
}
