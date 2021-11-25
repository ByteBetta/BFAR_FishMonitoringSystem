using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fish.BLL
{
    class FishBLL
    {
        public int FishID { get; set; }
        public string Species { get; set; }
        public string ShortDescription { get; set; }
        public string Biology { get; set; }
        public string Measurement { get; set; }
        public string OrderName { get; set; }
        public string FamilyName { get; set; }
        public string LocalName { get; set; }
        public string Distribution { get; set; }
        public string Environment { get; set; }
        public string FishBaseName { get; set; }
        public string Occurance { get; set; }
        public string Img { get; set; }
        public DateTime added_time { get; set; }
        public int added_by { get; set; }

    }
}
