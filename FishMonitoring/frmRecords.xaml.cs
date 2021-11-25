using Fish.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for frmRecords.xaml
    /// </summary>
    public partial class frmRecords : UserControl
    {
        SqlConnection cn = new SqlConnection();
        MyConnection db = new MyConnection();
        SqlCommand cm;
        SqlDataReader dr;
        frmTransactions f;

        FishDAL fdal = new FishDAL();

        public frmRecords()
        {
            InitializeComponent();
            cn = new SqlConnection(db.MyCon());
            dtpPicker1.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            dtpPicker2.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            dtpPicker1sd.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            dtpPicker2sd.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            dtpPicker1.SelectedDate = DateTime.Now;
            dtpPicker2.SelectedDate = DateTime.Now.AddDays(+1);
            dtpPicker1sd.SelectedDate = DateTime.Now;
            dtpPicker2sd.SelectedDate = DateTime.Now.AddDays(+1);


            DataTable fdd = fdal.Select();
     
            //Specify DataSource for Category ComboBox
            cmbSelectClass.ItemsSource = fdd.DefaultView;
            //Specify Display Member and Value Member for Combobox
            cmbSelectClass.DisplayMemberPath = "Fish Name";
            cmbSelectClass.SelectedValuePath = "Fish Name";


        }

        public void LoadRecord()
        {
            DataTable dt = new DataTable();
            try
            {
                string s1 = dtpPicker1.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string s2 = dtpPicker2.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

                if (cmbTopSelect.Text == "Sort BY Quantity")
                {
                    string sql = "SELECT top 50 ROW_NUMBER() OVER(ORDER BY isnull(SUM(quantity),0) DESC ) AS #, Species [Full Name], isnull(SUM(quantity),0) as Quantity FROM vwFishCatchCount WHERE added_date between '" + s1 + "' and '" + s2 + "' group by Species order by isnull(SUM(quantity),0) desc";
                    SqlCommand cmd = new SqlCommand(sql, db.con);

                    //SqlDataAdapter to Hold The Data from DataBase
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    //Open Database Connection
                    dt.Rows.Clear();
                    db.con.Open();
                    adapter.Fill(dt);
                    gridTopItems.ItemsSource = dt.DefaultView;
                }
                else if (cmbTopSelect.Text == "Sort BY Weight")
                {
                    string sql = "SELECT top 20 ROW_NUMBER() OVER(ORDER BY isnull(SUM(weight),0) DESC ) AS #, Species [Full Name], isnull(SUM(quantity),0) as Quantity, isnull(SUM(weight),0) as Weight FROM vwFishCatchCount WHERE added_date between '" + s1 + "' and '" + s2 + "' group by FishID, FishName order by isnull(SUM(weight),0) desc";
                    SqlCommand cmd = new SqlCommand(sql, db.con);

                    //SqlDataAdapter to Hold The Data from DataBase
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    //Open Database Connection
                    dt.Rows.Clear();
                    db.con.Open();
                    adapter.Fill(dt);
                    gridTopItems.ItemsSource = dt.DefaultView;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
               
            }
            finally
            {
                db.con.Close();
            }
        }


        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try {

            pnlChart.Children.Clear();
            if(cmbTopSelect.Text == String.Empty)
            {
                System.Windows.Forms.MessageBox.Show("Please Select Type From The DropDown List.", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                return;
            }
            LoadRecord();
           //LoadChartTopSelling();
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void fishReport()
        {
            
            //try
            //{
            
                System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
                var ch = new frmTopItemsChart(this);
           
            ch.TopLevel = false;
                host.Child = ch;
          
            this.lineChart.Children.Add(host);
         

            ch.Show();

                string s1 = dateForFish1.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string s2 = dateForFish2.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

            string fname = cmbSelectClass.SelectedValue.ToString();

                SqlDataAdapter da = new SqlDataAdapter();
                cn.Open();


               
                da = new SqlDataAdapter("SELECT FishName, quantity as Quantity, weight as Weight, Date FROM vwFishCatchCount WHERE Fishname = '" + fname + "' AND added_date between '" + s1 + "' and '" + s2 + "' group by  FishName, weight, quantity, Date order by quantity desc", cn);


             
                DataSet ds = new DataSet();
                da.Fill(ds, "Species");


                DataTable datee = new DataTable();
                da.Fill(datee);


            
            ch.chart1.Series[0].XValueType = ChartValueType.DateTime;
                ch.chart1.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd";
                ch.chart1.ChartAreas[0].AxisX.Interval = 1;
                ch.chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Weeks;
                ch.chart1.ChartAreas[0].AxisX.IntervalOffset = 1;

                ch.chart1.Series[0].XValueType = ChartValueType.DateTime;
                DateTime minDate = dateForFish1.SelectedDate.Value;
                DateTime maxDate = dateForFish2.SelectedDate.Value;
            ch.chart1.ChartAreas[0].AxisX.Minimum = minDate.ToOADate();
                ch.chart1.ChartAreas[0].AxisX.Maximum = maxDate.ToOADate();

          

            ch.chart1.Series[0].LegendText = "Stat";
            ch.chart1.Series[0].ChartType = SeriesChartType.Line;
            ch.chart1.Series[0].IsValueShownAsLabel = true;

            foreach (DataRow row in datee.Rows)
            {
                ch.chart1.Series[0].Points.AddXY(row["Date"],row["Quantity"]);
                Console.WriteLine(row["Date"]);
            }



            cn.Close();




            /*  }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message);
              }
              */
        }

        public void LoadChartTopSelling()
        {
            try
            {
                System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
                var ch = new frmTopItemsChart(this);
                ch.TopLevel = false;
                host.Child = ch;

                this.pnlChart.Children.Add(host);
                ch.Show();

                string s1 = dtpPicker1.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string s2 = dtpPicker2.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

                SqlDataAdapter da = new SqlDataAdapter();
                cn.Open();


                if (cmbTopSelect.Text == "Sort BY Quantity")
                {
                    da = new SqlDataAdapter("SELECT top 20  FishName, isnull(sum(quantity),0) as Quantity, isnull(sum(weight),0) as Weight  FROM vwFishCatchCount WHERE added_date between '" + s1 + "' and '" + s2 + "' group by  FishName order by FishName desc", cn);

                }
                else if (cmbTopSelect.Text == "Sort BY Weight")
                {
                    da = new SqlDataAdapter("SELECT top 20  FishName, isnull(sum(weight),0) as Quantity, isnull(sum(weight),0) as Weight  FROM vwFishCatchCount WHERE added_date between '" + s1 + "' and '" + s2 + "' group by  FishName order by FishName desc", cn);

                }
                DataSet ds = new DataSet();
                da.Fill(ds, "Top Catch Species");
                ch.chart1.DataSource = ds.Tables["Top Catch Species"];
                Series series = ch.chart1.Series[0];
                series.ChartType = SeriesChartType.Doughnut;

                series.Name = "Top Species";
                var chart = ch.chart1;
                chart.Series[0].XValueMember = "FishName";
                if (cmbTopSelect.Text == "Sort BY Quantity") { chart.Series[0].YValueMembers = "quantity"; }
                if (cmbTopSelect.Text == "Sort BY Weight") { chart.Series[0].YValueMembers = "Weight"; }

                chart.Series[0].IsValueShownAsLabel = true;
                if (cmbTopSelect.Text == "Sort BY Weight") { chart.Series[0].LabelFormat = "{#,##0.00}"; }
                if (cmbTopSelect.Text == "Sort BY Quantity") { chart.Series[0].LabelFormat = "{#,##0}"; }

                cn.Close();

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadSoldItemsChart()
        {
            try
            {
                System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
                var ch = new frmSoldItemsChart(this);
                ch.TopLevel = false;
                host.Child = ch;

                this.pnlChart2.Children.Add(host);
                ch.Show();

                string s1 = dtpPicker1sd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string s2 = dtpPicker2sd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

                SqlDataAdapter da = new SqlDataAdapter();
                cn.Open();
                
                da = new SqlDataAdapter("select fishdata.Full_Name, sum(c.total_price) as Total from TransDetails as c inner join Product as fishdata on c.FishID = fishdata.FishID where type like 'Sale' and added_date between '" + s1 + "' and '" + s2 + "' group by fishdata.Full_Name order by Total DESC", cn);

                DataSet ds = new DataSet();
                da.Fill(ds, "SOLD");
                ch.chart1.DataSource = ds.Tables["SOLD"];
                Series series = ch.chart1.Series[0];
                series.ChartType = SeriesChartType.Doughnut;

                series.Name = "SOLD ITEMS";
                var chart = ch.chart1;
                chart.Series[0].XValueMember = "Full_Name";
                chart.Series[0].YValueMembers = "Total";
                chart.Series[0].LabelFormat = "{#,##0.00}";
                chart.Series[0].IsValueShownAsLabel = true;

                cn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        private void btnPrintSD_Click(object sender, RoutedEventArgs e)
        {
            pnlChart2.Children.Clear();
            LoadSoldItemsChart();

            DataTable dt = new DataTable();
            try
            {
                string s1 = dtpPicker1sd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string s2 = dtpPicker2sd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

                //Write the SQL Query to Display all Transactions
                string sql = "select ROW_NUMBER() OVER(ORDER BY  c.price) AS #, c.FishID [PID], fishdata.Full_Name [Full Name], c.price [Price], sum(c.qty) as Quantity, sum(c.total_price) as Total from TransDetails as c inner join Product as fishdata on c.FishID = fishdata.FishID where type like 'Sale' and added_date between '" + s1 + "' and '" + s2 + "' group by c.FishID, fishdata.Full_Name, c.price";

                //SQLCommand to Execute Query
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //SqlDataAdapter to Hold The Data from DataBase
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open Database Connection
                dt.Rows.Clear();
                db.con.Open();
                adapter.Fill(dt);
                gridSoldItems.ItemsSource = dt.DefaultView;

                String x;
                string sqlsum = "select isnull(sum(total_price),0) from TransDetails  where type like 'Sale' and added_date between '" + s1 + "' and '" + s2 + "'";

                //SQLCommand to Execute Query
                SqlCommand cm = new SqlCommand(sqlsum, db.con);
                lblTotal.Content = Double.Parse(cm.ExecuteScalar().ToString()).ToString("#,##0.00");

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.con.Close();
            }
        }

        private void btnPrintAll_Click(object sender, RoutedEventArgs e)
        {
            frmRecordsReport f = new frmRecordsReport();

            string s1 = dtpPicker1.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string s2 = dtpPicker2.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

            if (cmbTopSelect.Text == "Sort BY QTY")
            {
                f.LoadTopSelling("SELECT top 10 ROW_NUMBER() OVER(ORDER BY  qty DESC ) AS #, FishID, Full_Name, isnull(sum(qty),0) as QTY, isnull(sum(total_price),0) as Total  FROM vwFishCatchCount WHERE added_date between '" + s1 + "' and '" + s2 + "' and type like 'Sale' group by FishID, Full_Name, qty order by qty desc", "From :" + s1 + " To: " + s2, "TOP SELLING ITEMS SORT BY QTY");


            }
            else if (cmbTopSelect.Text == "Sort BY Total Amount")
            {
                f.LoadTopSelling("SELECT top 10 ROW_NUMBER() OVER(ORDER BY  qty DESC ) AS #, FishID, Full_Name, isnull(sum(qty),0) as QTY, isnull(sum(total_price),0) as Total  FROM vwFishCatchCount WHERE added_date between '" + s1 + "' and '" + s2 + "' and type like 'Sale' group by FishID, Full_Name, qty order by Total desc", "From :" + s1 + " To: " + s2, "TOP SELLING ITEMS SORT BY TOTAL AMOUNT");

            }

            f.ShowDialog();
        }

        private void btnPrintData_Click(object sender, RoutedEventArgs e)
        {
            string s1 = dtpPicker1sd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string s2 = dtpPicker2sd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

            frmRecordsReport f = new frmRecordsReport();
            f.LoadSoldItems("select c.FishID, fishdata.Full_Name, c.price, sum(c.qty) as Quantity, sum(c.total_price) as Total from TransDetails as c inner join Product as fishdata on c.FishID = fishdata.FishID where type like 'Sale' and added_date between '" + s1 + "' and '" + s2 + "' group by c.FishID, fishdata.Full_Name, c.price", "From :" + s1 + " To: " + s2);
            f.ShowDialog();
        }

        private void btnShowFishData(object sender, RoutedEventArgs e)
        {
            lineChart.Children.Clear();
            fishReport();
        }



    }
}
