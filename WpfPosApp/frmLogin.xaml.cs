using Project.BLL;
using Project.DAL;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for frmLogin.xaml
    /// </summary>
    public partial class frmLogin : Window
    {
        MyConnection db = new MyConnection();
        SqlCommand cm;
        SqlDataReader rd;
        loginBLL l = new loginBLL();
        loginDAL dal = new loginDAL();


        public static string loggedIn;
        public static string _name;
        public static string _surname;
        public static string _password;
        public static string _gender;
        public static string _dob;
        public static int _id;
        public static string _img;
        public static string _usertype;
        public bool _isActive = false;

        public bool result;


        public frmLogin()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string _usernm = "", _role = "", _name = "";
                bool found = false;
                db.con.Open();
                cm = new SqlCommand("Select * from Login where username = @username and password = @password", db.con);
                cm.Parameters.AddWithValue("@username", txtUsername.Text);
                cm.Parameters.AddWithValue("@password", txtPassword.Password);
                rd = cm.ExecuteReader();
                rd.Read();
                if (rd.HasRows)
                {
                    found = true;
                    _usernm = rd["username"].ToString();
                    _role = rd["UserType"].ToString();
                    _name = rd["Name"].ToString();
                    loggedIn = txtUsername.Text;
                    _name = rd["Name"].ToString();
                    _surname = rd["Surname"].ToString();
                    _password = rd["Password"].ToString();
                    _gender = rd["SEX"].ToString();
                    _dob = rd["Birth_Date"].ToString();
                    _img = rd["Img"].ToString();
                    _usertype = rd["UserType"].ToString();
                    string loggedUsr = frmLogin.loggedIn;
                    loginBLL usr = dal.GetIDFromUsername(loggedUsr);
                    _id = usr.UserID;
                    _isActive = bool.Parse(rd["isActive"].ToString());

                }
                else
                {
                    found = false;
                }
                rd.Close();
                db.con.Close();

                if (found == true)
                {
                    if (_isActive == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Account is inactive. Unable to Login", "Inactive Account", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        return;
                    }
                    if (_role == "Admin")
                    {

                        Main m = new Main();
                        m.Show();
                        this.Close();
                    }
                    else
                    {
                        frmCashierDashboard c = new frmCashierDashboard();
                        c.Show();
                        this.Close();
                    }

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Username or Password is incorrect.");
                }
            }
            catch (Exception ex)
            {
                db.con.Close();
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

 
        private void usernameGotFocus(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == "Username")
            {
                txtUsername.Clear();
            }
        }

        private void usernameLostFocus(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == null || txtUsername.Text == "")
            {
                txtUsername.Text = "Username";
            }
        }

        private void passwordFocus(object sender, RoutedEventArgs e)
        {
            
        }
    }

   

}


