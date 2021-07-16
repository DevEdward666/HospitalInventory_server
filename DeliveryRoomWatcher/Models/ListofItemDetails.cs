using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class ListofItemDetails
    {
        public string reqno { get; set; }
        public short lineno { get; set; }
        public string linestatus { get; set; }
        public string deptcode { get; set; }
        public string sectioncode { get; set; }
        public string stockcode { get; set; }
        public string stockdesc { get; set; }
        public int reqqty { get; set; }
        public string unitdesc { get; set; }
        public float averagecost { get; set; }
        public string itemtrantype { get; set; }
        public string itemremarks { get; set; }
    }
}
