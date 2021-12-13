using Dapper;
using Fish.DAL;
using Project.BLL;
using Project.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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

using WpfPosApp.BLL;
using WpfPosApp.DAL;
using Color = System.Windows.Media.Color;


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
        DatePickerCalendar datePickerCalendar = new DatePickerCalendar();

        string defaultChart = "line";
        string sqlreport = "";

        FishDAL fdal = new FishDAL();
        string[] SuggestionValues;
        public frmRecords()
        {
            InitializeComponent();
            cn = new SqlConnection(db.MyCon());
            dtpPicker1.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            dtpPicker2.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            dtpPicker1sd.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            dtpPicker2sd.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            dtpPicker1.SelectedDate = DateTime.Now;
            dtpPicker2.SelectedDate = DateTime.Now.AddDays(+1);
            dtpPicker1sd.SelectedDate = DateTime.Now;
            dtpPicker2sd.SelectedDate = DateTime.Now.AddDays(+1);
            dateForFish1.SelectedDate = DateTime.Now;
            dateForFish2.SelectedDate = DateTime.Now.AddDays(+1);

            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearchSpecies.TextChanged += TxtSearchSpecies_TextChanged;
            txtSearchSpecies2.TextChanged += TxtSearchSpecies2_TextChanged;
            DataTable fdd = fdal.Select();

             SuggestionValues = fdd
                    .AsEnumerable()
                    .Select<System.Data.DataRow, String>(x => x.Field<String>("Species"))
                    .ToArray();

            //Specify DataSource for Category ComboBox
            for (int i = 1950; i <= int.Parse(DateTime.Now.ToString("yyyy")); i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i;
                dtpPicker1mf.Items.Add(item);
                //datepickerYear.Items.Add(item);
            }
            for (int i = 1950; i <= int.Parse(DateTime.Now.ToString("yyyy")); i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i;
               // dtpPicker1mf.Items.Add(item);
                datepickerYear.Items.Add(item);
            }
            for (int i = 1950; i <= int.Parse(DateTime.Now.ToString("yyyy")); i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i;
                // dtpPicker1mf.Items.Add(item);
                datepickerYear2.Items.Add(item);
            }
            for (int i = 1950; i <= int.Parse(DateTime.Now.ToString("yyyy")); i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i;
                // dtpPicker1mf.Items.Add(item);
                nsapdatepickerYear1.Items.Add(item);
            }

           
            selectiongeneration();


        }



        public void selectiongeneration()
        {
            List<string> lsitedata = new List<string> { "All Sites" };
            db.con.Open();
            string cmd = "Select title from landingSite";
            IList<landingSiteBLL> lsiteholder = db.con.Query<landingSiteBLL>(cmd).ToList();
            db.con.Close();

            foreach (var lsite in lsiteholder)
            {
                lsitedata.Add(lsite.title);
            }
            sitePicker1.ItemsSource = lsitedata;
            cmblandingsite.ItemsSource = lsitedata;
            cmblandingsite2.ItemsSource = lsitedata;
            cmbselectsite.ItemsSource = lsitedata;


            // MonthList
            db.con.Open();
            SqlCommand cmd2 = new SqlCommand("Select MonthNumber,MonthName from Months", db.con);
            SqlDataAdapter adapt = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            adapt.Fill(dt);
            db.con.Close();

            DataRow Filaa = dt.NewRow();
            Filaa["MonthName"] = "Select Month";
            DatePickerMonth.DisplayMemberPath = "MonthName";
            DatePickerMonth.SelectedValuePath = "MonthNumber";
            DatePickerMonth.ItemsSource = dt.DefaultView;
            nsapdatepickermonth2.DisplayMemberPath = "MonthName";
            nsapdatepickermonth2.SelectedValuePath = "MonthNumber";
            nsapdatepickermonth2.ItemsSource = dt.DefaultView;
            db.con.Close();

            // Vessel List
            List<string> vessels = new List<string> { "All Vessels" };
            db.con.Open();
            string cmdvessels = "Select VesselName from Vessel";
            IList<VesselBLL> vesselhoder = db.con.Query<VesselBLL>(cmdvessels).ToList();
            db.con.Close();

            foreach (var vessel in vesselhoder)
            {
                vessels.Add(vessel.VesselName);
            }
            cmbvessels.ItemsSource = vessels;
            cmbvessels2.ItemsSource = vessels;
            vesselpicker.ItemsSource = vessels;

            // gear List
            List<string> gears = new List<string> { "All Gears" };
            db.con.Open();
            string cmdgears = "Select title from gearList";
            IList<gearsBLL> gearholder = db.con.Query<gearsBLL>(cmdgears).ToList();
            db.con.Close();

            foreach (var gear in gearholder)
            {
                gears.Add(gear.title);
            }
            cmbgearused.ItemsSource = gears;
            cmbgearused2.ItemsSource = gears;
            gearpicker.ItemsSource = gears;

            // fisherman List
            List<string> fishermandata = new List<string> { "All Fisherman" };
            db.con.Open();
            string cmdfisherman = "Select name from Dealers";
            IList<DealersBLL> fishermanholder = db.con.Query<DealersBLL>(cmdfisherman).ToList();
            db.con.Close();

            foreach (var fisherman in fishermanholder)
            {
                fishermandata.Add(fisherman.name);
            }
            cmbFisherman.ItemsSource = fishermandata;
            cmbFisherman2.ItemsSource = fishermandata;


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
                    string sql = "SELECT top 50 ROW_NUMBER() OVER(ORDER BY isnull(SUM(quantity),0) DESC ) AS #, Species [Full Name], isnull(SUM(quantity),0) as Quantity FROM vwFishCatchCount WHERE added_date between '" + s1 + "' and '" + s2 + "' group by Species ORDER BY # asc";
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
                if (cmbTopSelect.Text == String.Empty)
                {
                    System.Windows.Forms.MessageBox.Show("Please Select Type From The DropDown List.", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    return;
                }
                LoadRecord();
                LoadChartTopSelling();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void fishReport()
        {

            try
            {

            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
            var ch = new frmTopItemsChart(this);

            ch.TopLevel = false;
            host.Child = ch;




            ch.Show();

            string s1 = dateForFish1.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string s2 = dateForFish2.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

            string fname = txtSearch.Text;

            SqlDataAdapter da = new SqlDataAdapter();
            cn.Open();



            da = new SqlDataAdapter("SELECT Count(Species) as volume, Species, AVG(weight) as Weight, AVG(length) as Length, MAX(weight) as MWeight, MAX(length) as MLength, MIN(weight) as LWeight, MIN(length) as LLength FROM dbo.TransDetails WHERE Species = '" + fname + "' AND added_date between '" + s1 + "' and '" + s2 + "' group by  Species", cn);
            DataTable ds = new DataTable();
            DataTable image = new DataTable();
            DataTable gearUsed = new DataTable();
            da.Fill(ds);
            da = new SqlDataAdapter("SELECT Img FROM dbo.FishDetails WHERE Species = '" + fname + "'", cn);
            da.Fill(image);
            da = new SqlDataAdapter("SELECT TOP 3 gearUsed FROM dbo.TransDetails WHERE Species = '" + fname + "' AND added_date between '" + s1 + "' and '" + s2 + "' group by  gearUsed", cn);
            da.Fill(gearUsed);
             cn.Close();
            if(ds != null)
                {
                    if(ds.Rows.Count > 0)
                    {
                        foreach (DataRow dataRow in ds.Rows)
                        {
                            foreach(DataRow dr in image.Rows)
                            {
                                string paths = System.Windows.Forms.Application.StartupPath.Substring(0, (System.Windows.Forms.Application.StartupPath.Length - 10));
                                string imagePath = paths + "\\Images\\Product\\" + dr["Img"];
                                imgBox.ImageSource = new BitmapImage(new Uri(imagePath));
                                
                            }
                            averagelegnth.Content = dataRow["Length"] + " cm";
                            averageweight.Content = dataRow["Weight"] + " kg";
                            lblll.Content = dataRow["LLength"] + " cm";
                            lbllw.Content = dataRow["LWeight"] + " kg";
                            lblhl.Content = dataRow["MLength"] + " cm";
                            lblhw.Content = dataRow["MWeight"] + " kg";
                            tblVolume.Content = dataRow["volume"];
                         



                        }
                        foreach (DataRow dr in gearUsed.Rows)
                        {
                            tblgear.Content += dr["gearUsed"].ToString() + Environment.NewLine;

                        }
                    } else
                    {
                        averagelegnth.Content = 0;
                        averageweight.Content = 0;
                        lblblt.Content =0;
                        lbllw.Content = 0;
                        lblhl.Content = 0;
                        lblhw.Content = 0;
                    }
                }
            

            


            




             }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message);
              }
              
        }

        public void LoadChartTopSelling()
        {
            try
            {
                Title title = new Title();
                System.Drawing.FontFamily family = new System.Drawing.FontFamily("Arial");
                title.Font = new Font(family, 14);
                title.Text = "Top 10 Species";

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
                    da = new SqlDataAdapter("SELECT top 10 ROW_NUMBER() OVER(ORDER BY isnull(SUM(quantity),0) DESC ) AS #, Species [Full Name], isnull(SUM(quantity),0) as Quantity FROM vwFishCatchCount WHERE added_date between '" + s1 + "' and '" + s2 + "' group by Species order by isnull(SUM(quantity),0) desc", cn);

                }

                DataSet ds = new DataSet();
                da.Fill(ds, "Top Catch Species");
                ch.chart1.DataSource = ds.Tables["Top Catch Species"];
                Series series = ch.chart1.Series[0];
                series.ChartType = SeriesChartType.Doughnut;
                
                ch.chart1.Titles.Add(title);

                series.Name = "Top Species";
                var chart = ch.chart1;
                chart.Series[0].XValueMember = "Full Name";
                if (cmbTopSelect.Text == "Sort BY Quantity") { chart.Series[0].YValueMembers = "quantity"; }
                chart.Series[0].IsValueShownAsLabel = true;
                if (cmbTopSelect.Text == "Sort BY Quantity") { chart.Series[0].LabelFormat = "{#,##0}"; }

                cn.Close();

            } catch (Exception ex)
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
            //lineChart.Children.Clear();
            fishReport();
        }

        private string _currentInput = "";
        private string _currentSuggestion = "";
        private string _currentText = "";

        private int _selectionStart;
        private int _selectionLength;
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var input = txtSearch.Text;
            if (input.Length > _currentInput.Length && input != _currentSuggestion)
            {
                _currentSuggestion = SuggestionValues.FirstOrDefault(x => x.StartsWith(input));
                if (_currentSuggestion != null)
                {
                    _currentText = _currentSuggestion;
                    _selectionStart = input.Length;
                    _selectionLength = _currentSuggestion.Length - input.Length;

                    txtSearch.Text = _currentText;
                    txtSearch.Select(_selectionStart, _selectionLength);
                }
            }
            _currentInput = input;

        }

        private void btn_loadData(object sender, RoutedEventArgs e)
        {
            string pcs = "##0pcs";
            string kg = "##0kg";
            string lsite = "";
            string gear = "";
            string vessels = "";

            if(sitePicker1.Text != "All Sites")
            {
                lsite = "AND c.landingSite = '" + sitePicker1.Text + "'";
            }

            if(gearpicker.Text != "All Gears")
            {
                gear = "AND c.gearUsed = '" + gearpicker.Text + "'"; 
            }

            if (vesselpicker.Text != "All Vessels")
            {
                vessels = "AND c.vessels = '" + vesselpicker.Text + "'";
            }


            if (dtpPicker1mf.Text == String.Empty || cmbtype.Text == String.Empty || sitePicker1.Text == String.Empty || gearpicker.Text == String.Empty || vesselpicker.Text == String.Empty)
            {
                System.Windows.Forms.MessageBox.Show("Please Select Items From The DropDown List.", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            } else
            {
                if (cmbtype.Text == "Number of Boxes") {
                    loadChartForFishStat("c.TotalBox", pcs.ToString(), lsite, defaultChart, gear, vessels);
                }
                if (cmbtype.Text == "Total Weight of Boxes")
                {
                    loadChartForFishStat("c.totalWeightBox", kg, lsite, defaultChart, gear, vessels);
                }
                if (cmbtype.Text == "Number of Sampled Boxes")
                {
                    loadChartForFishStat("c.totalSampleBox", pcs , lsite, defaultChart, gear, vessels);
                }
                if (cmbtype.Text == "Total Weight of Boxes")
                {
                    loadChartForFishStat("c.totalSampleWeightBox", kg, lsite, defaultChart, gear, vessels);
                }
            }

        }

        private void loadChartForFishStat(string typedata, string output, string landingsite, string charttype, string gear, string vessel)
        {

            this.panelchartforstats.Children.Clear();
            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
            var chartStats = new frmFishStatistic(this);
            chartStats.TopLevel = false;
            host.Child = chartStats;

            this.panelchartforstats.Children.Add(host);
            chartStats.Show();

            Axis XA = chartStats.chart1.ChartAreas[0].AxisX;
            Axis YA = chartStats.chart1.ChartAreas[0].AxisY;
            Series S1 = chartStats.chart1.Series[0];

         

            if(charttype == "line")
            {
                S1.BorderWidth = 5;
                S1.ChartType = SeriesChartType.Line;
            } else
            {
                S1.ChartType = SeriesChartType.Column;
            }
            cn.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            string sqlexecute = "SELECT Months.MonthNumber, Months.MonthName as date, SUM(COALESCE(" + typedata.ToString() + ", 0)) as Volume " +
                                " FROM" +
                                " Months " +
                                "LEFT JOIN tblTransaction c ON DATENAME(month, c.[transaction_date]) = Months.MonthName " +
                               " AND DATENAME(year, c.[transaction_date]) = " + dtpPicker1mf.Text +
                                landingsite.ToString() +
                                vessel.ToString() +
                                gear.ToString() +
                                " GROUP BY Months.MonthName, Months.MonthNumber ";
            DataTable ds = new DataTable();
            da = new SqlDataAdapter(sqlexecute.ToString(), cn);
            this.sqlreport = sqlexecute;
            da.Fill(ds);
            cn.Close();
            foreach (DataRow dataRow in ds.Rows)
            {
                S1.Points.AddXY(dataRow["date"], Convert.ToDouble(dataRow["Volume"]));
            }

            S1.LegendText = "Year " + dtpPicker1mf.Text;
            // move to the bottom center:
            chartStats.chart1.Legends[0].Docking = Docking.Bottom;
            chartStats.chart1.Legends[0].Alignment = StringAlignment.Center;
            S1.IsXValueIndexed = true;
            S1.IsValueShownAsLabel = true;
            XA.MajorGrid.Enabled = false;         // no gridlines 
            XA.Interval = 1;                                // ..every 1 months

            YA.LabelStyle.Format = output;  // for kilos etc you need to scale the y-values!

        }

        private void BtnfishStats_Copy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnfishStats_Copy_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeVIew_Click(object sender, RoutedEventArgs e)
        {
          
            if(defaultChart == "line")
            {
                defaultChart = "bar";
                Console.WriteLine("chart updated");
            //    reset("bar");
            } else
            {
                defaultChart = "line";
                Console.WriteLine("error updated");
               // reset("line");
            }
           
        }

      
        private void TxtSearchSpecies_TextChanged(object sender, TextChangedEventArgs e)
        {
            var input = txtSearchSpecies.Text;
            if (input.Length > _currentInput.Length && input != _currentSuggestion)
            {
                _currentSuggestion = SuggestionValues.FirstOrDefault(x => x.StartsWith(input));
                if (_currentSuggestion != null)
                {
                    _currentText = _currentSuggestion;
                    _selectionStart = input.Length;
                    _selectionLength = _currentSuggestion.Length - input.Length;

                    txtSearchSpecies.Text = _currentText;
                    txtSearchSpecies.Select(_selectionStart, _selectionLength);
                }
            }
            _currentInput = input;
        }

        private void BtnfishStatss_Click(object sender, RoutedEventArgs e)
        {
            if (datepickerYear.Text == String.Empty || DatePickerMonth.Text == String.Empty || txtSearchSpecies.Text == String.Empty || cmbFisherman.Text == String.Empty || cmbgearused.Text == String.Empty || cmblandingsite.Text == String.Empty || cmbvessels.Text == String.Empty)
            {
                System.Windows.Forms.MessageBox.Show("Please Select Items From The DropDown Lists", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
            else
            {
                string newDate = datepickerYear.Text + "/" + DatePickerMonth.SelectedValue + "/" + "1";
                string vessels = "";
                string fisherman = "";
                string landingsite = "";
                string gear = "";
                

                if(cmbvessels.Text != "All Vessels")
                {
                    vessels = " AND [project].[dbo].[TransDetails].vessels = '" + cmbvessels.Text + "'";
                }
                if (cmbFisherman.Text != "All Fisherman")
                {
                    fisherman = " AND [project].[dbo].[TransDetails].fisherman = '" + cmbFisherman.Text + "'";
                }
                if (cmbgearused.Text != "All Gears")
                {
                    gear = " AND [project].[dbo].[TransDetails].gearUsed = '" + cmbgearused.Text + "'";
                }
                if (cmblandingsite.Text != "All Sites")
                {
                    landingsite = " AND [project].[dbo].[TransDetails].landingSite = '" + cmblandingsite.Text + "'";
                }

                Console.WriteLine(fisherman);
                generatefishmonitoring(newDate, txtSearchSpecies.Text, vessels, fisherman, gear, landingsite);
            }
        }

        private void generatefishmonitoring(string date, string speciesadapter, string vessel, string fisherman, string gear, string landingsite)
        {
            this.panelforSpecies.Children.Clear();
            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
            var chartStats = new frmFishStatistic(this);
            chartStats.TopLevel = false;
            host.Child = chartStats;

            this.panelforSpecies.Children.Add(host);
            chartStats.Show();

            Axis XA = chartStats.chart1.ChartAreas[0].AxisX;
            Axis YA = chartStats.chart1.ChartAreas[0].AxisY;
            Series S1 = chartStats.chart1.Series[0];



            
           S1.BorderWidth = 5;
            S1.ChartType = SeriesChartType.Line;
           
          
            cn.Open();
            SqlDataAdapter da = new SqlDataAdapter();

            string SqlGenerate = "DECLARE @date DATE; " +
            " DECLARE @start_date DATE;" +
            " DECLARE @end_date DATE;" +
            " DECLARE @loop_date DATE; " +
    
            //declaring a table variable
            " DECLARE @dates TABLE (date DATE);" +
    
            // setting the first and the last date in the month given by date
            " SET @date = '" + date.ToString() + "'" + 
            " SET @start_date = DATEFROMPARTS(YEAR(@date ), MONTH(@date ), '01'); " +
            " SET @end_date = EOMONTH(@date);" +
 
           // populating a table (variable) with all dates in a given month
            " SET @loop_date = @start_date;" +
            " WHILE @loop_date <= @end_date" +
            " BEGIN" +
               " INSERT INTO @dates(date) VALUES (@loop_date);" +
               " SET @loop_date = DATEADD(DAY, 1, @loop_date);" +
            " END;" +

            " SELECT" +
                " d.date," +
                " COALESCE(COUNT([project].[dbo].[TransDetails].TransDetID), 0) AS Volume" +
            " FROM @dates d" +
            " LEFT JOIN [project].[dbo].[TransDetails] ON d.date = CAST([project].[dbo].[TransDetails].added_date AS DATE)" +
            " AND  [project].[dbo].[TransDetails].Species ='" + speciesadapter.ToString() + "'" +
            vessel.ToString() +
            fisherman.ToString() +
           landingsite.ToString() +
           gear.ToString() +
            " GROUP BY d.date;";

            this.sqlreport = SqlGenerate;
            DataTable ds = new DataTable();
            da = new SqlDataAdapter(SqlGenerate.ToString(), cn);

            da.Fill(ds);
            cn.Close();

 
            foreach (DataRow dataRow in ds.Rows)
            {
                S1.Points.AddXY(dataRow["date"], dataRow["Volume"]);
            }

            S1.LegendText = date;
            // move to the bottom center:
            chartStats.chart1.Legends[0].Docking = Docking.Bottom;
            chartStats.chart1.Legends[0].Alignment = StringAlignment.Center;

            S1.IsValueShownAsLabel = true;
            XA.MajorGrid.Enabled = false;         // no gridlines 
            XA.Interval = 1;                                // ..every 1 months
            YA.LabelStyle.Format = "0.00 pcs";


        }

        private void Btnmonitoringprint_Click(object sender, RoutedEventArgs e)
        {
            if (datepickerYear.Text == String.Empty || DatePickerMonth.Text == String.Empty || txtSearchSpecies.Text == String.Empty || cmbFisherman.Text == String.Empty || cmbgearused.Text == String.Empty || cmblandingsite.Text == String.Empty || cmbvessels.Text == String.Empty)
            {
                System.Windows.Forms.MessageBox.Show("Please Select Items From The DropDown Lists", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
            else
            {
                string newDate = datepickerYear.Text + "/" + DatePickerMonth.SelectedValue + "/" + "1";
                string vessels = "";
                string fisherman = "";
                string landingsite = "";
                string gear = "";

                generatefishmonitoring(newDate, txtSearchSpecies.Text, vessels, fisherman, gear, landingsite);

                if (cmbvessels.Text != "All Vessels")
                {
                    vessels = " AND [project].[dbo].[TransDetails].vessels = '" + cmbvessels.Text + "'";
                }
                if (cmbFisherman.Text != "All Fisherman")
                {
                    fisherman = " AND [project].[dbo].[TransDetails].fisherman = '" + cmbFisherman.Text + "'";
                }
                if (cmbgearused.Text != "All Gears")
                {
                    gear = " AND [project].[dbo].[TransDetails].gearUsed = '" + cmbgearused.Text + "'";
                }
                if (cmblandingsite.Text != "All Sites")
                {
                    landingsite = " AND [project].[dbo].[TransDetails].landingSite = '" + cmblandingsite.Text + "'";
                }


                frmRecordsReport f = new frmRecordsReport();
                UserBLL userBLL = new UserBLL();
                string currentuser = getUser();
                Console.WriteLine(currentuser);
                f.reportmonitoring(DatePickerMonth.Text, datepickerYear.Text, txtSearchSpecies.Text, cmbvessels.Text, cmbFisherman.Text, cmblandingsite.Text, cmbgearused.Text, sqlreport, currentuser);
                f.ShowDialog();

            }
        }


        private string getUser()
        {
            try
            {
                int userid = frmLogin._id;

                db.con.Open();
                string sqlquery = "SELECT Name, Surname, UserName, Gender, Birth_Date FROM Login WHERE UserID = " + userid.ToString();

                SqlCommand command = new SqlCommand(sqlquery, db.con);

                SqlDataReader sdr = command.ExecuteReader();

                string firstname = "";
                string lastname = "";
                while (sdr.Read())
                {
                    firstname = sdr["Name"].ToString();
                    lastname = sdr["Surname"].ToString();
                    return char.ToUpper(firstname[0]) + firstname.Substring(1) + " " + char.ToUpper(lastname[0]) + lastname.Substring(1);
                }
                db.con.Close();

                return char.ToUpper(firstname[0]) + firstname.Substring(1) + " " + char.ToUpper(lastname[0]) + lastname.Substring(1);
            } catch (Exception e)
            {
                return e.ToString();
            } finally
            {
                db.con.Close();
            }

        }

        private void TxtSearchSpecies2_TextChanged(object sender, TextChangedEventArgs e)
        {
            var input = txtSearchSpecies2.Text;
            if (input.Length > _currentInput.Length && input != _currentSuggestion)
            {
                _currentSuggestion = SuggestionValues.FirstOrDefault(x => x.StartsWith(input));
                if (_currentSuggestion != null)
                {
                    _currentText = _currentSuggestion;
                    _selectionStart = input.Length;
                    _selectionLength = _currentSuggestion.Length - input.Length;

                    txtSearchSpecies2.Text = _currentText;
                    txtSearchSpecies2.Select(_selectionStart, _selectionLength);
                }
            }
            _currentInput = input;
        }

        private void Btnstatsmonthly_Click(object sender, RoutedEventArgs e)
        {
            if (datepickerYear2.Text == String.Empty || txtSearchSpecies2.Text == String.Empty || cmbFisherman2.Text == String.Empty || cmbgearused2.Text == String.Empty || cmblandingsite2.Text == String.Empty || cmbvessels2.Text == String.Empty)
            {
                System.Windows.Forms.MessageBox.Show("Please Select Items From The DropDown Lists", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
            else
            {
                //string newDate = datepickerYear.Text + "/" + DatePickerMonth.SelectedValue + "/" + "1";
                string vessels = "";
                string fisherman = "";
                string landingsite = "";
                string gear = "";


                if (cmbvessels2.Text != "All Vessels")
                {
                    vessels = " AND c.vessels = '" + cmbvessels2.Text + "'";
                }
                if (cmbFisherman2.Text != "All Fisherman")
                {
                    fisherman = " AND c.fisherman = '" + cmbFisherman2.Text + "'";
                }
                if (cmbgearused2.Text != "All Gears")
                {
                    gear = " AND c.gearUsed = '" + cmbgearused2.Text + "'";
                }
                if (cmblandingsite2.Text != "All Sites")
                {
                    landingsite = " c.landingSite = '" + cmblandingsite2.Text + "'";
                }

                Console.WriteLine(fisherman);
                fishreportmonthly(defaultChart, landingsite, txtSearchSpecies2.Text, gear, fisherman, vessels);
            }
        }

        private void fishreportmonthly(string charttype, string landingsite, string species, string gear, string fisherman, string vessels)
        {
            this.panelforSpecies2.Children.Clear();
            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
            var chartStats = new frmFishStatistic(this);
            chartStats.TopLevel = false;
            host.Child = chartStats;

            this.panelforSpecies2.Children.Add(host);
            chartStats.Show();

            Axis XA = chartStats.chart1.ChartAreas[0].AxisX;
            Axis YA = chartStats.chart1.ChartAreas[0].AxisY;
            Series S1 = chartStats.chart1.Series[0];



            if (charttype == "line")
            {
                S1.BorderWidth = 5;
                S1.ChartType = SeriesChartType.Line;
            }
            else
            {
                S1.ChartType = SeriesChartType.Column;
            }
            cn.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            string sqlexecute = "SELECT dbo.Months.MonthNumber, Months.MonthName as date, SUM(COALESCE(c.quantity, 0)) as Volume" +
                                " FROM" +
                                " Months " +
                                "LEFT JOIN TransDetails c ON DATENAME(month, c.added_date) = Months.MonthName " +
                               " AND DATENAME(year, c.added_date) = " + datepickerYear2.Text +
                               " AND  c.Species ='" + species.ToString() + "'" +
                                landingsite.ToString() +
                                gear.ToString() +
                                fisherman.ToString() +
                                vessels.ToString() +
                                " GROUP BY Months.MonthName, Months.MonthNumber ";
            this.sqlreport = sqlexecute;
            DataTable ds = new DataTable();
            da = new SqlDataAdapter(sqlexecute.ToString(), cn);
            Console.WriteLine(sqlexecute);
            da.Fill(ds);
            cn.Close();
            foreach (DataRow dataRow in ds.Rows)
            {
                S1.Points.AddXY(dataRow["date"], Convert.ToDouble(dataRow["Volume"]));
            }

            S1.LegendText = "Year " + datepickerYear2.Text;
            // move to the bottom center:
            chartStats.chart1.Legends[0].Docking = Docking.Bottom;
            chartStats.chart1.Legends[0].Alignment = StringAlignment.Center;
            S1.IsXValueIndexed = true;
            S1.IsValueShownAsLabel = true;
            XA.MajorGrid.Enabled = false;         // no gridlines 
            XA.Interval = 1;                                // ..every 1 months

            YA.LabelStyle.Format = "0 pcs";  // for kilos etc you need to scale the y-values!
        }

        private void Btnmonitoringprint2_Click(object sender, RoutedEventArgs e)
        {
            if (datepickerYear2.Text == String.Empty || txtSearchSpecies2.Text == String.Empty || cmbFisherman2.Text == String.Empty || cmbgearused2.Text == String.Empty || cmblandingsite2.Text == String.Empty || cmbvessels2.Text == String.Empty)
            {
                System.Windows.Forms.MessageBox.Show("Please Select Items From The DropDown Lists", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
            else
            {
                //string newDate = datepickerYear.Text + "/" + DatePickerMonth.SelectedValue + "/" + "1";
                string vessels = "";
                string fisherman = "";
                string landingsite = "";
                string gear = "";


                if (cmbvessels2.Text != "All Vessels")
                {
                    vessels = " AND c.vessels = '" + cmbvessels2.Text + "'";
                }
                if (cmbFisherman2.Text != "All Fisherman")
                {
                    fisherman = " AND c.fisherman = '" + cmbFisherman2.Text + "'";
                }
                if (cmbgearused2.Text != "All Gears")
                {
                    gear = " AND c.gearUsed = '" + cmbgearused2.Text + "'";
                }
                if (cmblandingsite2.Text != "All Sites")
                {
                    landingsite = " c.landingSite = '" + cmblandingsite2.Text + "'";
                }

        
                fishreportmonthly(defaultChart, landingsite, txtSearchSpecies2.Text, gear, fisherman, vessels);

                frmRecordsReport f = new frmRecordsReport();
                string currentuser = getUser();
                Console.WriteLine(currentuser);
                f.reportmonitoring2(datepickerYear2.Text, txtSearchSpecies2.Text, cmbvessels2.Text, cmbFisherman2.Text, cmblandingsite2.Text, cmbgearused2.Text, sqlreport, currentuser, "Monthly Volume");
                f.ShowDialog();
                db.con.Close();
            }
        }

        private void BtnprintfishStats_Click(object sender, RoutedEventArgs e)
        {
            string pcs = "##0pcs";
            string kg = "##0kg";
            string lsite = "";
            string gear = "";
            string vessels = "";

            if (sitePicker1.Text != "All Sites")
            {
                lsite = "AND c.landingSite = '" + sitePicker1.Text + "'";
            }

            if (gearpicker.Text != "All Gears")
            {
                gear = "AND c.gearUsed = '" + gearpicker.Text + "'";
            }

            if (vesselpicker.Text != "All Vessels")
            {
                vessels = "AND c.vessels = '" + vesselpicker.Text + "'";
            }


            if (dtpPicker1mf.Text == String.Empty || cmbtype.Text == String.Empty || sitePicker1.Text == String.Empty || gearpicker.Text == String.Empty || vesselpicker.Text == String.Empty)
            {
                System.Windows.Forms.MessageBox.Show("Please Select Items From The DropDown List.", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
            else
            {
                if (cmbtype.Text == "Number of Boxes")
                {
                    loadChartForFishStat("c.TotalBox", pcs.ToString(), lsite, defaultChart, gear, vessels);
                }
                if (cmbtype.Text == "Total Weight of Boxes")
                {
                    loadChartForFishStat("c.totalWeightBox", kg, lsite, defaultChart, gear, vessels);
                }
                if (cmbtype.Text == "Number of Sampled Boxes")
                {
                    loadChartForFishStat("c.totalSampleBox", pcs, lsite, defaultChart, gear, vessels);
                }
                if (cmbtype.Text == "Total Weight of Boxes")
                {
                    loadChartForFishStat("c.totalSampleWeightBox", kg, lsite, defaultChart, gear, vessels);
                }

                frmRecordsReport f = new frmRecordsReport();
                string currentuser = getUser();
                Console.WriteLine(currentuser);
                f.reportmonitoring2(dtpPicker1mf.Text, "-", vesselpicker.Text, "-", sitePicker1.Text, gearpicker.Text, sqlreport, currentuser, cmbtype.Text);
                f.ShowDialog();
                db.con.Close();


            }


        }

        private void Btnprintreport_Click(object sender, RoutedEventArgs e)
        {
            lblspecies.Content = "";
            string siteselected = "";
            string siteselected2 = "";
            string speciesholder = "";

            if (nsapdatepickerYear1.Text == String.Empty || nsapdatepickermonth2.Text == String.Empty || cmbselectsite.Text == String.Empty)
            {
                System.Windows.Forms.MessageBox.Show("Please Select Items From The DropDown List.", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
            else
            {
                if (cmbselectsite.Text != "All Sites")
                {
                    siteselected = "AND tblTransaction.landingSite = '" + cmbselectsite.Text + "'";
                    siteselected2 = " AND TransDetails.landingSite = '" + cmbselectsite.Text + "'";
                }
                TransactionDAL tdal = new TransactionDAL();

                DataTable dtforgear = tdal.gearSortation(nsapdatepickermonth2.SelectedValue.ToString(), nsapdatepickerYear1.Text, siteselected);
                gridnsap.ItemsSource = dtforgear.DefaultView;
                DataTable dtfornumbeofboats = tdal.getNumberofBoats(nsapdatepickermonth2.SelectedValue.ToString(), nsapdatepickerYear1.Text, siteselected);
                foreach (DataRow dataRow in dtfornumbeofboats.Rows)
                {
                    lbltnbslbl.Content = dataRow["NumberofBoat"] + " pcs"; ;
                }
                DataTable dtfornumberofFish = tdal.fishvolumeandweight(nsapdatepickermonth2.SelectedValue.ToString(), nsapdatepickerYear1.Text, siteselected2);
                foreach (DataRow dataRow in dtfornumberofFish.Rows)
                {
                    lblfishnumber.Content = dataRow["FishTotal"] + " pcs"; ;
                    lblfishweight.Content = dataRow["FishMeasured"] + " kgs";
                }
                DataTable dtforspecies = tdal.topSpecies(nsapdatepickermonth2.SelectedValue.ToString(), nsapdatepickerYear1.Text, siteselected2);
                foreach (DataRow dataRow in dtforspecies.Rows)
                {
                    lblspecies.Content += dataRow["Species"].ToString() + Environment.NewLine;
                    speciesholder += dataRow["Species"].ToString() + ",";
                }
            }


            TransactionDAL tdal2 = new TransactionDAL();
            frmRecordsReport f = new frmRecordsReport();
            string currentuser = getUser();
            Console.WriteLine(currentuser);
            string sql = " SELECT tblTransaction.gearUsed as gearused, Count(tblTransaction.vessels) as noofboatslanded, SUM(tblTransaction.totalWeightBox) as totalboatscatch" +
                 " FROM[project].[dbo].tblTransaction " +
                    " WHERE MONTH(tblTransaction.[transaction_date]) = '" + nsapdatepickermonth2.SelectedValue.ToString() + "' AND YEAR(tblTransaction.[transaction_date]) ='" + nsapdatepickerYear1.Text + "'" +
                    siteselected.ToString() +
                    "  GROUP by tblTransaction.gearUsed";
            Console.WriteLine(sql);
            f.reportmonitoring3(nsapdatepickerYear1.Text, nsapdatepickermonth2.Text, lbltnbslbl.Content.ToString(), lblfishnumber.Content.ToString(), lblfishweight.Content.ToString(), speciesholder, currentuser, sql, cmbselectsite.Text);
            f.ShowDialog();
            db.con.Close();
        }

        private void Btnloadreport_Click(object sender, RoutedEventArgs e)
        {
            lblspecies.Content = "";
            string siteselected = "";
            string siteselected2 = "";

            if (nsapdatepickerYear1.Text == String.Empty || nsapdatepickermonth2.Text == String.Empty || cmbselectsite.Text == String.Empty)
            {
                System.Windows.Forms.MessageBox.Show("Please Select Items From The DropDown List.", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
            else
            {
                if (cmbselectsite.Text != "All Sites")
                {
                    siteselected = "AND tblTransaction.landingSite = '" + cmbselectsite.Text + "'";
                    siteselected2 = " AND TransDetails.landingSite = '" + cmbselectsite.Text + "'";
                }
                TransactionDAL tdal = new TransactionDAL();

                DataTable dtforgear = tdal.gearSortation(nsapdatepickermonth2.SelectedValue.ToString(), nsapdatepickerYear1.Text, siteselected);
                gridnsap.ItemsSource = dtforgear.DefaultView;
                DataTable dtfornumbeofboats = tdal.getNumberofBoats(nsapdatepickermonth2.SelectedValue.ToString(), nsapdatepickerYear1.Text, siteselected);
                foreach (DataRow dataRow in dtfornumbeofboats.Rows)
                {
                    lbltnbslbl.Content = dataRow["NumberofBoat"] + " pcs"; ;
                }
                DataTable dtfornumberofFish = tdal.fishvolumeandweight(nsapdatepickermonth2.SelectedValue.ToString(), nsapdatepickerYear1.Text, siteselected2);
                foreach (DataRow dataRow in dtfornumberofFish.Rows)
                {
                    lblfishnumber.Content = dataRow["FishTotal"] + " pcs"; ;
                    lblfishweight.Content = dataRow["FishMeasured"] + " kgs";
                }
                DataTable dtforspecies = tdal.topSpecies(nsapdatepickermonth2.SelectedValue.ToString(), nsapdatepickerYear1.Text, siteselected2);
                foreach (DataRow dataRow in dtforspecies.Rows)
                {
                    lblspecies.Content += dataRow["Species"].ToString() + Environment.NewLine;
                }
            }
        }
    }
    }

