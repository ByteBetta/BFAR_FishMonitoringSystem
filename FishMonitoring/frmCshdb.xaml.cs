using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for frmCshdb.xaml
    /// </summary>
    public partial class frmCshdb : UserControl
    {


        public SqlConnection con;
        SqlConnection cn = new SqlConnection();
        MyConnection dbcon = new MyConnection();
        frmSettingsM m;
        Main mm;
        chart css;

        public frmCshdb()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyCon());
            ChartLoad();
            MyDashboard();
            InitializeComponent();
        }



        public static string transactionType;

        Func<ChartPoint, string> labelPoint = chartpoint => string.Format("{0} ({1:P})", chartpoint.Y, chartpoint.Participation);


        public void MyDashboard()
        {
            lblUnit.Content = dbcon.DailySales().ToString();
            lblLastMonthUnit.Content = dbcon.MonthlySales().ToString();
            lbldfish.Content = dbcon.mostDominantFish().ToString();
            lblSpeciesList.Content = dbcon.numberSpecies().ToString();
            lblvessel.Content = dbcon.vessel().ToString();
            lblUnit.Content = dbcon.DailySales();
            lblfisherman.Content = dbcon.fisherman();
           // lblProductStockUnit.Content = dbcon.ProductStock().ToString("#,##0");
           //   lblCriticalUnits.Content = dbcon.CriticalProduct().ToString("#,##0");

        }



        public void ChartLoad()
        {

            /*
            PieChart1.InnerRadius = 30;
            PieChart1.LegendLocation = LegendLocation.Bottom;

            cn.Open();

            SqlCommand command = new SqlCommand("select Year(transaction_date) as year, isnull(sum(grandTotal),0.00) as grandTotal from tblTransaction where type like 'Sale' group by Year(transaction_date)", cn);
            var dr = command.ExecuteReader();

            PieChart1.Series = new SeriesCollection { };

            while (dr.Read())
            {
                PieChart1.Series.Add(
                    new PieSeries
                    {
                        Title = dr["year"].ToString(),
                        Values = new ChartValues<double> { Convert.ToDouble(dr["grandTotal"].ToString()) },
                        DataLabels = true
                    }
                );
            }

            */
            
        }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {

            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;


            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

        /*
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
            var ch = new chart();
            ch.TopLevel = false;
            host.Child = ch;

            this.pnl1.Children.Add(host);
            ch.Show();

        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            pnl1.Background = new SolidColorBrush(Color.FromRgb(21, 21, 27));
            pnl1_Copy.Background = new SolidColorBrush(Color.FromRgb(21, 21, 27));
            lblPieTitle.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));

        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            pnl1.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            pnl1_Copy.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            lblPieTitle.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        } */

        
    }
}
