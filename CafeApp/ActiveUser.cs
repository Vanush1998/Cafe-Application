using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace CafeApplication
{
    public class ActiveUser : User
    {
        public GeoCoordinate Location { get; set; }
        private ActiveUser(string name, string lastname, string username, string password)
            :base(name,lastname, username, password)
        {}
    }
}
