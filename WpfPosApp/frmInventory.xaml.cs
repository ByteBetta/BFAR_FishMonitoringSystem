using Project.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for frmInventory.xaml
    /// </summary>
    public partial class frmInventory : UserControl
    {
        MyConnection db = new MyConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataAdapter adapt;
        SqlDataReader dr;
        bool drag = false;

        Point start_point = new Point(0, 0);
        public frmInventory()
        {
            InitializeComponent();
            cmbCategories.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        CategoriesDAL cdal = new CategoriesDAL();
        ProductDAL pdal = new ProductDAL();

        private void DisplayData()
        {
            db.con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select ProdID [PID], PCode, Manufactor [Brand], Model, Full_Name [Full Name], Price, Category, Description, Year, Warranty, Quantity, Reorder [Reorder Level], Dealer  from Product", db.con);
            adapt.Fill(dt);
            gridInventory.ItemsSource = dt.DefaultView;
            db.con.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayData();
            DataTable pdd = pdal.SelectCritical();
            gridCritical.ItemsSource = pdd.DefaultView;
        }



        public void cmbCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

            try
            {
                string category = (e.AddedItems[0] as ComboBoxItem).Content as string;
                DataTable dt = pdal.DisplayProductsByCategory(category);
                gridInventory.ItemsSource = dt.DefaultView;
                 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            //Display all the products when this button is clicked
            DataTable dt = pdal.Select();
            gridInventory.ItemsSource = dt.DefaultView;

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            frmInventroyReport frm = new frmInventroyReport(this);
            frm.LoadReport();
            frm.ShowDialog();
        }

        private void btnPrintFiltered_Click(object sender, RoutedEventArgs e)
        {
            frmInventroyReport frm = new frmInventroyReport(this);
            frm.LoadReport2();
            frm.ShowDialog();
        }

        private void btnPrintCritical_Click(object sender, RoutedEventArgs e)
        {
            frmInventroyReport frm = new frmInventroyReport(this);
            frm.LoadCritical();
            frm.ShowDialog();
        }

    }
}
