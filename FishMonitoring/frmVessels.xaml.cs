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
    /// Interaction logic for frmDealers.xaml
    /// </summary>
    public partial class frmVessels : UserControl
    {
       
        frmFishDetails p;
  

        public frmVessels()
        {
            InitializeComponent();
            Colors();
        }

        VesselBLL vs = new VesselBLL();
        VesselDAL vsdal = new VesselDAL();
        DealersDAL dealersDAL = new DealersDAL();

        loginDAL uDal = new loginDAL();

        public void Clear()
        {
            txtID.Text = "";
            cmbOwnerList.Text = "";
            txtVesselCode.Text = "";
            txtVesselName.Text = "";
            txtSearch.Text = "";
        }


        public void Colors()
        {
            SolidColorBrush bbrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            SolidColorBrush wbrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            txtID.Foreground = bbrush;
            cmbOwnerList.Foreground = wbrush;
            txtVesselCode.Foreground = bbrush;
            txtVesselName.Foreground = bbrush;
            txtSearch.Foreground = bbrush;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            vs.VesselName = txtVesselName.Text;
            vs.VesselOwner = cmbOwnerList.Text;
            vs.VesselCode = txtVesselCode.Text;
            vs.added_date = DateTime.Now;

            VesselBLL vsl = vsdal.GetIDFromOwnerName(cmbOwnerList.Text);
            vs.DealID = vsl.DealID;

            string loggedUsr = frmLogin.loggedIn;

            loginBLL usr = uDal.GetIDFromUsername(loggedUsr);
            vs.added_by = usr.UserID;

            bool success = vsdal.Insert(vs);

            if (success == true)
            {
                MessageBox.Show("Vessel Added Succesfully");
                Clear();
                DataTable dt = vsdal.Select();
                gridDealer.ItemsSource = dt.DefaultView;
            }
            else
            {
                MessageBox.Show("Failed to Add New Vessel");

            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            vs.VesselID = int.Parse(txtID.Text);

            vs.VesselName = txtVesselName.Text;
            vs.VesselOwner = cmbOwnerList.Text;
            vs.VesselCode = txtVesselCode.Text;
            vs.added_date = DateTime.Now;


            string loggedUsr = frmLogin.loggedIn;
            loginBLL usr = uDal.GetIDFromUsername(loggedUsr);
            vs.added_by = usr.UserID;
 

            bool succes = vsdal.Update(vs);

            if (succes == true)
            {
                MessageBox.Show("Vessel updated Succesfully");
                Clear();
                DataTable dt = vsdal.Select();
                gridDealer.ItemsSource = dt.DefaultView;

            }
            else
            {
                MessageBox.Show("Failed to Update Vessel");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            vs.VesselID = int.Parse(txtID.Text);

            bool success = vsdal.Delete(vs);

            if (success == true)
            {
                //
                MessageBox.Show("Vessel Deleted Successfully");

                Clear();
                DataTable dt = vsdal.Select();
                gridDealer.ItemsSource = dt.DefaultView;


            }
            else
            {
                MessageBox.Show("Failed to Delete Vessel");

            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            //frmDataReport frm = new frmDataReport(u, cc, emp, p, this);
            //frm.LoadDealerstReport();
            //frm.ShowDialog();
        }

        private void gridDealer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                txtID.Text = row_selected[0].ToString();
 
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = vsdal.Select();
            gridDealer.ItemsSource = dt.DefaultView;

            DataTable ownerlist = dealersDAL.Select();
            //Specify DataSource for Category ComboBox
            cmbOwnerList.ItemsSource = ownerlist.DefaultView;
            //Specify Display Member and Value Member for Combobox
            cmbOwnerList.DisplayMemberPath = "Company Name";
            cmbOwnerList.SelectedValuePath = "Company Name";
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Get the keyword from text box
            string keyword = txtSearch.Text;

            if (keyword != null)
            {
                //Search the Dealer
                DataTable dt = vsdal.Search(keyword);
                gridDealer.ItemsSource = dt.DefaultView;
            }
            else
            {
                //Show all the Dealer
                DataTable dt = vsdal.Select();
                gridDealer.ItemsSource = dt.DefaultView;
            }
        }
    }
}
