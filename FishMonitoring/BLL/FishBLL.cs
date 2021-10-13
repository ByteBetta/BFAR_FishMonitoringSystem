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
        public string ScientificName { get; set; }
        public int MaxLength { get; set; }
        public int CommonLength { get; set; }
        public int AnalSpine { get; set; }
        public int AnalSoftRay { get; set; }
        public int DorsalSpine { get; set; }
        public int DorsalSoftRay { get; set; }
        public string Remark { get; set; }
        public string OrderName { get; set; }
        public string FamilyName { get; set; }
        public string LocalName { get; set; }
        public string Salinity { get; set; }
        public string Img { get; set; }
        public DateTime added_time { get; set; }
        public int added_by { get; set; }
        public string Occurance { get; set; }
        public string Location { get; set; }
        public string FishName { get; set; }
    }
}
