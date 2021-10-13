using Fish.BLL;
using Fish.DAL;
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
using System.Windows.Shapes;
using WpfPosApp.DAL;

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for frmFishDetails.xaml
    /// </summary>
    public partial class frmFishDetails : UserControl
    {
        MyConnection db = new MyConnection();

        SqlCommand cmd;
        SqlDataAdapter adapt;

        frmUser u;
        frmDealersandCustomers dc;
        frmEmployee emp;
        frmDealers dea;

        //ID variable used in Updating and Deleting Record  
        int FishID = 0;
        string imgLoc = "laptop.png";
        string sourcePath = "";
        string destinationPath = "";



        // Global Variabel For The Image To Delte
        string rowHeaderImage;


        bool drag = false;
        Point start_point = new Point(0, 0);
        public frmFishDetails()
        {
            InitializeComponent();
            DisplayData();
            Colors();
        }

        CategoriesDAL cdal = new CategoriesDAL();
        DealersDAL ddal = new DealersDAL();
        FishBLL fishdata = new FishBLL();
        FishDAL fishdal = new FishDAL();
        loginDAL udal = new loginDAL();

        private void DisplayData()
        {
            db.con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select FishID [FishID], ScientificName, MaxLength, CommonLength, AnalSpine, AnalSoftRay, DorsalSpine, DorsalSoftRay, Remark, OrderName, FamilyName, LocalName, Salinity, Location, Occurance, Img, FishName, added_time [Added Time], added_by [Added By]  from FishDetails", db.con);
            adapt.Fill(dt);
            grid_Product.ItemsSource = dt.DefaultView;
            db.con.Close();
        }

     
        //Clear Data  
        private void ClearData()
        {
            txtFishName.Text = "";
            txtScientificName.Text = "";
            txtMaxLength.Text = "";
            txtDorsalSpine.Text = "";
            txtCommonLength.Text = "";
            txtDorsalSoftray.Text = "";
            txtAnalSpine.Text = "";
            TxtAnalSoftRay.Text = "";
            txtRemark.Text = "";
            txtOrderName.Text = "";
            cmbFamilyName.Text = "";
            txtLocalName.Text = "";
            txtSalinity.Text = "";
            txtOccurrence.Text = "";
            TxtLocation.Text = "";
            FishID = 0;

            string paths = System.Windows.Forms.Application.StartupPath.Substring(0, (System.Windows.Forms.Application.StartupPath.Length - 10));

            string imagePath = paths + "\\Images\\Product\\laptop.png";
            imgBox.ImageSource = new BitmapImage(new Uri(imagePath));
            imgLoc = "laptop.png";
        }

        private void Colors()
        {
            SolidColorBrush bbrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            SolidColorBrush wbrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            txtFishName.Foreground = bbrush;
            txtScientificName.Foreground = bbrush;
            txtMaxLength.Foreground = bbrush;
            txtDorsalSpine.Foreground = bbrush;
            txtCommonLength.Foreground = bbrush;
            txtDorsalSoftray.Foreground = bbrush;
            txtAnalSpine.Foreground = bbrush;
            txtFishID.Foreground = bbrush;
            TxtAnalSoftRay.Foreground = bbrush;
            txtRemark.Foreground = bbrush;
            txtOrderName.Foreground = bbrush;
            txtSearch.Foreground = bbrush;
            txtSalinity.Foreground = bbrush;
            txtLocalName.Foreground = bbrush;
            txtOccurrence.Foreground = bbrush;
            TxtLocation.Foreground = bbrush;
            cmbFamilyName.Foreground = wbrush;
            
        }

        private void grid_Product_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid gd = (DataGrid)sender;
                DataRowView row_selected = gd.SelectedItem as DataRowView;
                if (row_selected != null)
                {
                    txtFishID.Text = row_selected[0].ToString();
                    txtScientificName.Text = row_selected[1].ToString();
                    txtOrderName.Text = row_selected[2].ToString();
                    cmbFamilyName.Text = row_selected[3].ToString();
                    txtLocalName.Text = row_selected[4].ToString();
                    txtFishName.Text = row_selected[5].ToString();
                    txtMaxLength.Text = row_selected[6].ToString();
                    txtCommonLength.Text = row_selected[7].ToString();
                    txtAnalSpine.Text = row_selected[8].ToString();
                    TxtAnalSoftRay.Text = row_selected[9].ToString();
                    txtDorsalSpine.Text = row_selected[10].ToString();
                    txtDorsalSoftray.Text = row_selected[11].ToString();
                    txtRemark.Text = row_selected[12].ToString();
                    txtSalinity.Text = row_selected[13].ToString();
                    TxtLocation.Text = row_selected[14].ToString();
                    txtOccurrence.Text = row_selected[15].ToString();
                    imgLoc = row_selected[16].ToString();

                    //Update the Value of Global Variable rowheaderImage
                    rowHeaderImage = imgLoc;

                    string paths = System.Windows.Forms.Application.StartupPath.Substring(0, (System.Windows.Forms.Application.StartupPath.Length - 10));
                    if (imgLoc != "computer.png")
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (imgLoc != "")
                {
                    fishdata.ScientificName = txtScientificName.Text;
                    fishdata.MaxLength = int.Parse(txtMaxLength.Text);
                    fishdata.CommonLength = int.Parse(txtCommonLength.Text);
                    fishdata.AnalSpine = int.Parse(txtAnalSpine.Text);
                    fishdata.AnalSoftRay = int.Parse(TxtAnalSoftRay.Text);
                    fishdata.DorsalSpine = int.Parse(txtDorsalSpine.Text);
                    fishdata.DorsalSoftRay = int.Parse(txtDorsalSoftray.Text);
                    fishdata.Remark = txtRemark.Text;
                    fishdata.OrderName = txtOrderName.Text;
                    fishdata.FamilyName = cmbFamilyName.Text;
                    fishdata.LocalName = txtLocalName.Text;
                    fishdata.Salinity = txtSalinity.Text;
                    fishdata.Img = imgLoc;
                    fishdata.added_time = DateTime.Now;
                    //Getting username of logged in user
                    string loggedUsr = frmLogin.loggedIn;
                    loginBLL usr = udal.GetIDFromUsername(loggedUsr);

                    fishdata.added_by = usr.UserID;
                    fishdata.Location = TxtLocation.Text;
                    fishdata.FishName = txtFishName.Text;
                    fishdata.Occurance = txtOccurrence.Text;

                }

                bool success = fishdal.Insert(fishdata);

                if (success == true)
                {
                    MessageBox.Show("Product Added Succesfully.");
                    ClearData();
                    DataTable dt = fishdal.Select();
                    grid_Product.ItemsSource = dt.DefaultView;

                }
                else
                {
                    MessageBox.Show("Error Adding Product. Try Again!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (imgLoc != "")
                {
                    fishdata.ScientificName = txtScientificName.Text;
                    fishdata.MaxLength = int.Parse(txtMaxLength.Text);
                    fishdata.CommonLength = int.Parse(txtCommonLength.Text);
                    fishdata.AnalSpine = int.Parse(txtAnalSpine.Text);
                    fishdata.AnalSoftRay = int.Parse(TxtAnalSoftRay.Text);
                    fishdata.DorsalSpine = int.Parse(txtDorsalSpine.Text);
                    fishdata.DorsalSoftRay = int.Parse(txtDorsalSoftray.Text);
                    fishdata.Remark = txtRemark.Text;
                    fishdata.OrderName = txtOrderName.Text;
                    fishdata.FamilyName = cmbFamilyName.Text;
                    fishdata.LocalName = txtLocalName.Text;
                    fishdata.Salinity = txtSalinity.Text;
                    fishdata.Img = imgLoc;
                    fishdata.added_time = DateTime.Now;
                    //Getting username of logged in user
                    string loggedUsr = frmLogin.loggedIn;
                    loginBLL usr = udal.GetIDFromUsername(loggedUsr);

                    fishdata.added_by = usr.UserID;
                    fishdata.Location = TxtLocation.Text;
                    fishdata.FishName = txtFishName.Text;
                    fishdata.Occurance = txtOccurrence.Text;
                }
                bool success = fishdal.Update(fishdata);

                if (success == true)
                {
                    MessageBox.Show("Fish Data Updated Succesfully.");
                    ClearData();

                    DataTable dt = fishdal.Select();
                    grid_Product.ItemsSource = dt.DefaultView;

                }
                else
                {
                    MessageBox.Show("Error Updating Fish. Try Again!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            fishdata.FishID = int.Parse(txtFishID.Text);

            bool success = fishdal.Delete(fishdata);

            if (success == true)
            {
                MessageBox.Show("Fish Data Deleted Succesfully.");
                ClearData();
                DataTable dt = fishdal.Select();
                grid_Product.ItemsSource = dt.DefaultView;
            }
            else
            {
                MessageBox.Show("Error Deleting Fish Data. Try Again!");
            }
        }

       

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            db.con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select * from FishDetails where FishName like '" + txtSearch.Text + "%'", db.con);
            adapt.Fill(dt);
            grid_Product.ItemsSource = dt.DefaultView;
            db.con.Close();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //Creating DAta Table to hold the categories from Database
            DataTable categoriesDT = cdal.Select();
            //Specify DataSource for Category ComboBox
            cmbFamilyName.ItemsSource = categoriesDT.DefaultView;
            //Specify Display Member and Value Member for Combobox
            cmbFamilyName.DisplayMemberPath = "Title";
            cmbFamilyName.SelectedValuePath = "Title";


            

            DataTable dt = fishdal.Select();
            grid_Product.ItemsSource = dt.DefaultView;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();

            open.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.PNG; *gifs;)|*.jpg; *.jpeg; *.png; *.PNG; *gifs";

            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (open.CheckFileExists)
                {
                    imgBox.ImageSource = new BitmapImage(new Uri(open.FileName));

                    string ext = System.IO.Path.GetExtension(open.FileName);

                    Random random = new Random();
                    int RandInt = random.Next(0, 1000);

                    imgLoc = "Prod" + RandInt + ext;

                    sourcePath = open.FileName;

                    string paths = System.Windows.Forms.Application.StartupPath.Substring(0, System.Windows.Forms.Application.StartupPath.Length - 10);

                    destinationPath = paths + "\\Images\\Product\\" + imgLoc;

                    File.Copy(sourcePath, destinationPath);


                }
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            frmDataReport frm = new frmDataReport(u, dc, emp, this, dea);
            frm.LoadProducReport();
            frm.ShowDialog();
        }

        private void CmbFamilyName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
