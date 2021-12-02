using Fish.DAL;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            DataTable fdd = fdal.Select();

             SuggestionValues = fdd
                    .AsEnumerable()
                    .Select<System.Data.DataRow, String>(x => x.Field<String>("Species"))
                    .ToArray();
            //Specify DataSource for Category ComboBox


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
                            foreach (DataRow dr in gearUsed.Rows)
                            {
                                tblgear.Content += dr["gearUsed"].ToString() + Environment.NewLine;

                            }



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

       
    }
}
