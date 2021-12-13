using Project.BLL;
using Project.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for frmlandingSite.xaml
    /// </summary>
    public partial class frmlandingSite : UserControl
    {


        MyConnection db = new MyConnection();

        SqlCommand cmd;
        SqlDataAdapter adapt;
        //ID variable used in Updating and Deleting Record  
        int EmpID = 0;
        string imgLoc = "";

        bool drag = false;
        Point start_point = new Point(0, 0);

        landingSiteBLL c = new landingSiteBLL();
        LandingSiteDAL dal = new LandingSiteDAL();
        loginDAL udal = new loginDAL();

        public frmlandingSite()
        {
            InitializeComponent();
            Colors();
            Clear();
        }

        public void Colors()
        {
            SolidColorBrush bbrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            txtlandingSiteID.Foreground = bbrush;
            txtlandingTitle.Foreground = bbrush;
            txtlandingDescription.Foreground = bbrush;
            txtlandingSearch.Foreground = bbrush;
        }
        public void Clear()
        {
            txtlandingSiteID.Text = "";
            txtlandingTitle.Text = "";
            txtlandingDescription.Text = "";
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            //Here write the code to display all the categries in DAta Grid View
            DataTable dt = dal.Select();
            dvlsite.ItemsSource = dt.DefaultView;
        }


        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //Get the values from Categroy Form
            c.title = txtlandingTitle.Text.Trim();
            c.description = txtlandingDescription.Text;
            c.added_date = DateTime.Now;

            //Getting ID in Added by field
            string loggedUser = frmLogin.loggedIn;
            loginBLL usr = udal.GetIDFromUsername(loggedUser);

            //Passign the id of Logged in User in added by field
            c.added_by = usr.UserID;

            //Creating Boolean Method To insert data into database
            bool success = dal.Insert(c);

            //If the category is inserted successfully then the value of the success will be true else it will be false
            if (success == true)
            {
                //NewCAtegory Inserted Successfully
                MessageBox.Show("New Landing Site Added Successfully.");
                Clear();
                //Refresh Data Grid View
                DataTable dt = dal.Select();
                dvlsite.ItemsSource = dt.DefaultView;
                int holder = Convert.ToInt32(dal.getLandingIDbyName(c.title).landingSiteID);
                c.landingSiteID = holder;
                dal.AddlandingtoFirebaseAsync(c);


            }
            else
            {
                //FAiled to Insert New Category
                MessageBox.Show("Failed to Add New Landing Site.");
            }
        }

        private void DgvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                txtlandingSiteID.Text = row_selected[0].ToString();
                txtlandingTitle.Text = row_selected[1].ToString();
                txtlandingDescription.Text = row_selected[2].ToString();
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //Get the Values from the CAtegory form
            c.landingSiteID = int.Parse(txtlandingSiteID.Text);
            c.title = txtlandingTitle.Text.Trim();
            c.description = txtlandingDescription.Text;
            c.added_date = DateTime.Now;
            //Getting ID in Added by field
            string loggedUser = frmLogin.loggedIn;
            loginBLL usr = udal.GetIDFromUsername(loggedUser);

            //Passign the id of Logged in User in added by field
            c.added_by = usr.UserID;

            //Creating Boolean variable to update categories and check 
            bool success = dal.Update(c);
            //If the cateory is updated successfully then the value of success will be true else it will be false
            if (success == true)
            {
                //CAtegory updated Successfully 
                MessageBox.Show("Landing Site Updated Successfully");
                Clear();
                //Refresh Data Gid View
                DataTable dt = dal.Select();
                dvlsite.ItemsSource = dt.DefaultView;
                dal.updatelandingFirestore(c);
            }
            else
            {
                //FAiled to Update Category
                MessageBox.Show("Failed to Update Landing Site");
            }

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            c.landingSiteID = int.Parse(txtlandingSiteID.Text);

            //Creating Boolean Variable to Delete The CAtegory
            bool success = dal.Delete(c);

            //If the CAtegory id Deleted Successfully then the vaue of success will be true else it will be false
            if (success == true)
            {
                //Category Deleted Successfully
                MessageBox.Show("Gear Deleted Successfully");
                Clear();
                dal.deletelandinginFirestore(c.landingSiteID);
                //REfreshing DAta Grid View
                DataTable dt = dal.Select();
                dvlsite.ItemsSource = dt.DefaultView;
            }
            else
            {
                //FAiled to Delete CAtegory 
                MessageBox.Show("Failed to Delete Gear");
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keywords = txtlandingSearch.Text;

            //Filte the categories based on keywords
            if (keywords != null)
            {
                //Use Searh Method To Display Categoreis
                DataTable dt = dal.Search(keywords);
                dvlsite.ItemsSource = dt.DefaultView;
            }
            else
            {
                //Use Select Method to Display All Categories
                DataTable dt = dal.Select();
                dvlsite.ItemsSource = dt.DefaultView;
            }
        }

        private void dgvCategories_Loaded(object sender, RoutedEventArgs e)
        {

            DataTable dt = dal.Select();
            dvlsite.ItemsSource = dt.DefaultView;
        }
    }

}

