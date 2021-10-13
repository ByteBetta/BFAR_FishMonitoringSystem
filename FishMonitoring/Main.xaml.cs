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
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Data;
using System.IO;

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        frmCashierDashboard d;
        public SqlConnection con;
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        MyConnection dbcon = new MyConnection();
        SqlDataReader dr;
         int UserID = 0;
        frmTopItemsChart ch;
        frmCshdb c;
        frmSettingsM m;
        frmCashierDashboard cash;
        frmSale sal;
        string imgLoc = "user.png";

        public static string transactionType;

        public Main()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyCon());
            getImage();
          
        }
      

        private void BtnPopUpLogout_Click(object sender, RoutedEventArgs e)
        {
            frmLogin objLogOut = new frmLogin();
            objLogOut.Show();
            this.Close();

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void BtnOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            btnOpenMenu.Visibility = Visibility.Collapsed;
            btnCloseMenu.Visibility = Visibility.Visible;
            logoBorder.Visibility = Visibility.Visible;
        }

        private void BtnCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            btnOpenMenu.Visibility = Visibility.Visible;
            btnCloseMenu.Visibility = Visibility.Collapsed;
            logoBorder.Visibility = Visibility.Collapsed;

        }




        private void frmUser_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frmUser user = new frmUser();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(user); 
            
        }


        private void frmFishDetails_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frmFishDetails frmFish = new frmFishDetails();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(frmFish);            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblloggedUsr.Content = "Hi, " +  frmLogin.loggedIn;
            lbllogged.Content = "Hi, " +  frmLogin.loggedIn;
            lblHeyLogged.Content = frmLogin.loggedIn;

            //Loads User's saved settings
            frmSettingsM ss = new frmSettingsM(this);
            ss.btnTheme.IsChecked = Properties.Settings.Default.CheckBox;
            ss.rb2.IsChecked = Properties.Settings.Default.pnlCheck2;
            ss.rbwhite.IsChecked = Properties.Settings.Default.pnlCheck;
            ss.btnBTheme.IsChecked = Properties.Settings.Default.CheckBox2;

             frmCshdb cs = new frmCshdb();
             pnlMain.Children.Clear();
             pnlMain.Children.Add(cs); 


       
        }

        private void frmEmployee_Click(object sender, MouseButtonEventArgs e)
        {
            frmEmployee employees = new frmEmployee();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(employees);
        }

        private void BtnCtg_Click(object sender, MouseButtonEventArgs e)
        {
            frmCategories categories = new frmCategories();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(categories);
        }

        private void frmDealers_Click(object sender, MouseButtonEventArgs e)
        {
            frmDealers deal = new frmDealers();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(deal);
        }


        private void frmCustomers_Click(object sender, MouseButtonEventArgs e)
        {
            frmVessels frmVessels = new frmVessels();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(frmVessels);
        }

        private void frmInventory_Click(object sender, MouseButtonEventArgs e)
        {
            frmInventory i = new frmInventory();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(i);
        }

        private void frmTransactions_Click(object sender, MouseButtonEventArgs e)
        {
            frmTransactions t = new frmTransactions();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(t);
        }

        
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            frmSettingsM ss = new frmSettingsM(this);
            pnlMain.Children.Clear();
            pnlMain.Children.Add(ss);
        }

        private void btnDashboard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frmCshdb csd = new frmCshdb();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(csd);
            
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            frmProfile prof = new frmProfile();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(prof);
        }


        private void getImage()
        {
            try
            {
                UserID = frmLogin._id;
                label1.Content = UserID;

                string sql = "SELECT Img From Login where UserID = " + label1.Content + "";

                if (cn.State != ConnectionState.Open)
                    cn.Open();
                cm = new SqlCommand(sql, cn);
                SqlDataReader rd = cm.ExecuteReader();
                rd.Read();
                if (rd.HasRows)
                {
                    string Immg = (string)(rd[0]);
                    byte[] Img = System.Text.Encoding.ASCII.GetBytes(Immg);

                    imgLoc = Immg;

                    string paths = System.Windows.Forms.Application.StartupPath.Substring(0, (System.Windows.Forms.Application.StartupPath.Length - 10));
                    if (imgLoc != "user.png")
                    {
                        string imagePath = paths + "\\Images\\" + imgLoc;
                        imageBox.ImageSource = new BitmapImage(new Uri(imagePath));
                        imageBox1.ImageSource = new BitmapImage(new Uri(imagePath));
                        imageBox2.ImageSource = new BitmapImage(new Uri(imagePath));
                    }
                    else
                    {
                        string imagePath = paths + "\\Images\\user.png";
                        imageBox.ImageSource = new BitmapImage(new Uri(imagePath));
                        imageBox1.ImageSource = new BitmapImage(new Uri(imagePath));
                        imageBox2.ImageSource = new BitmapImage(new Uri(imagePath));
                    }

                    cn.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
                
        }

        private void btnRecord_Click(object sender, MouseButtonEventArgs e)
        {
            frmRecords rec = new frmRecords();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(rec);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            usrBarcode bar = new usrBarcode();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(bar);
        }

        private void BtnPurchase_Click(object sender, RoutedEventArgs e)
        {
            frmPurchase purchase = new frmPurchase(cash, this);
            pnlMain.Children.Clear();
            pnlMain.Children.Add(purchase); 
        }

        private void BtnSale_Click(object sender, RoutedEventArgs e)
        {
            frmSale sale = new frmSale(cash, this);
            pnlMain.Children.Clear();
            pnlMain.Children.Add(sale);
        }

        private void Button_Click(object sender, MouseButtonEventArgs e)
        {
            usrBarcode bar = new usrBarcode();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(bar);
        }

        private void BtnDashboard_Selected(object sender, RoutedEventArgs e)
        {
            frmCshdb csd = new frmCshdb();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(csd);
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            frmUser user = new frmUser();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(user);
        }

        private void ListViewItem_Selected_1(object sender, RoutedEventArgs e)
        {
            frmFishDetails frmFish = new frmFishDetails();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(frmFish);
        }

        private void ListViewItem_Selected_2(object sender, RoutedEventArgs e)
        {
            frmVessels frmVessels = new frmVessels();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(frmVessels);
           
        }
    }
}
