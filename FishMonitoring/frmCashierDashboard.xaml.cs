using System;
using System.Collections.Generic;
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
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Data.SqlClient;
using WpfPosApp;
using System.Data;

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for Mainwindow.xaml
    /// </summary>
    public partial class frmCashierDashboard : Window
    {
        public SqlConnection con;
        SqlConnection cn = new SqlConnection();
        MyConnection dbcon = new MyConnection();
        SqlCommand cm = new SqlCommand();
        frmSettingsM m;
        Main ma;
        int UserID = 0;
        string imgLoc = "user.png";

        public static string transactionType;


        public frmCashierDashboard()
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


        private void BtnCtg_Click(object sender, RoutedEventArgs e)
        {
            frmCategories categories = new frmCategories();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(categories);

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblloggedUsr.Content = "Hi, " + frmLogin.loggedIn;
            lbllogged.Content = "Hi, " + frmLogin.loggedIn;
            lblHeyLogged.Content = frmLogin.loggedIn;


            //Loads User's saved settings
            frmSettings ss = new frmSettings(this);
           ss.btnTheme.IsChecked = Properties.Settings.Default.UCheckBox;
           ss.btnBtheme.IsChecked = Properties.Settings.Default.UCheckBox2;
           ss.rb2.IsChecked = Properties.Settings.Default.UpnlCheck2;
           ss.rbwhite.IsChecked = Properties.Settings.Default.UpnlCheck;

           frmCshdb cs = new frmCshdb();
           pnlMain.Children.Clear();
           pnlMain.Children.Add(cs);
        }


        private void btnPurchase_Click(object sender, MouseButtonEventArgs e)
        {
            frmPurchase purchase = new frmPurchase(this, ma);
            pnlMain.Children.Clear();
            pnlMain.Children.Add(purchase);
        }

        private void btnSale_Click(object sender, RoutedEventArgs e)
        {
            frmSale sale = new frmSale(this, ma);
            pnlMain.Children.Clear();
            pnlMain.Children.Add(sale);
        }

       

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            frmSettings set = new frmSettings(this);
            pnlMain.Children.Clear();
            pnlMain.Children.Add(set);
        }

        private void btnMain_Click(object sender, MouseButtonEventArgs e)
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

        private void btnCustomers(object sender, MouseButtonEventArgs e)
        {
            frmDealersandCustomers dc = new frmDealersandCustomers();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(dc);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            usrBarcode bar = new usrBarcode();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(bar);
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
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void BtnPurchase_Click(object sender, RoutedEventArgs e)
        {
            frmPurchase purchase = new frmPurchase(this, ma);
            pnlMain.Children.Clear();
            pnlMain.Children.Add(purchase);
        }

        private void BtnSale_Click_1(object sender, RoutedEventArgs e)
        {
            frmSale sale = new frmSale(this, ma);
            pnlMain.Children.Clear();
            pnlMain.Children.Add(sale);
        }

        private void Button_Click(object sender, MouseButtonEventArgs e)
        {
            usrBarcode bar = new usrBarcode();
            pnlMain.Children.Clear();
            pnlMain.Children.Add(bar);
        }
    }
}
