using Project.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfPosApp;

namespace Project.DAL
{
    class DealCustDAL
    {
        MyConnection db = new MyConnection();
        #region SELECT MEthod for Dealer and Customer
        public DataTable Select()
        {

            //DataTable
            DataTable dt = new DataTable();

            try
            {
                //Write SQL Query to select all the data from database
                string sql = "SELECT DealCustID [CID], name [Name], surname [Surname], email [Email], contact [Mobile], address [Address] , added_date [Added Time], added_by [Added By] FROM DealCust";

                //Creating sql command to execute quer
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //creating sql data adapter
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open database
                db.con.Open();

                //Passing the value from SQL Data Adapter
                adapter.Fill(dt);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.con.Close();
            }

            return dt;

            
        }
        #endregion
        #region INSERT Method to Add details for Dealer or Customer
        public bool Insert(DealCustBLL dc)
        {
            //Creating SQL Connection First

            bool isSuccess = false;

            try
            {
                string sql = "INSERT INTO DealCust (name, surname, email, contact, address, added_date, added_by) VALUES (@name, @surname, @email, @contact, @address, @added_date, @added_by)";

                SqlCommand cmd = new SqlCommand(sql, db.con);

                cmd.Parameters.AddWithValue("@name", dc.name);
                cmd.Parameters.AddWithValue("@surname", dc.surname);
                cmd.Parameters.AddWithValue("@email", dc.email);
                cmd.Parameters.AddWithValue("@contact", dc.contact);
                cmd.Parameters.AddWithValue("@address", dc.address);
                cmd.Parameters.AddWithValue("@added_date", dc.added_date);
                cmd.Parameters.AddWithValue("@added_by", dc.added_by);

                db.con.Open();

                int rows = cmd.ExecuteNonQuery();

                if(rows>0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in DealCu");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.con.Close();
            }

            return isSuccess;
        }
        #endregion
        #region  UPDATE method for Dealer and Customer Module
        
        public bool Update(DealCustBLL dc)
        {

            bool isSuccess = false;

            try
            {
                if (MessageBox.Show("Click YES to save the changes", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sql = "UPDATE DealCust SET name=@name, surname=@surname, email=@email, contact=@contact, address=@address, added_date=@added_date, added_by=@added_by WHERE DealCustID=@DealCustID";
                    SqlCommand cmd = new SqlCommand(sql, db.con);

                    cmd.Parameters.AddWithValue("@name", dc.name);
                    cmd.Parameters.AddWithValue("@surname", dc.surname);
                    cmd.Parameters.AddWithValue("@email", dc.email);
                    cmd.Parameters.AddWithValue("@contact", dc.contact);
                    cmd.Parameters.AddWithValue("@address", dc.address);
                    cmd.Parameters.AddWithValue("@added_date", dc.added_date);
                    cmd.Parameters.AddWithValue("@DealCustID", dc.DealCustID);
                    cmd.Parameters.AddWithValue("@added_by", dc.added_by);

                    //Open the Database Conncetion
                   db.con.Open();

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = false;
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.con.Close();
            }

            return isSuccess;

        }

        #endregion
        #region DELETE Method for Dealer and Customer Module
       public bool Delete(DealCustBLL dc)
        {
            bool isSuccess = false;

            try
            {
                if (MessageBox.Show("Are you sure you want to Delete this record?  Click YES To Confirm", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sql = "DELETE FROM DealCust WHERE DealCustID=@DealCustID";

                    SqlCommand cmd = new SqlCommand(sql, db.con);

                    cmd.Parameters.AddWithValue("@DealCustID", dc.DealCustID);

                    db.con.Open();

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        isSuccess = true;

                    }
                    else
                    {
                        isSuccess = false;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.con.Close();
            }

            return isSuccess;
        }
        #endregion
        #region Search Method for Dealer and Customer Module
        public DataTable Search(string keyword)
        {
            DataTable dt = new DataTable();

            try
            {
                // Write the Query to Search Dealer or Customer Based in id, type and name
                string sql = "SELECT * FROM DealCust WHERE DealCustID LIKE '%" + keyword + "%'  OR name LIKE '%" + keyword + "%' OR surname LIKE '%" + keyword + "%'";

                //SQL command to Execute the Query
                SqlCommand cmd = new SqlCommand(sql, db.con);
                //SQL Data Adapter to hold the data from database temporarily
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open Data Base Connection
                db.con.Open();
                //Pass the value from adapter to data table
                adapter.Fill(dt);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.con.Close();
            }

            return dt;
            
        }
        #endregion
        #region Method TO Search Dealer OR Customer From Transaction
        public DealCustBLL SearchDealerCustomerForTransaction(string keyword)
        {
            DealCustBLL dc = new DealCustBLL();
            DataTable dt = new DataTable();


            try
            {
                string sql = "SELECT name [Name], surname [Surname], email [Email], contact [Contact], address [Address] from DealCust WHERE DealCustID LIKE '%" + keyword + "%' OR name LIKE '%" + keyword + "%' OR surname LIKE '%" + keyword + "%'";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, db.con);

                db.con.Open();

                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dc.name = dt.Rows[0]["name"].ToString();
                    dc.surname = dt.Rows[0]["Surname"].ToString();
                    dc.email = dt.Rows[0]["email"].ToString();
                    dc.contact = dt.Rows[0]["contact"].ToString();
                    dc.address = dt.Rows[0]["address"].ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.con.Close();
            }

            return dc;
        }
        #endregion
        #region Method to Get ID OF THE Dealer OR Customer Based on Name
        public DealCustBLL getIDofFisherman(string Name)
        {
            //Create an object of DealCustBll 
            DealCustBLL dc = new DealCustBLL();
            DataTable dt = new DataTable();

            try
            {
                string sql = "SELECT DealCustID FROM DealCust WHERE name='"+Name+"'";
                //Create SQL Data Adapter
                SqlDataAdapter adapter = new SqlDataAdapter(sql, db.con);

                db.con.Open();

                //Passing the Value from Adapter to Datatabl
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //Pass the value from dt to DalCustBLL dc
                    dc.DealCustID = int.Parse(dt.Rows[0]["DealCustID"].ToString());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.con.Close();
            }


            return dc;
        }
        #endregion
    }
}
