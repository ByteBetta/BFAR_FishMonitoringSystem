using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfPosApp.BLL;


namespace WpfPosApp.DAL
{
    class VesselDAL
    {
        MyConnection db = new MyConnection();


        #region SELECT Method for Vessels
        public DataTable Select()
        {
            //DataTable
            DataTable dt = new DataTable();

            try
            {
                //Write SQL Query to select all the data from database
                string sql = "SELECT VesselID, VesselName [Vessel Name], VesselOwner [Owner], VesselCode [Vessel Code], added_date [Added Time], added_by [Added By], DealID [Owner ID] FROM Vessel";

                //Creating sql command to execute query
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //creating sql data adapter
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open database
                db.con.Open();

                //Passing the value from SQL Data Adapter
                adapter.Fill(dt);

            }
            catch (Exception ex)
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
        #region INSERT Method to Add details for Dealers
        public bool Insert(VesselBLL vs)
        {
            bool isSuccess = false;

            try
            {
                string sql = "INSERT INTO Vessel (Vesselname, Vesselcode, Vesselowner,  added_date, added_by, DealID) VALUES (@vesselname, @vesselcode, @vesselowner, @added_date, @added_by, @DealID)";

         

                SqlCommand cmd = new SqlCommand(sql, db.con);

                cmd.Parameters.AddWithValue("@vesselname", vs.VesselName);
                cmd.Parameters.AddWithValue("@vesselcode", vs.VesselCode);
                cmd.Parameters.AddWithValue("@vesselowner", vs.VesselOwner);
                cmd.Parameters.AddWithValue("@added_date", vs.added_date);
                cmd.Parameters.AddWithValue("@added_by", vs.added_by);
                cmd.Parameters.AddWithValue("@DealID", vs.DealID);

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

        #region  UPDATE method for Vessel Module

        public bool Update(VesselBLL vesselBLL)
        {
            bool isSuccess = false;

            try
            {
                if (MessageBox.Show("Click YES to save the changes", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sql = "UPDATE Vessel SET VesselName=@VesselName, VesselCode=@VesselCode, VesselName=@VesselName, VesselOwner=@VesselOwner, added_date=@added_date, added_by=@added_by WHERE VesselID=@VesselID";
                    SqlCommand cmd = new SqlCommand(sql, db.con);

                    cmd.Parameters.AddWithValue("@VesselName", vesselBLL.VesselName);
                    cmd.Parameters.AddWithValue("@VesselCode", vesselBLL.VesselCode);
                    cmd.Parameters.AddWithValue("@VesselName", vesselBLL.VesselName);
                    cmd.Parameters.AddWithValue("@VesselOwner", vesselBLL.VesselOwner);
                    cmd.Parameters.AddWithValue("@added_date", vesselBLL.added_date);
                    cmd.Parameters.AddWithValue("@VesselID", vesselBLL.VesselName);
                    cmd.Parameters.AddWithValue("@added_by", vesselBLL.added_by);

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
        #region DELETE Method for Vessel Module
        public bool Delete(VesselBLL vs)
        {
            bool isSuccess = false;

            try
            {
                if (MessageBox.Show("Are you sure you want to delete this Record?  Click YES to confirm", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sql = "DELETE FROM Vessel WHERE VesselID=@VesselID";

                    SqlCommand cmd = new SqlCommand(sql, db.con);

                    cmd.Parameters.AddWithValue("@VesselID", vs.VesselID);

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
        #region Search Method for Dealers
        public DataTable Search(string keyword)
        {
            DataTable dt = new DataTable();

            try
            {
                // Write the Query to Search Dealer Based in id, type and name
                string sql = "SELECT * FROM Vessel WHERE VesselID LIKE '%" + keyword + "%' OR VesselName LIKE '%" + keyword + "%' OR VesselCode LIKE '%" + keyword + "%' ";

                //SQL command to Execute the Query
                SqlCommand cmd = new SqlCommand(sql, db.con);
                //SQL Data Adapter to hold the data from database temporarily
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open Data Base Connection
                db.con.Open();
                //Pass the value from adapter to data table
                adapter.Fill(dt);
            }
            catch (Exception ex)
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

        #region Getting Deal ID from OwnerName
        public VesselBLL GetIDFromOwnerName(string vesselowner)
        {
            VesselBLL u = new VesselBLL();
            MyConnection db = new MyConnection();
            DataTable dt = new DataTable();

            try
            {
                string sql = "SELECT DealID FROM Dealers WHERE name='" + vesselowner + "'";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, db.con);
                db.con.Open();

                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    u.DealID = int.Parse(dt.Rows[0]["DealID"].ToString());
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
            return u;
        }
        #endregion
    }


}
