using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsSoupApp.Model
{
    public class Facility
    {
        public string _name { get; set; }
        public Facility(string name)
        {
            _name = name;
        }

        public override string ToString() => _name;
    }
}
