using Project.BLL;
using Project.DAL;
using System;
using System.Collections.Generic;
using System.Data;
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
using WpfPosApp.BLL;
using WpfPosApp.DAL;

namespace WpfPosApp
{
    /// <summary>
    /// <summary>
    /// Interaction logic for frmDealers.xaml
    /// </summary>
    public partial class frmDealers : UserControl
    {
        frmUser u;
        frmEmployee emp;
        frmFishDetails p;
        frmDealersandCustomers cc;

        public frmDealers()
        {
            InitializeComponent();
            Colors();
        }

        DealersBLL dc = new DealersBLL();
        DealersDAL dcDal = new DealersDAL();

        loginDAL uDal = new loginDAL();

        public void Clear()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtPerson.Text = "";
            txtEmail.Text = "";
            txtMobile.Text = "";
            txtAddress.Text = "";
            txtSearch.Text = "";
        }


        public void Colors()
        {
            SolidColorBrush bbrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            SolidColorBrush wbrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            txtID.Foreground = bbrush;
            txtAddress.Foreground = bbrush;
            txtEmail.Foreground = bbrush;
            txtMobile.Foreground = bbrush;
            txtName.Foreground = bbrush;
            txtPerson.Foreground = bbrush;
            txtSearch.Foreground = bbrush;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            dc.name = txtName.Text;
            dc.person = txtPerson.Text;
            dc.email = txtEmail.Text;
            dc.contact = txtMobile.Text;
            dc.address = txtAddress.Text;
            dc.added_date = DateTime.Now;

            string loggedUsr = frmLogin.loggedIn;

            loginBLL usr = uDal.GetIDFromUsername(loggedUsr);
            dc.added_by = usr.UserID;

            bool success = dcDal.Insert(dc);

            if (success == true)
            {
                MessageBox.Show("Dealer Added Succesfully");
                Clear();
                DataTable dt = dcDal.Select();
                gridDealer.ItemsSource = dt.DefaultView;
            }
            else
            {
                MessageBox.Show("Failed to Add New Dealer");

            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            dc.DealID = int.Parse(txtID.Text);
            dc.name = txtName.Text;
            dc.person = txtPerson.Text;
            dc.email = txtEmail.Text;
            dc.contact = txtMobile.Text;
            dc.address = txtAddress.Text;
            dc.added_date = DateTime.Now;


            string loggedUsr = frmLogin.loggedIn;
            loginBLL usr = uDal.GetIDFromUsername(loggedUsr);
            dc.added_by = usr.UserID;

            bool succes = dcDal.Update(dc);

            if (succes == true)
            {
                MessageBox.Show("Dealer updated Succesfully");
                Clear();
                DataTable dt = dcDal.Select();
                gridDealer.ItemsSource = dt.DefaultView;

            }
            else
            {
                MessageBox.Show("Failed to Update Dealer");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            dc.DealID = int.Parse(txtID.Text);

            bool success = dcDal.Delete(dc);

            if (success == true)
            {
                //
                MessageBox.Show("Dealer Deleted Successfully");

                Clear();
                DataTable dt = dcDal.Select();
                gridDealer.ItemsSource = dt.DefaultView;


            }
            else
            {
                MessageBox.Show("Failed to Delete Dealer");

            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            frmDataReport frm = new frmDataReport(u, cc, emp, p, this);
            frm.LoadDealerstReport();
            frm.ShowDialog();
        }

        private void gridDealer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                txtID.Text = row_selected[0].ToString();
                txtName.Text = row_selected[1].ToString();
                txtPerson.Text = row_selected[2].ToString();
                txtEmail.Text = row_selected[3].ToString();
                txtMobile.Text = row_selected[4].ToString();
                txtAddress.Text = row_selected[5].ToString();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = dcDal.Select();
            gridDealer.ItemsSource = dt.DefaultView;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Get the keyword from text box
            string keyword = txtSearch.Text;

            if (keyword != null)
            {
                //Search the Dealer
                DataTable dt = dcDal.Search(keyword);
                gridDealer.ItemsSource = dt.DefaultView;
            }
            else
            {
                //Show all the Dealer
                DataTable dt = dcDal.Select();
                gridDealer.ItemsSource = dt.DefaultView;
            }
        }
    }
}
