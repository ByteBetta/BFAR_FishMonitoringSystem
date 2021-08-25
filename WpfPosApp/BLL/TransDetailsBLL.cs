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
        public int ProdID { get; set; }
        public string transno { get; set; }
        public decimal price { get; set; }
        public decimal qty { get; set; }
        public decimal total_price { get; set; }
        public string type { get; set; }
        public int DealCustID { get; set; }
        public DateTime added_date { get; set; }

    }
}
