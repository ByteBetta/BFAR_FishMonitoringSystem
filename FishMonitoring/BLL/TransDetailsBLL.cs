using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL
{
  
    class TransDetailsBLL
    {
        public string UID { get; set; }
        public int FishID { get; set; }
        public string weight { get; set; }
        public decimal length { get; set; }
        public string transno { get; set; }
        public string Species { get; set; }
        public int DealID { get; set; }
        public int quantity { get; set; }
        public DateTime added_date { get; set; }
        public string landingSite { get; set; }
        public string gearUsed { get; set; }
        public string vessels { get; set; }
        public string fisherman { get; set; }
     

    }
}
