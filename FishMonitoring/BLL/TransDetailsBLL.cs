using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL
{
    class TransDetailsBLL
    {
        public int TransDetID { get; set; }
        public int FishID { get; set; }
        public string FishWeight { get; set; }
        public decimal FishQuantity { get; set; }
        public string transno { get; set; }
        public int DealID { get; set; }
        public DateTime added_date { get; set; }

    }
}
