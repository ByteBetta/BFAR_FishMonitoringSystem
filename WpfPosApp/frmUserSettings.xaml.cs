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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for frmUserSettings.xaml
    /// </summary>
    public partial class frmUserSettings : UserControl
    {
        frmUser u;
        MyConnection db = new MyConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public frmUserSettings(frmUser frm)
        {
            InitializeComponent();
            u = frm;
        }

        private void TxtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                db.con.Open();
                cm = new SqlCommand("select * from Login where UserName=@UserName", db.con);
                cm.Parameters.AddWithValue("@UserName", txtUsername.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    chkBox1.IsChecked = bool.Parse(dr["isActive"].ToString());
                } else
                {
                    chkBox1.IsChecked = false;
                }
                dr.Close();
                db.con.Close();
            }
            catch(Exception ex)
            {
                db.con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            { 

                bool found = true;
                db.con.Open();
                cm = new SqlCommand("select * from Login where UserName=@UserName", db.con);
                cm.Parameters.AddWithValue("@UserName", txtUsername.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows) { found = true; } else { found = false; }
                dr.Close();
                db.con.Close();

                if(found == true)
                { 
                    db.con.Open();
                    cm = new SqlCommand("update Login set isActive = @isActive where UserName = @UserName", db.con);
                    cm.Parameters.AddWithValue("@isActive", chkBox1.IsChecked.ToString());
                    cm.Parameters.AddWithValue("@UserName", txtUsername.Text);
                    cm.ExecuteNonQuery();
                    db.con.Close();
                    System.Windows.Forms.MessageBox.Show("Account status has been succesfully updated.", "Update Status", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    txtUsername.Clear();
                    chkBox1.IsChecked = false;

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Accout not exists!", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                }
            } catch(Exception ex)
            {
                db.con.Close();
                System.Windows.Forms.MessageBox.Show(ex.Message, "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        private void BtnClose_Click(object sender, MouseButtonEventArgs e)
        {
            u.gridUser.Children.Remove(this);

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            u.gridUser.Children.Remove(this);
          
        }
    }
}
