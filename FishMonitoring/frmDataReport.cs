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
    public partial class frmDataReport : Form
    {
        public SqlConnection con;
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        MyConnection dbcon = new MyConnection();
        SqlDataReader dr;
        frmUser usr;
        frmDealersandCustomers dea;
        frmEmployee emp;
        frmFishDetails prd;
        frmDealers deal;
        

        public frmDataReport(frmUser frm, frmDealersandCustomers frm2, frmEmployee frm3, frmFishDetails frm4, frmDealers frm5)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyCon());
            usr = frm;
            dea = frm2;
            emp = frm3;
            prd = frm4;
            deal = frm5;
        }

        private void FrmDataReport_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        public void LoadUserReport()
        {
            try
            {

                ReportDataSource rptDS;

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rptUsers.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("SELECT UserID, Name, Surname, UserName, Password, UserType, Gender, Birth_Date FROM Login", cn);
                da.Fill(ds.Tables["dtUser"]);
                cn.Close();

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtUser"]);
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

        public void LoadDeaCustReport()
        {
            try
            {

                ReportDataSource rptDS;

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rptDealCust.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("SELECT DealCustID, type, name, email, contact, address FROM DealCust", cn);
                da.Fill(ds.Tables["dtCustomer"]);
                cn.Close();

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtCustomer"]);
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

        public void LoadDealerstReport()
        {
            try
            {

                ReportDataSource rptDS;

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rptDealers.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("SELECT DealID, name, person, email, contact, address FROM Dealers", cn);
                da.Fill(ds.Tables["dtDealer"]);
                cn.Close();

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtDealer"]);
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

        public void LoadEmployeeReport()
        {
            try
            {

                ReportDataSource rptDS;

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rptEmployee.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("SELECT EmpID, Name, Surname, Gender, Birth_Date, Age, Address, Mobile, DateOfReciving, Sallary  FROM Employee", cn);
                da.Fill(ds.Tables["dtEmployee"]);
                cn.Close();

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtEmployee"]);
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

        public void LoadProducReport()
        {
            try
            {

                ReportDataSource rptDS;

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rptProduct.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("SELECT FishID, Manufactor, Model, Full_Name, Price, Category, Description, Year, Warranty  FROM Product", cn);
                da.Fill(ds.Tables["dtProduct"]);
                cn.Close();

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtProduct"]);
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
