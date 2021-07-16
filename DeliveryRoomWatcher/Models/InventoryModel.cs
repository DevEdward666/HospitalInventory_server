using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class InventoryModel
    {
        public class listofrequest
        {
            public string id { get; set; }
        }  
        public class listofrequestseareched
        {
            public string dept { get; set; }
            public string status { get; set; }
            public string date { get; set; }
        }
        public class listofrequestbydept
        {
            public string id { get; set; }
            public string status { get; set; }
        }
        public class searchitems
        {
            public string search { get; set; }
            public string classcode { get; set; }
        }
    }
}
