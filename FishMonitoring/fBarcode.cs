using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;


namespace WpfPosApp
{
    public partial class fBarcode : Form
    {
        MyConnection db = new MyConnection();

        public fBarcode()
        {
            InitializeComponent();
        }

        private void FBarcode_Load(object sender, EventArgs e)
        {
            productEntities db = new productEntities();
            var list = db.Product.Select(p => new { ProdID = p.ProdID, Barcode = p.Barcode, Model = p.Model });
            rptBarcode1.SetDataSource(list);
            crystalReportViewer1.BackColor = SystemColors.Window;
            crystalReportViewer1.BorderStyle = BorderStyle.None;
            
        }

    }
}
