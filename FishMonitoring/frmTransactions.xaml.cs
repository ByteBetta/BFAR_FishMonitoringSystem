using Fish.BLL;
using Google.Cloud.Firestore;
using Project.BLL;
using Project.DAL;
using System;
using System.Collections;
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
using WpfPosApp.DAL;

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for frmTransactions.xaml
    /// </summary>
    public partial class frmTransactions : UserControl
    {
        SqlConnection cn = new SqlConnection();
        MyConnection db = new MyConnection();
        FirestoreDb firestoreDatabase;
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

        TransDetailsDAL tdal = new TransDetailsDAL();
        DealersDAL ddal = new DealersDAL();

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

            DataTable dt = tdal.DisplayAllTransactions();
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
                string sql = "SELECT TransDetID [ID], fisherman [Fisherman], vessels [vessels], added_date [Transaction Time], transno [Transaction Number] FROM TransDetails WHERE added_date between '" + s1 + "' and '" + s2 + "'";

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

        private async void BtnRetrievedData_Click(object sender, RoutedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"bfar-testproj.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            firestoreDatabase = FirestoreDb.Create("bfar-testproj");

            Query allrecords = firestoreDatabase.Collection("records");
            QuerySnapshot allrecordssnapshot = await allrecords.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allrecordssnapshot.Documents)
            {
                
                Console.WriteLine("Document data for {0} document:", documentSnapshot.Id);
                Dictionary<string, object> city = documentSnapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in city)
                {
                    Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                   
                   
                }
                Console.WriteLine("");
            }

            Query allfishdata = firestoreDatabase.Collection("fishdata");
            QuerySnapshot allfishdatalist = await allfishdata.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allfishdatalist.Documents)
            {
                
                
                TransDetailsBLL transDetailsBLL = new TransDetailsBLL();
                transDetailsBLL.quantity = 1;
                Console.WriteLine("Document data for {0} document:", documentSnapshot.Id);
                transDetailsBLL.UID = documentSnapshot.Id;

                string sql = "SELECT COUNT(*) FROM TransDetails WHERE UID = '" + documentSnapshot.Id + "'" ;

                //SQLCommand to Execute Query
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //SqlDataAdapter to Hold The Data from DataBase
                db.con.Open();
                int UserExist = (int)cmd.ExecuteScalar();
                db.con.Close();
                if (UserExist > 0)
                {
                    //Username exist
                }
                else
                {
                    Dictionary<string, object> city = documentSnapshot.ToDictionary();
                    foreach (KeyValuePair<string, object> pair in city)
                    {
                        if (nameof(transDetailsBLL.Species) == pair.Key.ToString())
                        {
                            transDetailsBLL.Species = pair.Value.ToString();
                        }
                        if (nameof(transDetailsBLL.length) == pair.Key.ToString().ToLower())
                        {
                            transDetailsBLL.length = int.Parse(pair.Value.ToString());
                        }
                        if (nameof(transDetailsBLL.weight) == pair.Key.ToString().ToLower())
                        {
                            transDetailsBLL.weight = pair.Value.ToString();
                        }
                        if (nameof(transDetailsBLL.added_date) == pair.Key.ToString())
                        {
                            transDetailsBLL.added_date = Convert.ToDateTime(pair.Value);
                        }
                        if (nameof(transDetailsBLL.fisherman) == pair.Key.ToString())
                        {
                            transDetailsBLL.fisherman = pair.Value.ToString();
                        }
                        if (nameof(transDetailsBLL.vessels) == pair.Key.ToString())
                        {
                            transDetailsBLL.vessels = pair.Value.ToString();
                        }
                        if (nameof(transDetailsBLL.transno) == pair.Key.ToString())
                        {
                            transDetailsBLL.transno = pair.Value.ToString();
                        }
                        if (nameof(transDetailsBLL.gearUsed) == pair.Key.ToString())
                        {
                            transDetailsBLL.gearUsed = pair.Value.ToString();
                        }
                        if (nameof(transDetailsBLL.landingSite) == pair.Key.ToString())
                        {
                            transDetailsBLL.landingSite = pair.Value.ToString();
                        }
                    }

                    TransDetailsDAL tdal = new TransDetailsDAL();
                    tdal.InsertTransDetails(transDetailsBLL);
                    Console.WriteLine("");
                   
                }
                MessageBox.Show("Retrival Success");


            }

          



        }
    }
}
