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
using WpfPosApp.BLL;
using WpfPosApp.DAL;

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for frmUserChangePassword.xaml
    /// </summary>
    public partial class frmUserChangePassword : UserControl
    {
        frmProfile p;
        UserBLL u = new UserBLL();
        userDAL dal = new userDAL();
        MyConnection db = new MyConnection();
        SqlCommand cm;



        public frmUserChangePassword(frmProfile frm)
        {
            InitializeComponent();
            p = frm;
        }     

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            p.profileGrid.Children.Remove(this);

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lblUserID.Content = frmLogin._id;
            txtUsername.Text = frmLogin.loggedIn; 

        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtPasswordOld.Password != frmLogin._password)
                {
                    System.Windows.Forms.MessageBox.Show("Old Password Didn't Matched!", "Invalid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    return;
                }
                if (txtPasswordNew.Password != txtPasswordNewRe.Password)
                {
                    System.Windows.Forms.MessageBox.Show("Confirm New Password", "Invalid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    return;
                }
                
                db.con.Open();
                cm = new SqlCommand("update Login set  Password=@Password where UserID = @UserID", db.con);
                cm.Parameters.AddWithValue("@Password", txtPasswordNew.Password);
                cm.Parameters.AddWithValue("@UserID", lblUserID.Content);
                cm.ExecuteNonQuery();
                db.con.Close();
                System.Windows.Forms.MessageBox.Show("Password has been Successfully changed!", "Changed Password", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                txtPasswordOld.Clear();
                txtPasswordNew.Clear();
                txtPasswordNewRe.Clear();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning); 
            }
        }

        private void BtnClose_Click(object sender, MouseButtonEventArgs e)
        {
            p.profileGrid.Children.Remove(this);
        }
    }
}
