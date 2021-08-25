using Microsoft.Reporting.WinForms;
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

namespace WpfPosApp
{
    public partial class frmRec : Form
    {
        public SqlConnection con;
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        MyConnection dbcon = new MyConnection();
        SqlDataReader dr;
        frmPurchase purchase;
        string store = "PekrxProd Shop";
        string address = "Tbilisi, Georgia";

        public frmRec(frmPurchase frm2)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyCon());
            purchase = frm2;
        }

        private void frmRec_Load(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("select c.TransDetID, c.ProdID, c.price, c.qty, c.total_price, c.added_date, p.Full_Name from TransDetails as c inner join Product as p on p.ProdID = c.ProdID where transno like '" + purchase.lblTransNoUnit.Content + "'", cn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                
                ReportDataSource rds = new ReportDataSource("DataSet1", dt);
                reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\Report1.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.RefreshReport();
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

                ReportParameter pVat = new ReportParameter("pVat", purchase.txtVAT.Text);
                ReportParameter pDiscount = new ReportParameter("pDiscount", purchase.txtDiscount.Text);
                ReportParameter pTotal = new ReportParameter("pTotal", purchase.txtGrandTotal.Text);
                ReportParameter pCash = new ReportParameter("pCash", purchase.txtPaidAmount.Text);
                ReportParameter pChange = new ReportParameter("pChange", purchase.txtReturnAmount.Text);
                ReportParameter pStore = new ReportParameter("pStore", store);
                ReportParameter pAddress = new ReportParameter("pAddress", address);
                ReportParameter pTransaction = new ReportParameter("pTransaction", "Invoice #: " + purchase.lblTransNoUnit.Content.ToString());


                reportViewer1.LocalReport.SetParameters(pVat);
                reportViewer1.LocalReport.SetParameters(pDiscount);
                reportViewer1.LocalReport.SetParameters(pTotal);
                reportViewer1.LocalReport.SetParameters(pCash);
                reportViewer1.LocalReport.SetParameters(pChange);
                reportViewer1.LocalReport.SetParameters(pStore);
                reportViewer1.LocalReport.SetParameters(pAddress);
                reportViewer1.LocalReport.SetParameters(pTransaction);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.reportViewer1.RefreshReport();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
