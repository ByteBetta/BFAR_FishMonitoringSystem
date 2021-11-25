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
        string imgLoc = "fish.png";
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
            adapt = new SqlDataAdapter("select FishID [FishID], Species [Species Name], OrderName [Order Name], FamilyName [Family Name], LocalName [Local Name], FishBaseName [Fish Name], ShortDescription [Description], Biology [Biology], Measurement [Measurement], Distribution [Distribution], Environment [Environment], Occurance, Img, added_time [Added Time], added_by [Added By]  from FishDetails", db.con);
            adapt.Fill(dt);
            grid_Product.ItemsSource = dt.DefaultView;
            db.con.Close();
        }

     
        //Clear Data  
        private void ClearData()
        {
            txtFishName.Text = "";
            txtSpeciesName.Text = "";
            txtShortDescription.Text = "";
            txtBiology.Text = "";
            txtMeasurement.Text = "";
            txtOrderName.Text = "";
            txtLocalName.Text = "";
            txtDistribution.Text = "";
            txtEnvironment.Text = "";
            cmbFamilyName.Text = "";
            txtOccurrence.Text = "";
            FishID = 0;

            string paths = System.Windows.Forms.Application.StartupPath.Substring(0, (System.Windows.Forms.Application.StartupPath.Length - 10));

            string imagePath = paths + "\\Images\\Product\\fish.png";
            imgBox.ImageSource = new BitmapImage(new Uri(imagePath));
            imgLoc = "fish.png";
        }

        private void Colors()
        {
            SolidColorBrush bbrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            SolidColorBrush wbrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            txtFishName.Foreground = bbrush;
            txtSpeciesName.Foreground = bbrush;
            txtShortDescription.Foreground = bbrush;
            txtBiology.Foreground = bbrush;
            txtMeasurement.Foreground = bbrush;
            txtOrderName.Foreground = bbrush;
            txtLocalName.Foreground = bbrush;
            txtFishID.Foreground = bbrush;
            txtDistribution.Foreground = bbrush;
            txtEnvironment.Foreground = bbrush;
            txtOrderName.Foreground = bbrush;
            txtSearch.Foreground = bbrush;
            txtLocalName.Foreground = bbrush;
            txtOccurrence.Foreground = bbrush;
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
                    txtSpeciesName.Text = row_selected[1].ToString();
                    txtOrderName.Text = row_selected[2].ToString();
                    cmbFamilyName.Text = row_selected[3].ToString();
                    txtLocalName.Text = row_selected[4].ToString();
                    txtFishName.Text = row_selected[5].ToString();
                    txtShortDescription.Text = row_selected[6].ToString();
                    txtBiology.Text = row_selected[7].ToString();
                    txtMeasurement.Text = row_selected[8].ToString();
                    txtDistribution.Text = row_selected[9].ToString();
                    txtEnvironment.Text = row_selected[10].ToString();
                    txtOccurrence.Text = row_selected[11].ToString();
                    imgLoc = row_selected[12].ToString();

                    //Update the Value of Global Variable rowheaderImage
                    rowHeaderImage = imgLoc;

                    string paths = System.Windows.Forms.Application.StartupPath.Substring(0, (System.Windows.Forms.Application.StartupPath.Length - 10));
                    if (imgLoc != "fish.png")
                    {
                        string imagePath = paths + "\\Images\\Product\\" + imgLoc;
                        imgBox.ImageSource = new BitmapImage(new Uri(imagePath));

                    }
                    else
                    {
                        string imagePath = paths + "\\Images\\Product\\fish.png";
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
                    fishdata.Species = txtSpeciesName.Text;
                    fishdata.ShortDescription = txtShortDescription.Text;
                    fishdata.Biology = txtBiology.Text;
                    fishdata.Measurement = txtMeasurement.Text;
                    fishdata.Species = txtSpeciesName.Text;
                    fishdata.OrderName = txtOrderName.Text;
                    fishdata.FamilyName = cmbFamilyName.Text;
                    fishdata.LocalName = txtLocalName.Text;
                    fishdata.Distribution = txtDistribution.Text;
                    fishdata.Environment = txtEnvironment.Text;
                    fishdata.Img = imgLoc;
                    fishdata.added_time = DateTime.Now;
                    //Getting username of logged in user
                    string loggedUsr = frmLogin.loggedIn;
                    loginBLL usr = udal.GetIDFromUsername(loggedUsr);
                
                    fishdata.added_by = usr.UserID;
                    fishdata.FishBaseName = txtFishName.Text;
                    fishdata.Occurance = txtOccurrence.Text;

                }

                bool success = fishdal.Insert(fishdata);

                if (success == true)
                {
                    MessageBox.Show("Fish Added Succesfully.");
                    ClearData();
                    DataTable dt = fishdal.Select();
                    grid_Product.ItemsSource = dt.DefaultView;

                }
                else
                {
                    MessageBox.Show("Error Adding Fish. Try Again!");
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
                    fishdata.FishID = int.Parse(txtFishID.Text);
                    fishdata.Species = txtSpeciesName.Text;
                    fishdata.ShortDescription = txtShortDescription.Text;
                    fishdata.Biology = txtBiology.Text;
                    fishdata.Measurement = txtMeasurement.Text;
                    fishdata.Species = txtSpeciesName.Text;
                    fishdata.OrderName = txtOrderName.Text;
                    fishdata.FamilyName = cmbFamilyName.Text;
                    fishdata.LocalName = txtLocalName.Text;
                    fishdata.Distribution = txtDistribution.Text;
                    fishdata.Environment = txtEnvironment.Text;
                    fishdata.Img = imgLoc;
                    fishdata.added_time = DateTime.Now;
                    //Getting username of logged in user
                    string loggedUsr = frmLogin.loggedIn;
                    loginBLL usr = udal.GetIDFromUsername(loggedUsr);

                    fishdata.added_by = usr.UserID;
                    fishdata.FishBaseName = txtFishName.Text;
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
            adapt = new SqlDataAdapter("select * from FishDetails where Species like '" + txtSearch.Text + "%'", db.con);
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

                    imgLoc = "Fish" + RandInt + ext;

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

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            this.fishdal.ConnecttoFirebase();
            DataTable dt = fishdal.Select();

            try
            {
                foreach (DataRow data in dt.Rows)
                {


                    fishdata.FishID = Convert.ToInt32(data["FishID"]);
                    fishdata.Species = data["Species"].ToString();
                    fishdata.Img = data["Img"].ToString();
                    fishdata.ShortDescription = data["Description"].ToString();
                    fishdata.Biology = data["Biology"].ToString();
                    fishdata.Measurement = data["Measurement"].ToString();
                    fishdata.OrderName = data["Order Name"].ToString();
                    fishdata.FamilyName = data["Family Name"].ToString();
                    fishdata.LocalName = data["Local Name"].ToString();
                    fishdata.Distribution = data["Distribution"].ToString();
                    fishdata.Environment = data["Environment"].ToString();
                    fishdata.FishBaseName = data["Fish Name"].ToString();
                    fishdata.Occurance = data["Occurance"].ToString();



                    fishdal.AddFishtoFirebaseAsync(fishdata);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        } 
    }
}
