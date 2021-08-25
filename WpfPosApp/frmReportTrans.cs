using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;

namespace WpfPosApp
{
    public partial class frmReportTrans : Form
    {
        public SqlConnection con;
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        MyConnection dbcon = new MyConnection();
        SqlDataReader dr;
        frmTransactions f;
        frmRecords r;

        string store = "PekrxProd Shop";
        string address = "Tbilisi, Georgia";

        public frmReportTrans(frmTransactions frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyCon());
            f = frm;
        }

        private void FrmReportTrans_Load(object sender, EventArgs e)
        {

        }

       

        public void LoadReport()
        {
            try
            {
                string s1 = f.dtpPicker1.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string s2 = f.dtpPicker2.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

                ReportDataSource rptDS;

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\Report2.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("SELECT id, type, DealCustID, grandTotal, transaction_date, tax, discount FROM tblTransaction WHERE transaction_date between '" + s1 + "' and '" + s2 + "'", cn);
                da.Fill(ds.Tables["dtTransaction"]);
                cn.Close();

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtTransaction"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

               

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }


        public void LoadReport2()
        {
            try
            {
               
                ReportDataSource rptDS;

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\Report2.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("SELECT id, type, DealCustID, grandTotal, transaction_date, tax, discount FROM tblTransaction", cn);
                da.Fill(ds.Tables["dtTransaction"]);
                cn.Close();

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtTransaction"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

             

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
