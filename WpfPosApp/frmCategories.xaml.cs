using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using Project.BLL;
using Project.DAL;

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for Categories
    /// </summary>
    //public partial class frmCategories : Window
    public partial class frmCategories : UserControl
    {
        MyConnection db = new MyConnection();
               
        SqlCommand cmd;
        SqlDataAdapter adapt;
        //ID variable used in Updating and Deleting Record  
        int EmpID = 0;
        string imgLoc = "";

        bool drag = false;
        Point start_point = new Point(0, 0);

        CategoriesBLL c = new CategoriesBLL();
        CategoriesDAL dal = new CategoriesDAL();
        loginDAL udal = new loginDAL();
        public frmCategories()
        {
            InitializeComponent();
            Clear();
            Colors();
        }

        public void Colors()
        {
            SolidColorBrush bbrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            txtCatID.Foreground = bbrush;
            txtTitle.Foreground = bbrush;
            txtDescription.Foreground = bbrush;
            txtSearch.Foreground = bbrush;
        }
        public void Clear()
        {
            txtCatID.Text = "";
            txtTitle.Text = "";
            txtDescription.Text = "";
        }
      

        private void Window_Activated(object sender, EventArgs e)
        {
            //Here write the code to display all the categries in DAta Grid View
            DataTable dt = dal.Select();
            dgvCategories.ItemsSource = dt.DefaultView;
        }

       

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //Get the values from Categroy Form
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
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
                MessageBox.Show("New Category Added Successfully.");
                Clear();
                //Refresh Data Grid View
                DataTable dt = dal.Select();
                dgvCategories.ItemsSource = dt.DefaultView;

                
            }
            else
            {
                //FAiled to Insert New Category
                MessageBox.Show("Failed to Add New Category.");
            }
        }

        private void DgvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            int RowIndex = e.SelectedIndex;
            txtCatID.Text = dgvCategories.Rows[RowIndex].Cells[0].Value.ToString();
            txtTitle.Text = dgvCategories.Rows[RowIndex].Cells[1].Value.ToString();
            txtDescription.Text = dgvCategories.Rows[RowIndex].Cells[2].Value.ToString();
            */
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected !=null)
            {
                txtCatID.Text = row_selected[0].ToString();
                txtTitle.Text = row_selected[1].ToString();
                txtDescription.Text = row_selected[2].ToString();
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //Get the Values from the CAtegory form
            c.CatID = int.Parse(txtCatID.Text);
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
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
                MessageBox.Show("Category Updated Successfully");
                Clear();
                //Refresh Data Gid View
                DataTable dt = dal.Select();
                dgvCategories.ItemsSource = dt.DefaultView;
            }
            else
            {
                //FAiled to Update Category
                MessageBox.Show("Failed to Update Category");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            //Get te ID of the Category Which we want to Delete
            c.CatID = int.Parse(txtCatID.Text);

            //Creating Boolean Variable to Delete The CAtegory
            bool success = dal.Delete(c);

            //If the CAtegory id Deleted Successfully then the vaue of success will be true else it will be false
            if (success == true)
            {
                //Category Deleted Successfully
                MessageBox.Show("Category Deleted Successfully");
                Clear();
                //REfreshing DAta Grid View
                DataTable dt = dal.Select();
                dgvCategories.ItemsSource = dt.DefaultView;
            }
            else
            {
                //FAiled to Delete CAtegory 
                MessageBox.Show("Failed to Delete Category");
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Get the Keywords
            string keywords = txtSearch.Text;

            //Filte the categories based on keywords
            if (keywords != null)
            {
                //Use Searh Method To Display Categoreis
                DataTable dt = dal.Search(keywords);
                dgvCategories.ItemsSource = dt.DefaultView;
            }
            else
            {
                //Use Select Method to Display All Categories
                DataTable dt = dal.Select();
                dgvCategories.ItemsSource = dt.DefaultView;
            }
        }

        private void dgvCategories_Loaded(object sender, RoutedEventArgs e)
        {
            //Here write the code to display all the categries in DAta Grid View
           
            DataTable dt = dal.Select();
            dgvCategories.ItemsSource = dt.DefaultView;

        }
    }
}
