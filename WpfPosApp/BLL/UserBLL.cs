using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPosApp.BLL
{
    class UserBLL
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string SEX { get; set; }
        public string Birth_Date { get; set; }
        public string Img { get; set; }
        public DateTime Added_Date { get; set; }
        public int Added_By { get; set; }

    }
}
