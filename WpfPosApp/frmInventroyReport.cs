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
using Project.DAL;

namespace WpfPosApp
{
    public partial class frmInventroyReport : Form
    {
        SqlConnection cn = new SqlConnection();
        MyConnection db = new MyConnection();
        SqlCommand cm;
        SqlDataReader dr;
        frmInventory inv;


        public frmInventroyReport(frmInventory frm)
        {
            cn = new SqlConnection(db.MyCon());
            InitializeComponent();
            inv = frm;
        }

        private void FrmInventroyReport_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }


        

        public void LoadReport()
        {
            ReportDataSource rptDS;
            try
            {
                reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\Report3.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("SELECT ProdID, Manufactor, Model, Full_Name, Price, Category, Description, Quantity, Reorder, Dealer FROM Product", cn);
                da.Fill(ds.Tables["dtInventory"]);
                cn.Close();

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtInventory"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void LoadReport2()
        {
            ReportDataSource rptDS;
            try
            {
                reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\Report3.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("SELECT ProdID, Manufactor, Model, Full_Name, Price, Category, Description, Quantity, Reorder, Dealer FROM Product WHERE Category = '" + inv.cmbCategories.Text.ToString() + "'", cn);
                da.Fill(ds.Tables["dtInventory"]);
                cn.Close();

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtInventory"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void LoadCritical()
        {
            ReportDataSource rptDS;
            try
            {
                reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\Report3.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("select ProdID, Manufactor, Model, Full_Name, Price, Category, Description, Year, Warranty, Quantity, Reorder, Dealer from vwCriticalItems", cn);
                da.Fill(ds.Tables["dtInventory"]);
                cn.Close();

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtInventory"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
