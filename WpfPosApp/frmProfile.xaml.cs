using Project.BLL;
using Project.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
using System.Drawing;
using WpfPosApp.BLL;
using WpfPosApp.DAL;

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for frmProfile.xaml
    /// </summary>
    public partial class frmProfile : UserControl
    {
        MyConnection db = new MyConnection();

        SqlCommand cmd;
        SqlDataAdapter adapt;
        //ID variable used in Updating and Deleting Record 
        int UserID = 0;
        string imgLoc = "user.png";

        UserBLL u = new UserBLL();
        userDAL dal = new userDAL();

        public frmProfile()
        {
            InitializeComponent();
            Colors();
            showdataGender();
            DisplayData();
            getimage();

        }

        public void Colors()
        {
            SolidColorBrush bbrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            SolidColorBrush wbrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));

            txtFirstName.Foreground = bbrush;
            txtLastName.Foreground = bbrush;
            txtUsername.Foreground = bbrush;
            cmbSex.Foreground = wbrush;
            dtpBirth.Foreground = wbrush;
        }

        private void getimage()
        {
            try
            {
                u.UserID = frmLogin._id;
                label1.Content = u.UserID;

                string sql = "SELECT Img From Login where UserID = " + label1.Content + "";

                if (db.con.State != ConnectionState.Open)
                    db.con.Open();
                cmd = new SqlCommand(sql, db.con);
                SqlDataReader rd = cmd.ExecuteReader();
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

                    }
                    else
                    {
                        string imagePath = paths + "\\Images\\user.png";
                        imageBox.ImageSource = new BitmapImage(new Uri(imagePath));
                    }

                    db.con.Close();
                }
                else
                {
                    db.con.Close();
                    MessageBox.Show("This ID Doesn't exits!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void showdataGender()
        {

            db.con.Open();
            cmd = new SqlCommand("Select GenderID,GenderType from Gender", db.con);
            adapt = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapt.Fill(dt);
            db.con.Close();

            DataRow Filaa = dt.NewRow();
            Filaa["GenderType"] = "Select Gender";
            cmbSex.DisplayMemberPath = "GenderType";
            cmbSex.SelectedValuePath = "GenderID";
            cmbSex.ItemsSource = dt.DefaultView;
            db.con.Close();
        }

        private void DisplayData()
        {

            db.con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select * from Login", db.con);
            adapt.Fill(dt);
            //grid_User.ItemsSource = dt.DefaultView;
            db.con.Close(); 
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            label1.Content = frmLogin._id;
            imgLoc = frmLogin._img;

            db.con.Open();
            string sqlquery = "SELECT Name, Surname, UserName, SEX, Birth_Date FROM Login WHERE UserID = " + label1.Content;

            SqlCommand command = new SqlCommand(sqlquery, db.con);

            SqlDataReader sdr = command.ExecuteReader();

            while (sdr.Read())
            {
                txtFirstName.Text = sdr["Name"].ToString();
                txtLastName.Text = sdr["Surname"].ToString();
                txtUsername.Text = sdr["UserName"].ToString();
                cmbSex.Text = sdr["SEX"].ToString();
                dtpBirth.Text = sdr["Birth_Date"].ToString();
            }
            db.con.Close();
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                db.con.Open();
                cmd = new SqlCommand("update Login set Name=@Name, Surname=@Surname, UserName=@UserName, SEX=@SEX, Birth_Date=@Birth_Date, Img=@Img, Added_Date=@Added_Date  where UserID = @UserID", db.con);
                cmd.Parameters.AddWithValue("@UserID", label1.Content);
                cmd.Parameters.AddWithValue("@Name", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@Surname", txtLastName.Text);
                cmd.Parameters.AddWithValue("@UserName", txtUsername.Text);
                cmd.Parameters.AddWithValue("@SEX", cmbSex.Text);
                cmd.Parameters.AddWithValue("@Birth_Date", dtpBirth.Text);
                cmd.Parameters.AddWithValue("@Img", imgLoc);
                cmd.Parameters.AddWithValue("@Added_Date", DateTime.Now);


                cmd.ExecuteNonQuery();
                db.con.Close();
                System.Windows.Forms.MessageBox.Show("Your Profile Information Has Been Successfully Changed!", "Changed Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();

                open.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.PNG; *gifs;)|*.jpg; *.jpeg; *.png; *.PNG; *gifs";

                if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (open.CheckFileExists)
                    {
                        imageBox.ImageSource = new BitmapImage(new Uri(open.FileName));

                        string ext = System.IO.Path.GetExtension(open.FileName);

                        Random random = new Random();
                        int RandInt = random.Next(0, 1000);

                        imgLoc = "POS_USER" + RandInt + ext;

                        string sourcePath = open.FileName;

                        string paths = System.Windows.Forms.Application.StartupPath.Substring(0, System.Windows.Forms.Application.StartupPath.Length - 10);

                        string destinationPath = paths + "\\Images\\" + imgLoc;

                        File.Copy(sourcePath, destinationPath);

                        MessageBox.Show("Image Succesfully Uploaded");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            frmUserChangePassword usr = new frmUserChangePassword(this);
            profileGrid.Children.Add(usr);
            
        }

      
    }
}

        
  

