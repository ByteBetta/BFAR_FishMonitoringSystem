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
    /// Interaction logic for frmProduct.xaml
    /// </summary>
    public partial class frmProduct : UserControl
    {
        MyConnection db = new MyConnection();

        SqlCommand cmd;
        SqlDataAdapter adapt;

        frmUser u;
        frmDealersandCustomers dc;
        frmEmployee emp;
        frmDealers dea;

        //ID variable used in Updating and Deleting Record  
        int ProdID = 0;
        string imgLoc = "laptop.png";
        string sourcePath = "";
        string destinationPath = "";



        // Global Variabel For The Image To Delte
        string rowHeaderImage;


        bool drag = false;
        Point start_point = new Point(0, 0);
        public frmProduct()
        {
            InitializeComponent();
            DisplayData();
            Colors();
        }

        CategoriesDAL cdal = new CategoriesDAL();
        DealersDAL ddal = new DealersDAL();
        ProductBLL p = new ProductBLL();
        ProductDAL pdal = new ProductDAL();
        loginDAL udal = new loginDAL();

        private void DisplayData()
        {
            db.con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select ProdID [PID], PCode, Manufactor [Brand], Model, Full_Name [Full Name], Price, Category, Description, Year, Warranty, Quantity, Reorder [Reorder Level], Dealer, Img, added_time [Added Time], added_by [Added By]  from Product", db.con);
            adapt.Fill(dt);
            grid_Product.ItemsSource = dt.DefaultView;
            db.con.Close();
        }

     
        //Clear Data  
        private void ClearData()
        {
            txtCode.Text = "";
            txtManufactor.Text = "";
            txtModel.Text = "";
            txtName.Text = "";
            txtPrice.Text = "";
            cmbDealer.Text = "";
            txtDescription.Text = "";
            txtReleaseYear.Text = "";
            txtWarranty.Text = "";
            txtQuantity.Text = "";
            cmbDealer.Text = "";
            txtReorder.Text = "";
            ProdID = 0;

            string paths = System.Windows.Forms.Application.StartupPath.Substring(0, (System.Windows.Forms.Application.StartupPath.Length - 10));

            string imagePath = paths + "\\Images\\Product\\laptop.png";
            imgBox.ImageSource = new BitmapImage(new Uri(imagePath));
            imgLoc = "laptop.png";
        }

        private void Colors()
        {
            SolidColorBrush bbrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            SolidColorBrush wbrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            txtCode.Foreground = bbrush;
            txtBarcode.Foreground = bbrush;
            txtDescription.Foreground = bbrush;
            txtManufactor.Foreground = bbrush;
            txtModel.Foreground = bbrush;
            txtName.Foreground = bbrush;
            txtPrice.Foreground = bbrush;
            txtProdID.Foreground = bbrush;
            txtQuantity.Foreground = bbrush;
            txtReleaseYear.Foreground = bbrush;
            txtReorder.Foreground = bbrush;
            txtSearch.Foreground = bbrush;
            txtWarranty.Foreground = bbrush;
            cmbCategory.Foreground = wbrush;
            cmbDealer.Foreground = wbrush;
            
        }

        private void grid_Product_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid gd = (DataGrid)sender;
                DataRowView row_selected = gd.SelectedItem as DataRowView;
                if (row_selected != null)
                {
                    txtProdID.Text = row_selected[0].ToString();
                    txtCode.Text = row_selected[1].ToString();
                    txtBarcode.Text = row_selected[2].ToString();
                    txtManufactor.Text = row_selected[3].ToString();
                    txtModel.Text = row_selected[4].ToString();
                    txtName.Text = row_selected[5].ToString();
                    txtPrice.Text = row_selected[6].ToString();
                    cmbCategory.Text = row_selected[7].ToString();
                    txtDescription.Text = row_selected[8].ToString();
                    txtReleaseYear.Text = row_selected[9].ToString();
                    txtWarranty.Text = row_selected[10].ToString();
                    txtQuantity.Text = row_selected[11].ToString();
                    cmbDealer.Text = row_selected[13].ToString();
                    txtReorder.Text = row_selected[12].ToString();
                    imgLoc = row_selected[14].ToString();

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
                    p.PCode = txtCode.Text;
                    p.Barcode = txtBarcode.Text;
                    p.Manufactor = txtManufactor.Text;
                    p.Model = txtModel.Text;
                    p.Full_Name = txtName.Text;
                    p.Price = decimal.Parse(txtPrice.Text);
                    p.Category = cmbCategory.Text;
                    p.Description = txtDescription.Text;
                    p.Year = txtReleaseYear.Text;
                    p.Warranty = int.Parse(txtWarranty.Text);
                    p.Quantity = 0;
                    p.Dealer = cmbDealer.Text;
                    p.Img = imgLoc;
                    p.added_time = DateTime.Now;
                    //Getting username of logged in user
                    string loggedUsr = frmLogin.loggedIn;
                    loginBLL usr = udal.GetIDFromUsername(loggedUsr);

                    p.added_by = usr.UserID;
                    p.Reorder = int.Parse(txtReorder.Text);
                }

                bool success = pdal.Insert(p);

                if (success == true)
                {
                    MessageBox.Show("Product Added Succesfully.");
                    ClearData();
                    DataTable dt = pdal.Select();
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
                    p.ProdID = int.Parse(txtProdID.Text);
                    p.PCode = txtCode.Text;
                    p.Barcode = txtBarcode.Text;
                    p.Manufactor = txtManufactor.Text;
                    p.Model = txtModel.Text;
                    p.Full_Name = txtName.Text;
                    p.Price = decimal.Parse(txtPrice.Text);
                    p.Category = cmbCategory.Text;
                    p.Description = txtDescription.Text;
                    p.Year = txtReleaseYear.Text;
                    p.Warranty = int.Parse(txtWarranty.Text);

                    p.Dealer = cmbDealer.Text;
                    p.Img = imgLoc;
                    p.added_time = DateTime.Now;

                    //Getting username of logged in user
                    string loggedUsr = frmLogin.loggedIn;
                    loginBLL usr = udal.GetIDFromUsername(loggedUsr);

                    p.added_by = usr.UserID;

                    p.Reorder = int.Parse(txtReorder.Text);
                }
                bool success = pdal.Update(p);

                if (success == true)
                {
                    MessageBox.Show("Product Updated Succesfully.");
                    ClearData();

                    DataTable dt = pdal.Select();
                    grid_Product.ItemsSource = dt.DefaultView;

                }
                else
                {
                    MessageBox.Show("Error Updating Product. Try Again!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            p.ProdID = int.Parse(txtProdID.Text);

            bool success = pdal.Delete(p);

            if (success == true)
            {
                MessageBox.Show("Product Deleted Succesfully.");
                ClearData();
                DataTable dt = pdal.Select();
                grid_Product.ItemsSource = dt.DefaultView;
            }
            else
            {
                MessageBox.Show("Error Deleting Product. Try Again!");
            }
        }

       

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            db.con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select * from Product where Full_Name like '" + txtSearch.Text + "%'", db.con);
            adapt.Fill(dt);
            grid_Product.ItemsSource = dt.DefaultView;
            db.con.Close();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //Creating DAta Table to hold the categories from Database
            DataTable categoriesDT = cdal.Select();
            //Specify DataSource for Category ComboBox
            cmbCategory.ItemsSource = categoriesDT.DefaultView;
            //Specify Display Member and Value Member for Combobox
            cmbCategory.DisplayMemberPath = "Title";
            cmbCategory.SelectedValuePath = "Title";


            //Creating Data Table to Hold the Dealers from DataBase
            DataTable dealersDT = ddal.Select();
            //Specify DataSource for Dealer ComboBox
            cmbDealer.ItemsSource = dealersDT.DefaultView;
            //Specify Display Member and Value Member for ComboBox

            cmbDealer.DisplayMemberPath = "Company Name";
            cmbDealer.SelectedValuePath = "Company Name";


            DataTable dt = pdal.Select();
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
    }
}
