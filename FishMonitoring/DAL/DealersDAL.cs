using Firebase.Storage;
using Google.Cloud.Firestore;
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
using WpfPosApp.BLL;

namespace WpfPosApp.DAL
{
    class DealersDAL
    {
        MyConnection db = new MyConnection();
        FirestoreDb firestoreDatabase;

        #region SELECT MEthod for Dealers
        public DataTable Select()
        {
            //DataTable
            DataTable dt = new DataTable();

            try
            {
                //Write SQL Query to select all the data from database
                string sql = "SELECT DealID, name [Company Name], person , email [Email], contact [Mobile], address [Address], added_date [Added Time], added_by [Added By] FROM Dealers";

                //Creating sql command to execute quer
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
        public bool Insert(DealersBLL dc)
        {
            bool isSuccess = false;

            try
            {
                string sql = "INSERT INTO Dealers (name, person, email, contact, address, added_date, added_by) VALUES (@name, @person, @email, @contact, @address, @added_date, @added_by)";

                SqlCommand cmd = new SqlCommand(sql, db.con);

                cmd.Parameters.AddWithValue("@name", dc.name);
                cmd.Parameters.AddWithValue("@person", dc.person);
                cmd.Parameters.AddWithValue("@email", dc.email);
                cmd.Parameters.AddWithValue("@contact", dc.contact);
                cmd.Parameters.AddWithValue("@address", dc.address);
                cmd.Parameters.AddWithValue("@added_date", dc.added_date);
                cmd.Parameters.AddWithValue("@added_by", dc.added_by);

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
        #region  UPDATE method for Dealer and Customer Module

        public bool Update(DealersBLL dc)
        {
            bool isSuccess = false;

            try
            {
                if (MessageBox.Show("Click YES to save the changes", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sql = "UPDATE Dealers SET name=@name, person=@person, email=@email, contact=@contact, address=@address, added_date=@added_date, added_by=@added_by WHERE DealID=@DealID";
                    SqlCommand cmd = new SqlCommand(sql, db.con);

                    cmd.Parameters.AddWithValue("@name", dc.name);
                    cmd.Parameters.AddWithValue("@person", dc.person);
                    cmd.Parameters.AddWithValue("@email", dc.email);
                    cmd.Parameters.AddWithValue("@contact", dc.contact);
                    cmd.Parameters.AddWithValue("@address", dc.address);
                    cmd.Parameters.AddWithValue("@added_date", dc.added_date);
                    cmd.Parameters.AddWithValue("@DealID", dc.DealID);
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
        public bool Delete(DealersBLL dc)
        {
            bool isSuccess = false;

            try
            {
                if (MessageBox.Show("Are you sure you want to delete this Record?  Click YES to confirm", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sql = "DELETE FROM Dealers WHERE DealID=@DealID";

                    SqlCommand cmd = new SqlCommand(sql, db.con);

                    cmd.Parameters.AddWithValue("@DealID", dc.DealID);

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
                string sql = "SELECT * FROM Dealers WHERE DealID LIKE '%" + keyword + "%' OR name LIKE '%" + keyword + "%' OR person LIKE '%" + keyword + "%' ";

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
        #region Method TO Search Dealer OR Customer From Transaction
        public DealersBLL SearchDealerForTransaction(string keyword)
        {
            DealersBLL dc = new DealersBLL();
            DataTable dt = new DataTable();

            try
            {
                string sql = "SELECT name [Name], person [Person], email [Email], contact [Contact], address [Address] from Dealers WHERE DealID LIKE '%" + keyword + "%' OR name LIKE '%" + keyword + "%' OR person LIKE '%" + keyword + "%'";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, db.con);

                db.con.Open();

                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dc.name = dt.Rows[0]["name"].ToString();
                    dc.person = dt.Rows[0]["person"].ToString();
                    dc.email = dt.Rows[0]["email"].ToString();
                    dc.contact = dt.Rows[0]["contact"].ToString();
                    dc.address = dt.Rows[0]["address"].ToString();
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

            return dc;
        }
        #endregion
        #region Method to Get ID OF THE Dealer OR Customer Based on Name
        public DealersBLL getIDofFisherman(string Name)
        {
            //Create an object of DealCustBll 
            DealersBLL dc = new DealersBLL();
            DataTable dt = new DataTable();

            try
            {
                string sql = "SELECT DealID FROM Dealers WHERE name='" + Name + "'";
                //Create SQL Data Adapter
                SqlDataAdapter adapter = new SqlDataAdapter(sql, db.con);

                db.con.Open();

                //Passing the Value from Adapter to Datatabl
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //Pass the value from dt to DalCustBLL dc
                    dc.DealID = int.Parse(dt.Rows[0]["DealID"].ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Form");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.con.Close();
            }


            return dc;
        }
        #endregion

        #region Method to Retrieve all the Vessels of the Company
        public DataTable GetVesselFromFisherman(string Name)
        {
            //Create an object of DealCustBll 
            DealersBLL deal = new DealersBLL();
            DataTable dt = new DataTable();

            try
            {
                string sql = "SELECT VesselName FROM Vessel WHERE VesselOwner='" + Name + "'";
                //Create SQL Data Adapter
                SqlDataAdapter adapter = new SqlDataAdapter(sql, db.con);

                db.con.Open();

                //Passing the Value from Adapter to Datatabl
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    
                    
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


            return dt;
        }
        #endregion
        public void ConnecttoFirebase()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"bfar-testproj.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            firestoreDatabase = FirestoreDb.Create("bfar-testproj");

        }





        public async void AddFishtoFirebaseAsync(DealersBLL fisherman)
        {

            try
            {
                DataTable vessels = GetVesselFromFisherman(fisherman.name);
                string[] arrray = vessels.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                DocumentReference usercollection = firestoreDatabase.Collection("Fisherman").Document(fisherman.DealID.ToString());
                Dictionary<string, object> userdata = new Dictionary<string, object>()
            {
                    { nameof(fisherman.DealID).ToString() , fisherman.DealID.ToString()},
                    { nameof(fisherman.name).ToString() , fisherman.name.ToString()},
                    { nameof(fisherman.person).ToString() , fisherman.person.ToString()},
                    { nameof(fisherman.email).ToString() , fisherman.email.ToString()},
                    { nameof(fisherman.address).ToString() , fisherman.address.ToString()},
                    { nameof(fisherman.contact).ToString() , fisherman.contact.ToString()},
                    { "Vessels", arrray }

            };
                Console.WriteLine(nameof(fisherman.DealID).ToString());
                await usercollection.SetAsync(userdata);


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }



    }

}
