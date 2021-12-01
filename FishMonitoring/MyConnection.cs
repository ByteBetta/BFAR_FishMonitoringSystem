using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfPosApp
{
    class MyConnection
    {
        public SqlConnection con;
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        private double dailysales;
        private double monthlysales;
        private string dominantfish;
        private double totalsales;
        private int productline;
        private int productstock;
        private int critical;
        private string conn;


        public MyConnection()
        {
            con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connstring"].ConnectionString);
        }

        public string MyCon()
        {
            conn = @"Data Source=JERRALD\SQLEXPRESS01;Initial Catalog=project;Integrated Security=True; MultipleActiveResultSets = True";
            return conn;
        }

        public static string type;

        public double DailySales()
        {
            /*
            string transaction_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime dt = DateTime.Now;
            dt = dt.AddDays(-1);
            string s2 = dt.ToString("yyyy-MM-dd HH:mm:ss");
            cn = new SqlConnection(conn);
            cn.ConnectionString = conn;
            cn.Open();
            cm = new SqlCommand("select isnull(sum(grandTotal), 0) as grandTotal from tblTransaction where transaction_date between '" + s2 + "' and '" + transaction_date + "' and type like 'Sale'", cn);
            dailysales = double.Parse(cm.ExecuteScalar().ToString());
            cn.Close();
            return dailysales;
            */
            return 0;
        }


        public double MonthlySales()
        {
            
            string trans_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime dtm = DateTime.Now;
            dtm = dtm.AddDays(-30);
            string st2 = dtm.ToString("yyyy-MM-dd HH:mm:ss");
            cn = new SqlConnection(conn);
            cn.ConnectionString = conn;
            cn.Open();
            cm = new SqlCommand("Select COUNT(*) as totalFish  from TransDetails where added_date between '" + st2 + "' and '" + trans_date + "'", cn);
            monthlysales = double.Parse(cm.ExecuteScalar().ToString());
            cn.Close();
            return monthlysales;
         
        }

        public double TotalSales()
        {
            /*
            DateTime dtm = DateTime.Now;
            cn = new SqlConnection(conn);
            cn.ConnectionString = conn;
            cn.Open();

            cm = new SqlCommand("select isnull(sum(grandTotal), 0) as grandTotal from tblTransaction where type like 'Sale'", cn);
            totalsales = double.Parse(cm.ExecuteScalar().ToString());
            cn.Close();
            return totalsales;
            */
            return 0;
            
        }

        public string mostDominantFish()
        {
            
            string trans_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime dtm = DateTime.Now;
            dtm = dtm.AddDays(-30);
            string st2 = dtm.ToString("yyyy-MM-dd HH:mm:ss");
            cn = new SqlConnection(conn);
            cn.ConnectionString = conn;
            cn.Open();
            cm = new SqlCommand("Select TOP (1) Species, Count(Species) as Number from TransDetails where added_date between '" + st2 + "' and '" + trans_date + "'" + " Group by Species ORDER by Number desc", cn);
            
            dominantfish = cm.ExecuteScalar().ToString();
            cn.Close();
            Console.WriteLine(dominantfish);
            return dominantfish;
        }


        /*  public double ProductLine()
         {
             cn = new SqlConnection(conn);
             cn.ConnectionString = conn;
             cn.Open();
             cm = new SqlCommand("select count(*) from Product", cn);
             productline = int.Parse(cm.ExecuteScalar().ToString());
             cn.Close();
             return productline;
         }
         public double ProductStock()
         {
             cn = new SqlConnection(conn);
             cn.ConnectionString = conn;
             cn.Open();
             cm = new SqlCommand("select isnull(sum(Quantity),0)  as Quantity from Product", cn);
             productstock = int.Parse(cm.ExecuteScalar().ToString());
             cn.Close();
             return productstock;
         }
         public double CriticalProduct()
         {
             cn = new SqlConnection(conn);
             cn.ConnectionString = conn;
             cn.Open();
             cm = new SqlCommand("select count(*) from vwCriticalItems", cn);
             critical = int.Parse(cm.ExecuteScalar().ToString());
             cn.Close();
             return critical;
         }
          */

    }
}