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

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for frmPurchase.xaml
    /// </summary>
    public partial class frmPurchase : System.Windows.Controls.UserControl
    {
        MyConnection db = new MyConnection();
        SqlDataReader dr;


        string imgLoc = "laptop.png";
        string sourcePath = "";
        string destinationPath = "";
        // Global Variabel For The Image To Delte
        string rowHeaderImage;
        frmCashierDashboard cd;
        
        Main ma;

        public frmPurchase(frmCashierDashboard frm, Main frm2)
        {
            InitializeComponent();
            Colors();
            cd = frm;
            ma = frm2;
        }

        DealCustDAL dcDAL = new DealCustDAL();
        DealersDAL dDAL = new DealersDAL();
        ProductDAL pDAL = new ProductDAL();
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
            txtDCEmail.Foreground = bbrush;
            txtDCMobile.Foreground = bbrush;
            txtDCName.Foreground = bbrush;
            txtSurname.Foreground = bbrush;
            txtDCSearch.Foreground = bbrush;
            txtDiscount.Foreground = bbrush;
            txtGrandTotal.Foreground = pbrush;
            txtPaidAmount.Foreground = pbrush;
            txtPDInventory.Foreground = bbrush;
            txtPDName.Foreground = bbrush;
            txtPDPrice.Foreground = bbrush;
            txtPDQuantity.Foreground = bbrush;
            txtPDSearch.Foreground = bbrush;
            txtReturnAmount.Foreground = bbrush;
            txtVAT.Foreground = bbrush;

        }

        private void txtDCSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtDCSearch.Text;

            if (keyword == "")
            {
                txtDCName.Text = "";
                txtDCEmail.Text = "";
                txtDCMobile.Text = "";
                txtDCAddress.Text = "";
                txtSurname.Text = "";


                return;
            }

           
                DealersBLL dc = dDAL.SearchDealerForTransaction(keyword);
                txtDCName.Text = dc.name;
                txtDCEmail.Text = dc.email;
                txtDCMobile.Text = dc.contact;
                txtDCAddress.Text = dc.address;
                txtSurname.Text = dc.person;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Specify Columns for our TransactionDataTable
            transactionDT.Columns.Add("Product Name");
            transactionDT.Columns.Add("Price");
            transactionDT.Columns.Add("Quantity");
            transactionDT.Columns.Add("Total");

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
                    txtPDName.Text = "";
                    txtPDInventory.Text = "";
                    txtPDPrice.Text = "";
                    txtPDQuantity.Text = "";

                    string pathhs = System.Windows.Forms.Application.StartupPath.Substring(0, (System.Windows.Forms.Application.StartupPath.Length - 10));
                    string imagePath = pathhs + "\\Images\\Product\\laptop.png";
                    imgBox.ImageSource = new BitmapImage(new Uri(imagePath));
                    imgLoc = "laptop.png";
                    return;
                }

                //Search the product and display on respective textboxes
                ProductBLL p = pDAL.GetProductsForTransaction(keyword);

                // Set the values on textboxes 
                txtPDName.Text = p.Full_Name;
                txtPDInventory.Text = p.Quantity.ToString();
                txtPDPrice.Text = p.Price.ToString();
                imgLoc = p.Img;
                rowHeaderImage = imgLoc;

                string paths = System.Windows.Forms.Application.StartupPath.Substring(0, (System.Windows.Forms.Application.StartupPath.Length - 10));
                if (imgLoc != "laptop.png")
                {
                    string imagePath = paths + "\\Images\\Product\\" + imgLoc;
                    imgBox.ImageSource = new BitmapImage(new Uri(imagePath));

                }
                else
                {
                    string imagePath = paths + "\\Images\\Product\\laptop.png";
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

                //Get Product Name, Rate and QTY customer wants to buy
                string productName = txtPDName.Text;
                decimal Rate = decimal.Parse(txtPDPrice.Text);
                decimal Qty = decimal.Parse(txtPDQuantity.Text);
                string Img = rowHeaderImage;
                decimal Total = Rate * Qty;//Total=Rate*Qty

                //Display the Subtotal in textbox
                //Get the subtotal value from textbox
                decimal subTotal = decimal.Parse(txtSubTotal.Text);
                subTotal = subTotal + Total;

                //Check wheter the product is selected or not
                if (productName == "")
                {
                    //Display error message
                    System.Windows.MessageBox.Show("Select the product first.");
                }
                else
                {
                    //Add product to the data Grid View
                    transactionDT.Rows.Add(productName, Rate, Qty, Total);

                    //Show in Data Grid View
                    gridAddedProducts.ItemsSource = transactionDT.DefaultView;
                    //Sizing DataGridView Columns Width
                    gridAddedProducts.Columns[0].Width = 130;
                    gridAddedProducts.Columns[1].Width = 90;
                    gridAddedProducts.Columns[2].Width = 83;
                    gridAddedProducts.Columns[3].Width = 100;
                    //Display the subtotal in textbox
                    txtSubTotal.Text = subTotal.ToString();


                    //Clear the TextBoxes
                    txtPDSearch.Text = "";
                    txtPDName.Text = "";
                    txtPDInventory.Text = "0.00";
                    txtPDPrice.Text = "0.00";
                    txtPDQuantity.Text = "0";
                    string paths = System.Windows.Forms.Application.StartupPath.Substring(0, (System.Windows.Forms.Application.StartupPath.Length - 10));

                    string imagePath = paths + "\\Images\\Product\\laptop.png";
                    imgBox.ImageSource = new BitmapImage(new Uri(imagePath));
                    imgLoc = "laptop.png";
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void txtDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //Get the value from discount textbox
                string value = txtDiscount.Text;

                if (value == "")
                {
                    //Display Error Message
                    System.Windows.MessageBox.Show("Please Add Discount First");
                }
                else
                {
                    //Get the discount in decimal value
                    decimal subTotal = decimal.Parse(txtSubTotal.Text);
                    decimal discount = decimal.Parse(txtDiscount.Text);

                    //Calculate the grandtotal based on discount
                    decimal grandTotal = ((100 - discount) / 100) * subTotal;

                    //Display the Grand Total in TextBox
                    txtGrandTotal.Text = grandTotal.ToString();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void txtVAT_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //Check if the grandTotal has value or not then calculate the discount first
                string check = txtGrandTotal.Text;
                if (check == "")
                {
                    //Display the error message to calculate discount
                    System.Windows.MessageBox.Show("Calculate the discount and set the Grand Total First.");
                }
                else
                {
                    //Calculate VAT
                    //Getting the VAT Percent First
                    decimal previousGT = decimal.Parse(txtGrandTotal.Text);
                    decimal vat = decimal.Parse(txtVAT.Text);
                    decimal grandTotalWithVAT = ((100 + vat) / 100) * previousGT;

                    //Display new Grand Total with VAT
                    txtGrandTotal.Text = grandTotalWithVAT.ToString();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void txtPaidAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                //Get the paid amount and grand total
                decimal grandTotal = decimal.Parse(txtGrandTotal.Text);
                decimal paidAmount = decimal.Parse(txtPaidAmount.Text);
                decimal returnAmount = paidAmount - grandTotal;

                //Display the return amount

                if (paidAmount < grandTotal)
                {
                    txtPaidAmount.Foreground = new SolidColorBrush(Color.FromRgb(255, 106, 106));
                }
                else
                {

                    txtPaidAmount.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    txtReturnAmount.Text = returnAmount.ToString();
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Get the Value from PurchaseSales Form 
                TransactionBLL transaction = new TransactionBLL();

                transaction.type = lblTop.Content.ToString();

                //Get the ID of Dealer or Customer HERE
                //Lets get name of the dealer or customer firs
                string deaCustName = txtDCName.Text;
                DealCustBLL dc = dcDAL.GetDealCustIDFromName(deaCustName);

                transaction.DealCustID = dc.DealCustID;
                transaction.grandTotal = Math.Round(decimal.Parse(txtGrandTotal.Text), 2);
                transaction.transaction_date = DateTime.Now;
                transaction.tax = decimal.Parse(txtVAT.Text);
                transaction.discount = decimal.Parse(txtDiscount.Text);

                bool success = false;
                //Actua code to Insert Transaction and Transaction Details
                using (TransactionScope scope = new TransactionScope())
                {
                    int TransactionID = -1;
                    //Create a boolean value and insert transaction
                    bool w = tDAL.Insert_Transaction(transaction, out TransactionID);

                    //Use for loop to insert Transaction Details
                    for (int i = 0; i < transactionDT.Rows.Count; i++)
                    {
                        //Get all the details of the product
                        TransDetailsBLL transactionDetail = new TransDetailsBLL();
                        string ProductName = transactionDT.Rows[i][0].ToString();
                        ProductBLL p = pDAL.GetProductIDFromName(ProductName);

                        transactionDetail.ProdID = p.ProdID;
                        transactionDetail.transno = lblTransNoUnit.Content.ToString();
                        transactionDetail.price = decimal.Parse(transactionDT.Rows[i][1].ToString());
                        transactionDetail.qty = decimal.Parse(transactionDT.Rows[i][2].ToString());
                        transactionDetail.total_price = Math.Round(decimal.Parse(transactionDT.Rows[i][3].ToString()), 2);
                        transactionDetail.type = "Purchase";
                        transactionDetail.DealCustID = dc.DealCustID;
                        transactionDetail.added_date = DateTime.Now;
                        //transactionDetail.added_by = u.id;


                        //Here Increase or Decrase Product Quantity based on Purchase or sales
                        string transactionType = "Purchase";

                        //Lets check whether we are on Purchase or Sales
                        bool x = false;
                        if (transactionType == "Purchase")
                        {
                            //Increase the Product
                            x = pDAL.IncreaseProduct(transactionDetail.ProdID, transactionDetail.qty);
                        }
                        else if (transactionType == "Sale")
                        {
                            //Decrease the Product Quantity
                            x = pDAL.DecraseProduct(transactionDetail.ProdID, transactionDetail.qty);
                        }


                        //Insert Transaction Details inside the database
                        bool y = tdDAL.InsertTransDetails(transactionDetail);

                        success = w && x && y;
                    }

                    if (success == true)
                    {
                        frmRec frm = new frmRec(this);
                        frm.Show();
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
                        txtDCEmail.Text = "";
                        txtDCMobile.Text = "";
                        txtDCAddress.Text = "";
                        txtPDSearch.Text = "";
                        txtPDName.Text = "";
                        txtPDInventory.Text = "0";
                        txtPDPrice.Text = "0";
                        txtPDQuantity.Text = "0";
                        txtSubTotal.Text = "0";
                        txtDiscount.Text = "0";
                        txtVAT.Text = "0";
                        txtGrandTotal.Text = "0";
                        txtPaidAmount.Text = "0";
                        txtReturnAmount.Text = "0";
                        lblTransNoUnit.Content = "0000000000";

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
            }
        }


        System.Windows.Controls.Control ActiveControl;

        private void Btn_Nmb1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActiveControl.Focus();
                var sim = new InputSimulator();
                sim.Keyboard.KeyPress(new VirtualKeyCode[] { VirtualKeyCode.VK_1 });
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }


        private void Btn_Nmb2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActiveControl.Focus();
                var sim = new InputSimulator();
                sim.Keyboard.KeyPress(new VirtualKeyCode[] { VirtualKeyCode.VK_2 });

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Nmb3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActiveControl.Focus();
                var sim = new InputSimulator();
                sim.Keyboard.KeyPress(new VirtualKeyCode[] { VirtualKeyCode.VK_3 });

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Nmb4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActiveControl.Focus();
                var sim = new InputSimulator();
                sim.Keyboard.KeyPress(new VirtualKeyCode[] { VirtualKeyCode.VK_4 });

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Nmb5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActiveControl.Focus();
                var sim = new InputSimulator();
                sim.Keyboard.KeyPress(new VirtualKeyCode[] { VirtualKeyCode.VK_5 });

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Nmb6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActiveControl.Focus();
                var sim = new InputSimulator();
                sim.Keyboard.KeyPress(new VirtualKeyCode[] { VirtualKeyCode.VK_6 });

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Nmb7_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActiveControl.Focus();
                var sim = new InputSimulator();
                sim.Keyboard.KeyPress(new VirtualKeyCode[] { VirtualKeyCode.VK_7 });

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Nmb8_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActiveControl.Focus();
                var sim = new InputSimulator();
                sim.Keyboard.KeyPress(new VirtualKeyCode[] { VirtualKeyCode.VK_8 });

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Nmb9_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActiveControl.Focus();
                var sim = new InputSimulator();
                sim.Keyboard.KeyPress(new VirtualKeyCode[] { VirtualKeyCode.VK_9 });

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Nmb0_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActiveControl.Focus();
                var sim = new InputSimulator();
                sim.Keyboard.KeyPress(new VirtualKeyCode[] { VirtualKeyCode.VK_0 });

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Btn_NmbDOT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActiveControl.Focus();
                var sim = new InputSimulator();
                sim.Keyboard.KeyPress(new VirtualKeyCode[] { VirtualKeyCode.OEM_COMMA });


            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Nmb12_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtDiscount.Text = "";
                txtVAT.Text = "";
                txtPaidAmount.Text = "";
                txtGrandTotal.Text = "";
                txtPaidAmount.Text = "";
                txtReturnAmount.Text = "";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }



        private void txtPaidAmount_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ActiveControl = (System.Windows.Controls.Control)sender;

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
            txtDCEmail.Text = "";
            txtDCMobile.Text = "";
            txtDCAddress.Text = "";
            txtPDSearch.Text = "";
            txtPDName.Text = "";
            txtPDInventory.Text = "0";
            txtPDPrice.Text = "0";
            txtPDQuantity.Text = "0";
            txtSubTotal.Text = "0";
            txtDiscount.Text = "0";
            txtVAT.Text = "0";
            txtGrandTotal.Text = "0";
            txtPaidAmount.Text = "0";
            txtReturnAmount.Text = "0";

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


    }
}
