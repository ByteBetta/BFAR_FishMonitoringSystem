using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPosApp.BLL
{
    class DealersBLL
    {
        public int DealID { get; set; }
        public string name { get; set; }
        public string person { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
        public string address { get; set; }
        public DateTime added_date { get; set; }
        public int added_by { get; set; }
    }
}
