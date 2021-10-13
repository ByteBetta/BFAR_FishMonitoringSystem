using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPosApp.BLL
{
    class VesselBLL
    {
        public int VesselID { get; set; }
        public string VesselName { get; set; }
        public string VesselCode { get; set; }
        public string VesselOwner { get; set; }
        public DateTime added_date { get; set; }
        public int added_by { get; set; }
        public int DealID { get; set; }
    }
}
