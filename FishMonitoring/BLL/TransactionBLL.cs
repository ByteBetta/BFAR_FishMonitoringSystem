using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL
{
    class TransactionBLL
    {
        public string transno { get; set; }
        public string fisherman { get; set; }
        public DateTime transactiondate { get; set; }
        public string remark { get; set; }
        public string vessels { get; set; }
        public string gearUsed { get; set; }
        public string landingSite { get; set; }
        public decimal totalBox { get; set; }
        public decimal totalSampleBox { get; set; }
        public decimal totalWeightBox { get; set; }
        public decimal totalSampleWeightBox { get; set; }
        public int added_by { get; set; }
    }
}
