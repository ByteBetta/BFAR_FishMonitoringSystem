using Project.DAL;
using Project.BLL;
using System;
using System.Collections.Generic;
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
using System.IO;

namespace WpfPosApp
{
    /// <summary>
    /// Interaction logic for frmEmplyee.xaml
    /// </summary>
    public partial class frmEmployee : UserControl
    {
        MyConnection db = new MyConnection();

        SqlCommand cmd;
        SqlDataAdapter adapt;
        //ID variable used in Updating and Deleting Record  
        int EmpID = 0;
        string imgLoc = "";

        bool drag = false;
        Point start_point = new Point(0, 0);
        loginDAL uDal = new loginDAL();
        frmUser u;
        frmDealersandCustomers dc;
        frmProduct p;
        frmDealers dea;


        public frmEmployee()
        {
            InitializeComponent();
            //   showdataClass();
            DisplayData();
            showdataGender();
            Colors();
        }

        private void DisplayData()
        {
            db.con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select EmpID, Name, Surname, ID, SEX [Gender], Birth_Date, Age, Address, Mobile, DateOfReciving [Reciving Date], Sallary, added_date [Added Time] from Employee", db.con);
            adapt.Fill(dt);
            grid_Employee.ItemsSource = dt.DefaultView;
            db.con.Close();
        }

        //Clear Data  
        private void ClearData()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtIDNumber.Text = "";
            cmbSex.Text = "";
            dtpBirth.Text = "";
            txtAge.Text = "";
            txtAddress.Text = "";
            txtMobile.Text = "";
            dtpReciving.Text = "";
            txtSallary.Text = "";


            EmpID = 0;
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

        public void Colors()
        {
            SolidColorBrush bbrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            SolidColorBrush wbrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            txtAddress.Foreground = bbrush;
            txtAge.Foreground = bbrush;
            txtFirstName.Foreground = bbrush;
            txtIDNumber.Foreground = bbrush;
            txtLastName.Foreground = bbrush;
            txtMobile.Foreground = bbrush;
            txtSallary.Foreground = bbrush;
            txtSearchEmoloyee.Foreground = bbrush;
            dtpBirth.Foreground = wbrush;
            dtpReciving.Foreground = wbrush;
            cmbSex.Foreground = wbrush;
        }
        private void grid_Employee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid gd = (DataGrid)sender;
                DataRowView row_selected = gd.SelectedItem as DataRowView;
                if (row_selected != null)
                {
                    EmpID = Convert.ToInt32(row_selected[0].ToString());
                    txtFirstName.Text = row_selected[1].ToString();
                    txtLastName.Text = row_selected[2].ToString();
                    txtIDNumber.Text = row_selected[3].ToString();
                    cmbSex.Text = row_selected[4].ToString();
                    dtpBirth.Text = row_selected[5].ToString();
                    txtAge.Text = row_selected[6].ToString();
                    txtAddress.Text = row_selected[7].ToString();
                    txtMobile.Text = row_selected[8].ToString();
                    dtpReciving.Text = row_selected[9].ToString();
                    txtSallary.Text = row_selected[10].ToString();
                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {

            cmd = new SqlCommand("Insert into Employee(Name,Surname,ID,SEX,Birth_Date,Age,Address,Mobile,DateOfReciving,Sallary,added_date) Values(@Name,@Surname,@ID,@SEX,@Birth_Date,@Age,@Address,@Mobile,@DateOfReciving,@Sallary,@added_date)", db.con);
            db.con.Open();
            cmd.Parameters.AddWithValue("@Name", txtFirstName.Text);
            cmd.Parameters.AddWithValue("@Surname", txtLastName.Text);
            cmd.Parameters.AddWithValue("@ID", txtIDNumber.Text);
            cmd.Parameters.AddWithValue("@SEX", cmbSex.Text);
            cmd.Parameters.AddWithValue("@Birth_Date", dtpBirth.Text);
            cmd.Parameters.AddWithValue("@Age", txtAge.Text);
            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
            cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
            cmd.Parameters.AddWithValue("@DateOfReciving", dtpReciving.Text);
            cmd.Parameters.AddWithValue("@Sallary", txtSallary.Text);
            cmd.Parameters.AddWithValue("@added_date", DateTime.Now);

            cmd.ExecuteNonQuery();
            db.con.Close();
            MessageBox.Show("Employee Added Succesfully");
            DisplayData();
            ClearData();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("Update Employee Set Name=@Name,Surname=@Surname,ID=@ID,SEX=@SEX,Birth_Date=@Birth_Date,Age=@Age,Address=@Address,Mobile=@Mobile,DateOfReciving=@DateOfReciving,Sallary=@Sallary,added_date=@added_date Where EmpID=@EmpID", db.con);
            db.con.Open();
            cmd.Parameters.AddWithValue("@EmpID", EmpID);
            cmd.Parameters.AddWithValue("@Name", txtFirstName.Text);
            cmd.Parameters.AddWithValue("@Surname", txtLastName.Text);
            cmd.Parameters.AddWithValue("@ID", txtIDNumber.Text);
            cmd.Parameters.AddWithValue("@SEX", cmbSex.Text);
            cmd.Parameters.AddWithValue("@Birth_Date", dtpBirth.Text);
            cmd.Parameters.AddWithValue("@Age", txtAge.Text);
            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
            cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
            cmd.Parameters.AddWithValue("@DateOfReciving", dtpReciving.Text);
            cmd.Parameters.AddWithValue("@Sallary", txtSallary.Text);
            cmd.Parameters.AddWithValue("@added_date", DateTime.Now);

            cmd.ExecuteNonQuery();
            db.con.Close();
            MessageBox.Show("Employee Updated Succesfully!");
            DisplayData();
            ClearData();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("Delete Employee Where EmpID=@EmpID", db.con);
            db.con.Open();
            cmd.Parameters.AddWithValue("@EmpID", EmpID);
            cmd.ExecuteNonQuery();
            db.con.Close();
            MessageBox.Show("Employee Deleted Succesfully");
            DisplayData();
            ClearData();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            frmDataReport frm = new frmDataReport(u, dc, this, p, dea);
            frm.LoadEmployeeReport();
            frm.ShowDialog();
        }

        private void txtIDNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtIDNumber.Text, "[^0-9]"))
            {
                MessageBox.Show("Please Enter Only Numbers");
                txtIDNumber.Text = txtIDNumber.Text.Remove(txtIDNumber.Text.Length - 1);
            }
        }

        private void txtMobile_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtMobile.Text, "[^0-9]"))
            {
                MessageBox.Show("Please Enter Only Numbers");
                txtMobile.Text = txtMobile.Text.Remove(txtMobile.Text.Length - 1);
            }
        }


        private void txtSallary_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtSallary.Text, "[^0-9]"))
            {
                MessageBox.Show("Please Enter Only Numbers");
                txtSallary.Text = txtSallary.Text.Remove(txtSallary.Text.Length - 1);
            }
        }

        private void txtSearchEmoloyee_TextChanged(object sender, TextChangedEventArgs e)
        {
            db.con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select * from Employee where Name like '" + txtSearchEmoloyee.Text + "%'", db.con);
            adapt.Fill(dt);
            grid_Employee.ItemsSource = dt.DefaultView;
            db.con.Close();
        }

    }
}
