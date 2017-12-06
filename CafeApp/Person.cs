using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeApplication
{  
    public class Person
    {
        public Person(string name, string lastname)
        {
            Name = name;
            LastName = lastname;
        }
        public Person()
        {
        }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
