using Project.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
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
using System.Windows.Shapes;

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for frmTransactions.xaml
    /// </summary>
    public partial class frmTransactions : UserControl
    {
        SqlConnection cn = new SqlConnection();
        MyConnection db = new MyConnection();
        SqlCommand cm;
        SqlDataReader dr;
        

        public frmTransactions()
        {
            InitializeComponent();
            cn = new SqlConnection(db.MyCon());
            dtpPicker1.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            dtpPicker2.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            dtpPicker1.SelectedDate = DateTime.Now;
            dtpPicker2.SelectedDate = DateTime.Now.AddDays(+1);            

        }

        TransactionDAL tdal = new TransactionDAL();

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = tdal.DisplayAllTransactions();
            gridTransaction.ItemsSource = dt.DefaultView;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            frmReportTrans frm = new frmReportTrans(this);
            frm.LoadReport2();
            frm.ShowDialog();
        }

        private void btnPrintFiltered_Click(object sender, RoutedEventArgs e)
        {
              frmReportTrans frm = new frmReportTrans(this);
              frm.LoadReport();
              frm.ShowDialog(); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Display all the Transactions
             DataTable dt = tdal.DisplayAllTransactions();
             gridTransaction.ItemsSource = dt.DefaultView;
            
        }

        private void cmbTransactions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Get the Value from ComboBox
            string type = (e.AddedItems[0] as ComboBoxItem).Content as string;

            DataTable dt = tdal.DisplayTransactionByType(type);
            gridTransaction.ItemsSource = dt.DefaultView;
       
       }


       public void LoadRecord()
        {
            

            DataTable dt = new DataTable();
            try
            {
                string s1 = dtpPicker1.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string s2 = dtpPicker2.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

                //Write the SQL Query to Display all Transactions
                string sql = "SELECT id [ID], type [Type], DealCustID, grandTotal [Grand Total], transaction_date [Transaction Time], tax [TAX], discount [Discount] FROM tblTransaction WHERE transaction_date between '" + s1 + "' and '" + s2 + "'";

                //SQLCommand to Execute Query
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //SqlDataAdapter to Hold The Data from DataBase
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open Database Connection

                db.con.Open();
                adapter.Fill(dt);
                gridTransaction.ItemsSource = dt.DefaultView;

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

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadRecord();
        }

      
    }
}
