using Project.BLL;
using Project.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WindowsInput;
using WindowsInput.Native;
using WpfPosApp.BLL;
using WpfPosApp.DAL;
using MessageBox = System.Windows.Forms.MessageBox;
using Fish.BLL;
using Fish.DAL;

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for frmAddTransaction.xaml
    /// </summary>
    public partial class frmAddTransaction : System.Windows.Controls.UserControl
    {
        MyConnection db = new MyConnection();
        SqlDataReader dr;


        string imgLoc = "Fish.png";
        string sourcePath = "";
        string destinationPath = "";
        // Global Variabel For The Image To Delte
        string rowHeaderImage;
        frmCashierDashboard cd;
        
        Main ma;

        public frmAddTransaction(frmCashierDashboard frm, Main frm2)
        {
            InitializeComponent();
            Colors();
            cd = frm;
            ma = frm2;
        }

        DealCustDAL dcDAL = new DealCustDAL();
        DealersDAL dDAL = new DealersDAL();
        FishDAL fishdal = new FishDAL();
        loginDAL uDAL = new loginDAL();
        TransactionDAL tDAL = new TransactionDAL();
        TransDetailsDAL tdDAL = new TransDetailsDAL();
        DataTable transactionDT = new DataTable();



        public void Colors()
        {
            SolidColorBrush bbrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            SolidColorBrush pbrush = new SolidColorBrush(Color.FromRgb(94, 137, 251));
            SolidColorBrush wbrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            txtDCAddress.Foreground = bbrush;
            txtCompanyName.Foreground = bbrush;
           
            txtDCName.Foreground = bbrush;
            txtSurname.Foreground = bbrush;
            txtDCSearch.Foreground = bbrush;
            txtFWeight.Foreground = bbrush;
            txtFName.Foreground = bbrush;
            txtFSalinity.Foreground = bbrush;
            txtFQuantity.Foreground = bbrush;
            txtPDSearch.Foreground = bbrush;

        }

        private void txtDCSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtDCSearch.Text;

            if (keyword == "")
            {
                txtDCName.Text = "";
                txtCompanyName.Text = "";
             
                txtDCAddress.Text = "";
                txtSurname.Text = "";


                return;
            }

           
                DealersBLL dc = dDAL.SearchDealerForTransaction(keyword);
                txtDCName.Text = dc.name;
            txtCompanyName.Text = dc.email;
                cmbOwnerList.Text = dc.contact;
                txtDCAddress.Text = dc.address;
                txtSurname.Text = dc.person;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            transactionDT.Columns.Add("Fish Name");
            transactionDT.Columns.Add("Quantity");
            transactionDT.Columns.Add("Weight");

            Console.WriteLine(gridAddedProducts.Columns.Count);
            //Specify Columns for our TransactionDataTable
            if (dDAL.GetVesselFromFisherman(txtCompanyName.Text) != null)
            {
                DataTable ownerlist = dDAL.GetVesselFromFisherman(txtCompanyName.Text);
                //Specify DataSource for Category ComboBox
                cmbOwnerList.ItemsSource = ownerlist.DefaultView;
                //Specify Display Member and Value Member for Combobox
                cmbOwnerList.DisplayMemberPath = "name";
                cmbOwnerList.SelectedValuePath = "name";
            }

            // tmrClock.Start();
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            lblDate.Content = DateTime.Now.ToLongDateString();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                lblTime.Content = DateTime.Now.ToString("T");
            }));
        }

        private void txtPDSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                string keyword = txtPDSearch.Text;

                if (keyword == "")
                {
                    txtFName.Text = "";
                    txtFWeight.Text = "";
                    txtFSalinity.Text = "";
                    txtFQuantity.Text = "";

                    string pathhs = System.Windows.Forms.Application.StartupPath.Substring(0, (System.Windows.Forms.Application.StartupPath.Length - 10));
                    string imagePath = pathhs + "\\Images\\Product\\Fish.png";
                    imgBox.ImageSource = new BitmapImage(new Uri(imagePath));
                    imgLoc = "fish.png";
                    return;
                }

                //Search the Fish and display on respective textboxes
                FishBLL fish = fishdal.GetProductsForTransaction(keyword);

                // Set the values on textboxes 
                //txtFName.Text = fish.FishName.ToString();
                //txtFSalinity.Text = fish.Salinity.ToString();
                imgLoc = fish.Img;
                rowHeaderImage = imgLoc;

                string paths = System.Windows.Forms.Application.StartupPath.Substring(0, (System.Windows.Forms.Application.StartupPath.Length - 10));
                if (imgLoc != "Fish.png")
                {
                    string imagePath = paths + "\\Images\\Product\\" + imgLoc;
                    imgBox.ImageSource = new BitmapImage(new Uri(imagePath));

                }
                else
                {
                    string imagePath = paths + "\\Images\\Product\\Fish.png";
                    imgBox.ImageSource = new BitmapImage(new Uri(imagePath));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnADD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetTransNo();

                //Get Fish Name, FishWeight and FishQty from Fisherman
                string Fishname = txtFName.Text;
                decimal FishWeight = decimal.Parse(txtFWeight.Text);
                decimal FishQty = int.Parse(txtFQuantity.Text);
                string Img = rowHeaderImage;
              

      
 
                //Check Wether the Fish is selected or not
                if (Fishname == "")
                {
                    //Display error message
                    System.Windows.MessageBox.Show("Select the Fish first.");
                }
                else
                {
                    //Add Fish to the data Grid View
                    transactionDT.Rows.Add(Fishname, FishWeight, FishQty);

                    //Show in Data Grid View
                    gridAddedProducts.ItemsSource = transactionDT.DefaultView;
                    //Sizing DataGridView Columns Width
                    gridAddedProducts.Columns[0].Width = 130;
                    gridAddedProducts.Columns[1].Width = 90;
                    gridAddedProducts.Columns[2].Width = 83;


                    //Clear the TextBoxes
                    txtPDSearch.Text = "";
                    txtFName.Text = "";
                    txtFWeight.Text = "0.00";
                    txtFSalinity.Text = "";
                    txtFQuantity.Text = "0";
                    string paths = System.Windows.Forms.Application.StartupPath.Substring(0, (System.Windows.Forms.Application.StartupPath.Length - 10));

                    string imagePath = paths + "\\Images\\Product\\fish.png";
                    imgBox.ImageSource = new BitmapImage(new Uri(imagePath));
                    imgLoc = "fish.png";
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void txtDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }

        private void txtVAT_TextChanged(object sender, TextChangedEventArgs e)
        {
         
        }

        private void txtPaidAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
          
               
        }

        private void btnProceed_Cick(object sender, RoutedEventArgs e)
        {
           try
             {
                Console.WriteLine(DateTime.Now.ToString());
                //Get the Value from PurchaseSales Form 
                TransactionBLL transaction = new TransactionBLL();

             

                //Get the ID of Dealer or Customer HERE
                //Lets get name of the dealer or customer firs
                string dealersName = txtDCName.Text;
                DealersBLL dc = dDAL.getIDofFisherman(dealersName);

               // transaction.DealID = dc.DealID;
                transaction.transactiondate = DateTime.Now;
                transaction.transno = lblTransNoUnit.Content.ToString();
                transaction.remark = txtRemark.ToString();
                bool success = false;

                //Actua code to Insert Transaction and Transaction Details
                using (TransactionScope scope = new TransactionScope())
                {
                    int TransactionID = -1;
                    //Create a boolean value and insert transaction
                    //bool w = tDAL.Insert_Transaction(transaction, out TransactionID);

                    //Use for loop to insert Transaction Details
                    for (int i = 0; i < transactionDT.Rows.Count; i++)
                    {
                        //Get all the details of the product
                        TransDetailsBLL transactionDetail = new TransDetailsBLL();
                        string FishName = transactionDT.Rows[i][0].ToString();
                        FishBLL fishdata = fishdal.GetFishIDFromName(FishName);

                        transactionDetail.FishID = fishdata.FishID;
                        transactionDetail.transno = lblTransNoUnit.Content.ToString();
                        transactionDetail.weight = transactionDT.Rows[i][1].ToString();
                        transactionDetail.length = decimal.Parse(transactionDT.Rows[i][2].ToString());
                        transactionDetail.DealID = dc.DealID;
                        transactionDetail.added_date = DateTime.Now;
                        //transactionDetail.added_by = u.id;


                    


                        //Insert Transaction Details inside the database
                        bool y = tdDAL.InsertTransDetails(transactionDetail);

                      //  success = w && y;
                    }

                    if (success == true)
                    {
                        //frmRec frm = new frmRec(this);
                       // frm.Show();
                        //Transaction Complete
                        scope.Complete();

                        System.Windows.MessageBox.Show("Transaction Completed Succesfully");


                        //Clear the Data Grid Viw and Clear all the Textboxes
                        gridAddedProducts.ItemsSource = null;
                        gridAddedProducts.Items.Clear();
                        transactionDT.Rows.Clear();

                        txtDCSearch.Text = "";
                        txtDCName.Text = "";
                        txtSurname.Text = "";
                        txtCompanyName.Text = "";
                        cmbOwnerList.Text = "";
                        txtDCAddress.Text = "";
                        txtPDSearch.Text = "";
                        txtFName.Text = "";
                        txtFWeight.Text = "";
                        txtFSalinity.Text = "";
                        txtFQuantity.Text = "";
         

                    }
                    else
                    {
                        //Transaction Failed
                        System.Windows.MessageBox.Show("Transaction Failed");
                    }


                }

            }
           catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                   Console.WriteLine("Error in FrmMain");
            } 
        }



        private void btnDiscard_Click(object sender, RoutedEventArgs e)
        {
            // Discard a Transaction
            gridAddedProducts.ItemsSource = null;
            gridAddedProducts.Items.Clear();
            transactionDT.Rows.Clear();


            txtDCSearch.Text = "";
            txtDCName.Text = "";
            txtSurname.Text = "";
            txtCompanyName.Text = "";
            cmbOwnerList.Text = "";
            txtDCAddress.Text = "";
            txtPDSearch.Text = "";

            txtFName.Text = "";
            txtFWeight.Text = "0";
            txtFSalinity.Text = "";
            txtFQuantity.Text = "0";

        }


        private void GetTransNo()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                string transno;
                int count;
                db.con.Open();
                SqlCommand cm = new SqlCommand("select top 1 transno from TransDetails where transno like '" + sdate + "%' order by TransDetID desc", db.con);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lblTransNoUnit.Content = sdate + (count + 1);
                }
                else
                {
                    transno = sdate + "1001";
                    lblTransNoUnit.Content = transno;
                }
                dr.Close();
                db.con.Close();



            }
            catch (Exception ex)
            {
                db.con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtCompanyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dDAL.GetVesselFromFisherman(txtCompanyName.Text) != null)
            {
                DataTable ownerlist = dDAL.GetVesselFromFisherman(txtDCName.Text);
                //Specify DataSource for Category ComboBox
                cmbOwnerList.ItemsSource = ownerlist.DefaultView;
                //Specify Display Member and Value Member for Combobox
                cmbOwnerList.DisplayMemberPath = "VesselName";
                cmbOwnerList.SelectedValuePath = "VesselName";
            } else
            {
                cmbOwnerList.Items.Clear();
            }
        }
    }
}
