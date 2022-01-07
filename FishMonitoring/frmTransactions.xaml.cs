using Dapper;
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
using WpfPosApp.BLL;
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
            dtpPicker1.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            dtpPicker2.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            dtpPicker1.SelectedDate = DateTime.Now;
            dtpPicker2.SelectedDate = DateTime.Now.AddDays(+1);

            List<string> userdata = new List<string> { "All Users" };
            db.con.Open();
            string cmd = "Select UserName from Login";
            IList<UserBLL> userholder = db.con.Query<UserBLL>(cmd).ToList();
            db.con.Close();

            foreach (var user in userholder)
            {
                userdata.Add(user.UserName);
            }

            cmbUser.ItemsSource = userdata;

        }

        TransactionDAL tdal = new TransactionDAL();
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


       public void LoadRecord(string user)
        {
            

            DataTable dt = new DataTable();
            try
            {
                string s1 = dtpPicker1.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string s2 = dtpPicker2.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

                //Write the SQL Query to Display all Transactions
                string sql = "SELECT " +
                    " tblTransaction.transno, tblTransaction.totalBox, Login.UserName, Login.Name as firstname, Login.Surname as lastname, " +
                    " Login.UserType, tblTransaction.fisherman, tblTransaction.transaction_date, tblTransaction.vessels, tblTransaction.gearUsed," +
                    " tblTransaction.landingSite, tblTransaction.totalSampleBox, tblTransaction.totalWeightBox, tblTransaction.totalSampleWeightBox " +
                    " FROM tblTransaction INNER JOIN Login ON tblTransaction.added_by = Login.UserID " +
                " WHERE tblTransaction.transaction_date between '" + s1 + "' and '" + s2 + "'" +
                user.ToString();

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
            if(dtpPicker1.Text == String.Empty || dtpPicker2.Text == String.Empty || cmbUser.Text == String.Empty)
            {
                System.Windows.Forms.MessageBox.Show("Please Select Items From The DropDown List.", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            } else
            {
                string holder = "";
                if (cmbUser.Text != "All Users")
                {
                    holder = " AND Login.UserName = '" + cmbUser.Text + "'";
                    LoadRecord(holder);
                }
                
            }
            
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
                string sql = "SELECT COUNT(*) FROM tblTransaction WHERE transno = '" + documentSnapshot.Id + "'";

                //SQLCommand to Execute Query
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //SqlDataAdapter to Hold The Data from DataBase
                db.con.Open();
                int idExist = (int)cmd.ExecuteScalar();
                db.con.Close();
                if (idExist > 0)
                {

                }
                else
                {
                    TransactionBLL tbll = new TransactionBLL();
                    tbll.transno = documentSnapshot.Id;
                    tbll.remark = "";
                    Console.WriteLine("Document data for {0} document:", documentSnapshot.Id);
                    Dictionary<string, object> city = documentSnapshot.ToDictionary();
                    foreach (KeyValuePair<string, object> pair in city)
                    {
                        if (nameof(tbll.added_by) == pair.Key.ToString().ToLower())
                        {
                            tbll.added_by = int.Parse(pair.Value.ToString());
                        }
                        if ("Boat" == pair.Key.ToString())
                        {
                            tbll.vessels = pair.Value.ToString();
                        }
                        if ("Date" == pair.Key.ToString())
                        {
                            tbll.transactiondate = Convert.ToDateTime(pair.Value.ToString());
                        }
                        if ("Fisherman" == pair.Key.ToString())
                        {
                            tbll.fisherman = pair.Value.ToString();
                        }
                        if ("GearUsed" == pair.Key.ToString())
                        {
                            tbll.gearUsed = pair.Value.ToString();
                        }
                        if ("LandingSite" == pair.Key.ToString())
                        {
                            tbll.landingSite = pair.Value.ToString();
                        }
                        if ("NumberofBoxes" == pair.Key.ToString())
                        {
                            tbll.totalBox = Convert.ToDecimal(pair.Value.ToString());
                        }
                        if ("NumberofBoxesSample" == pair.Key.ToString())
                        {
                            tbll.totalSampleBox = Convert.ToDecimal(pair.Value.ToString());
                        }
                        if ("TotalSampleWeight" == pair.Key.ToString())
                        {
                            tbll.totalSampleWeightBox = Convert.ToDecimal(pair.Value.ToString());
                        }
                        if ("TotalWeight" == pair.Key.ToString())
                        {
                            tbll.totalWeightBox = Convert.ToDecimal(pair.Value.ToString());
                        }
                    }

                    TransactionDAL ttdal = new TransactionDAL();
                    ttdal.Insert_Transaction(tbll);
                    Console.WriteLine("");

                }
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
               


            }

            MessageBox.Show("Retrival Success");





        }

        private void GridTransaction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid gd = (DataGrid)sender;
                DataRowView row_selected = gd.SelectedItem as DataRowView;
                if (row_selected != null)
                {
                    Console.WriteLine(row_selected["transno"].ToString());
                }
            }
            catch (Exception es)
            {
                MessageBox.Show(es.ToString());
            }
        }
    }
}
