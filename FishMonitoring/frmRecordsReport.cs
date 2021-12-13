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
    public partial class frmRecordsReport : Form
    {
        public SqlConnection con;
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        MyConnection dbcon = new MyConnection();
        SqlDataReader dr;

        public frmRecordsReport()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyCon());
        }

        public void LoadTopSelling(string sql, string param, string header)
         {
            try
            {

                ReportDataSource rptDS;

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rptTop.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand(sql, cn);
                da.Fill(ds.Tables["dtTopSelling"]);
                cn.Close();

                ReportParameter pHeader = new ReportParameter("pHeader", header);
                ReportParameter pDate = new ReportParameter("pDate", param);
                reportViewer1.LocalReport.SetParameters(pDate);
                reportViewer1.LocalReport.SetParameters(pHeader);


                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtTopSelling"]);
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

        public void LoadSoldItems(string sql, string param)
        {
            try
            {

                ReportDataSource rptDS;

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rptSold.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand(sql, cn);
                da.Fill(ds.Tables["dtSoldItems"]);
                cn.Close();

                ReportParameter pDate = new ReportParameter("pDate", param);
                reportViewer1.LocalReport.SetParameters(pDate);

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtSoldItems"]);
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

        public void reportmonitoring(string month, string year, string speciesadapter, string vessel, string fisherman, string landingsite, string gear, string SqlGenerate, string user)
        {
            {
                try
                {
                    ReportDataSource rptDS;

                    reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\Report5.rdlc";
                    ReportParameterCollection reportParameters = new ReportParameterCollection();
                    reportParameters.Add(new ReportParameter("Month", month));
                    reportParameters.Add(new ReportParameter("Species", speciesadapter));
                    reportParameters.Add(new ReportParameter("LandingSite", landingsite));
                    reportParameters.Add(new ReportParameter("Gear", gear));
                    reportParameters.Add(new ReportParameter("Fisherman", fisherman));
                    reportParameters.Add(new ReportParameter("Vessels", vessel));
                    reportParameters.Add(new ReportParameter("Year", year));
                    reportParameters.Add(new ReportParameter("USER", user));
                  
                    this.reportViewer1.LocalReport.DataSources.Clear();

                    DataSet1 ds = new DataSet1();
                    SqlDataAdapter da = new SqlDataAdapter();


                  
                  


                    cn.Open();
                    da.SelectCommand = new SqlCommand(SqlGenerate, cn);
                    da.Fill(ds.Tables["ChartReport"]);
                    cn.Close();


                    

                    rptDS = new ReportDataSource("DataSet1", ds.Tables["ChartReport"]);
                    reportViewer1.LocalReport.DataSources.Add(rptDS);



                    reportViewer1.LocalReport.SetParameters(reportParameters);
                    reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                    reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;
       
                    var setup = reportViewer1.GetPageSettings();
                    setup.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
                    reportViewer1.SetPageSettings(setup);

                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    cn.Close();
                }


            }


        }

        public void reportmonitoring2(string year, string speciesadapter, string vessel, string fisherman, string landingsite, string gear, string SqlGenerate, string user, string type)
        {
            {
                try
                {
                    ReportDataSource rptDS;

                    reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\Report6.rdlc";
                    ReportParameterCollection reportParameters = new ReportParameterCollection();
                    reportParameters.Add(new ReportParameter("Species", speciesadapter));
                    reportParameters.Add(new ReportParameter("LandingSite", landingsite));
                    reportParameters.Add(new ReportParameter("Gear", gear));
                    reportParameters.Add(new ReportParameter("Fisherman", fisherman));
                    reportParameters.Add(new ReportParameter("Vessels", vessel));
                    reportParameters.Add(new ReportParameter("Year", year));
                    reportParameters.Add(new ReportParameter("USER", user));
                    reportParameters.Add(new ReportParameter("Type", type));
                    this.reportViewer1.LocalReport.DataSources.Clear();

                    DataSet1 ds = new DataSet1();
                    SqlDataAdapter da = new SqlDataAdapter();
                    
                    cn.Open();
                    da.SelectCommand = new SqlCommand(SqlGenerate, cn);
                    da.Fill(ds.Tables["ChartReportMonthly"]);
                    cn.Close();




                    rptDS = new ReportDataSource("DataSet1", ds.Tables["ChartReportMonthly"]);
                    reportViewer1.LocalReport.DataSources.Add(rptDS);



                    reportViewer1.LocalReport.SetParameters(reportParameters);
                    reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                    reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;

                    var setup = reportViewer1.GetPageSettings();
                    setup.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
                    reportViewer1.SetPageSettings(setup);



                    cn.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    cn.Close();
                }


            }


        }


        public void reportmonitoring3(string year, string month, string nbs, string nfm, string wfm, string species, string user, string sqlgenerate, string lsite)
        {
            {
                try
                {
                    ReportDataSource rptDS;

                    reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\Report7.rdlc";
                    ReportParameterCollection reportParameters = new ReportParameterCollection();
                    reportParameters.Add(new ReportParameter("Species", species));
                    reportParameters.Add(new ReportParameter("Month", month));
                    reportParameters.Add(new ReportParameter("LandingSite", lsite));
                    reportParameters.Add(new ReportParameter("nbs", nbs));
                    reportParameters.Add(new ReportParameter("nfm", nfm));
                    reportParameters.Add(new ReportParameter("wfm", wfm));
                    reportParameters.Add(new ReportParameter("Year", year));
                    reportParameters.Add(new ReportParameter("USER", user));

                    this.reportViewer1.LocalReport.DataSources.Clear();

                    DataSet1 ds = new DataSet1();
                    SqlDataAdapter da = new SqlDataAdapter();

                    cn.Open();
                    da.SelectCommand = new SqlCommand(sqlgenerate, cn);
                    da.Fill(ds.Tables["accomplishmentReport"]);
                    cn.Close();




                    rptDS = new ReportDataSource("DataSet1", ds.Tables["accomplishmentReport"]);
                    reportViewer1.LocalReport.DataSources.Add(rptDS);



                    reportViewer1.LocalReport.SetParameters(reportParameters);
                    reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                    reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;

                    var setup = reportViewer1.GetPageSettings();
                    setup.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
                    reportViewer1.SetPageSettings(setup);



                    cn.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    cn.Close();
                }


            }


        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
