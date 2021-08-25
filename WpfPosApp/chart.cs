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
using System.Windows.Forms.DataVisualization.Charting;

namespace WpfPosApp
{
    public partial class chart : Form
    {
        public chart()
        {
            InitializeComponent();
            chartLoad();
        }

        MyConnection db = new MyConnection();
        public void chartLoad()
        {
          
            db.con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select YEAR([transaction_date ]) as YEAR,  MONTH([transaction_date]) as MONTH, isnull(sum(grandTotal), 0.00) as Total from tblTransaction where YEAR([transaction_date]) = YEAR(GETDATE()) AND type like 'Sale' group by MONTH(transaction_date), YEAR(transaction_date);", db.con);
            SqlDataAdapter daa = new SqlDataAdapter("select YEAR([transaction_date ]) as YEAR,  MONTH([transaction_date]) as MONTH, isnull(sum(grandTotal), 0.00) as Total from tblTransaction where YEAR([transaction_date]) = YEAR(GETDATE())-1 AND type like 'Sale' group by MONTH(transaction_date), YEAR(transaction_date);", db.con);
            DataSet ds = new DataSet();
            DataSet dd = new DataSet();
            

            // Chart 1

            da.Fill(ds, "Sales");
            daa.Fill(dd, "Sales");
         
            chart1.Legends.Add("Sales");
            Series series1 = chart1.Series["Series1"];
          
            chart1.Series[0].ChartType = SeriesChartType.Column;
            chart1.Series[0].Points.DataBindXY(ds.Tables[0].DefaultView, "MONTH", ds.Tables[0].DefaultView, "TOTAL");

            chart1.Series[0].ToolTip = "Data Point Y Value: #VALY{G}";

            Series series2 = chart1.Series["Series2"];
            chart1.Series[1].ChartType = SeriesChartType.Column;
            chart1.Series[1].Points.DataBindXY(dd.Tables[0].DefaultView, "MONTH", dd.Tables[0].DefaultView, "TOTAL");

            chart1.Series[1].ToolTip = "Data Point Y Value: #VALY{G}";

            series1.Name = "Monthly Sales";
            series2.Name = "Last Year Monthly Sales";

            chart1.Series[0].IsValueShownAsLabel = true;
            chart1.Series[1].IsValueShownAsLabel = true;

        }
    }
}
