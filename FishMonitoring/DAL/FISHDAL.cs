using Fish.DAL;
using Fish.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using WpfPosApp;
using Google.Cloud.Firestore;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Firebase.Storage;
using Image = System.Drawing.Image;
using System.Reflection;
using Dapper;
using System.Windows.Media.Imaging;
using System.Threading;

namespace Fish.DAL
{
    class FishDAL
    {
        MyConnection db = new MyConnection();
        FirestoreDb firestoreDatabase;

        #region Select Method for Product Module
        public DataTable Select()
        {

            DataTable dt = new DataTable();
            try
            {
                const String sql = "select FishID [FishID], Species [Species], OrderName [Order Name], FamilyName [Family Name], LocalName [Local Name], FishBaseName [FishBase Name], ShortDescription [ShortDescription], Biology [Biology], Measurement [Measurement], Distribution [Distribution], Environment [Environment], Occurrence [Occurrence], Img, added_time [Added Time], added_by [Added By]  from FishDetails";

                SqlCommand cmd = new SqlCommand(sql, db.con);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                db.con.Open();

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


        #region Display CriticalItems
        public DataTable SelectCritical()
        {

            DataTable dt = new DataTable();
            try
            {
                String sql = "select FishID [PID], Manufactor, Model, Full_Name [Full Name], Price, Category, Description, Year, Warranty, Quantity, Reorder [ Reorder Level], Dealer from vwCriticalItems";
                SqlCommand cmd = new SqlCommand(sql, db.con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                db.con.Open();
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

        #region Insert Method for Product Module

        public bool Insert(FishBLL p)
        {
            bool isSuccess = false;


            try
            {
                
                String sql = "INSERT INTO FishDetails (Species, ShortDescription, Biology, Measurement, OrderName, FamilyName, LocalName, Distribution, Environment, FishBaseName, Occurrence, Img, added_time, added_by) " +
                "VALUES (@Species, @ShortDescription, @Biology, @Measurement, @OrderName, @FamilyName, @LocalName, @Distribution, @Environment, @FishBaseName," +
                "@Occurrence, @Img, @added_time, @added_by)";
                SqlCommand cmd = new SqlCommand(sql, db.con);

                cmd.Parameters.AddWithValue("@Species", p.Species);
                cmd.Parameters.AddWithValue("@ShortDescription", p.ShortDescription);
                cmd.Parameters.AddWithValue("@Biology", p.Biology);
                cmd.Parameters.AddWithValue("@Measurement", p.Measurement);
                cmd.Parameters.AddWithValue("@OrderName", p.OrderName);
                cmd.Parameters.AddWithValue("@FamilyName", p.FamilyName);
                cmd.Parameters.AddWithValue("@Distribution", p.Distribution);
                cmd.Parameters.AddWithValue("@LocalName", p.LocalName);
                cmd.Parameters.AddWithValue("@Environment", p.Environment);
                cmd.Parameters.AddWithValue("@FishBaseName", p.FishBaseName);
                cmd.Parameters.AddWithValue("@Occurrence", p.Occurance);
                cmd.Parameters.AddWithValue("@Img", p.Img);
                cmd.Parameters.AddWithValue("@added_time", p.added_time);
                cmd.Parameters.AddWithValue("@added_by", p.added_by);

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
        #region Update Method for Product Module

        public bool Update(FishBLL p)
        {
            bool isSuccess = false;
            try
            {
                if (MessageBox.Show("Click YES to save the changes", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    String sql = "UPDATE FishDetails SET Species=@Species, ShortDescription=@ShortDescription, Biology=@Biology, Measurement=@Measurement, " +
                        " OrderName=@OrderName, FamilyName=@FamilyName, LocalName=@LocalName, Distribution=@Distribution," +
                        " Environment=@Environment ,FishBaseName = @FishBaseName ,added_time=@added_time,added_by=@added_by, Occurrence=@Occurrence, Img=@Img Where FishID=@FishID";


                    SqlCommand cmd = new SqlCommand(sql, db.con);

                    cmd.Parameters.AddWithValue("@FishID", p.FishID);
                    cmd.Parameters.AddWithValue("@Species", p.Species);
                    cmd.Parameters.AddWithValue("@ShortDescription", p.ShortDescription);
                    cmd.Parameters.AddWithValue("@Biology", p.Biology);
                    cmd.Parameters.AddWithValue("@Measurement", p.Measurement);
                    cmd.Parameters.AddWithValue("@OrderName", p.OrderName);
                    cmd.Parameters.AddWithValue("@FamilyName", p.FamilyName);
                    cmd.Parameters.AddWithValue("@LocalName", p.LocalName);
                    cmd.Parameters.AddWithValue("@Distribution", p.Distribution);
                    cmd.Parameters.AddWithValue("@Environment", p.Environment);
                    cmd.Parameters.AddWithValue("@FishBaseName", p.FishBaseName);
                    cmd.Parameters.AddWithValue("@Occurrence", p.Occurance);
                    cmd.Parameters.AddWithValue("@Img", p.Img);
                    cmd.Parameters.AddWithValue("@added_time", p.added_time);
                    cmd.Parameters.AddWithValue("@added_by", p.added_by);


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
        #region Delete Method for Product Module

        public bool Delete(FishBLL p)
        {
            bool isSuccess = false;
            try
            {
                if(MessageBox.Show("Are you sure you want to Delete this record?  Click YES To Confirm",  "CONFIRM", MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                { 
                    String sql = "DELETE FROM FishDetails WHERE FishID=@FishID";
                    SqlCommand cmd = new SqlCommand(sql, db.con);

                    cmd.Parameters.AddWithValue("@FishID", p.FishID);
                    db.con.Open();

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        isSuccess = true;
                        this.deleteDatainFirestore(p.FishID);
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
        #region Search Product Method in Transaction Module
        public FishBLL GetProductsForTransaction(string keyword)
        {
            FishBLL p = new FishBLL();
            DataTable dt = new DataTable();

            try
            {
                string sql = "SELECT FishName, ScientificName, Salinity, Img FROM FishDetails WHERE ScientificName Like '%" + keyword + "%' OR FishName LIKE '%" + keyword + "%'";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, db.con);

                db.con.Open();

                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                   // p.FishName = dt.Rows[0]["FishName"].ToString();
                    //p.ScientificName = dt.Rows[0]["ScientificName"].ToString();
                    //p.Img = dt.Rows[0]["Img"].ToString();
                    //p.Salinity = dt.Rows[0]["Salinity"].ToString();
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

            return p;
        }


        #endregion
        #region Method to get Fish ID Base on Fish Name
        public FishBLL GetFishIDFromName(string FishName)
        {
            //Create an object of DealCustBll 
            FishBLL p = new FishBLL();
            DataTable dt = new DataTable();

            try
            {
                string sql = "SELECT FishID FROM FishDetails WHERE Species='" + FishName + "'";
                //Create SQL Data Adapter
                SqlDataAdapter adapter = new SqlDataAdapter(sql, db.con);

                db.con.Open();

                //Passing the Value from Adapter to Datatabl
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //Pass the value from dt to DalCustBLL dc
                    p.FishID = int.Parse(dt.Rows[0]["FishID"].ToString());
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


            return p;
        }
        #endregion
        #region Meethod To Get Current Quantity from the Database based on Product ID
        public decimal GetProductQty(int ProductID)
        {
            //Create a Decimal variable and set it's default value to 0
            decimal Quantity = 0;

            //Create Data Table to Save the data from Database temporarily
            DataTable dt = new DataTable();

            try
            {
                //Write SQL Query to Get Quantity from Database
                string sql = "SELECT Quantity FROM Product WHERE FishID = " + ProductID;
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //Create a SQL Data  Adapter to Execute the query
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open Database Connection
                db.con.Open();

                //Pass the Value from Data Adapter to Data Table
                adapter.Fill(dt);

                //Lets check if the datatable has value or not
                if(dt.Rows.Count > 0)
                {
                    Quantity = decimal.Parse(dt.Rows[0]["Quantity"].ToString());
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

            return Quantity;

        }
        #endregion
        #region Method to Update Quantity
        public bool UpdateQuantity(int ProductID, decimal Qty)
        {
            //Create a Boolean Variable and Srt Its value to false
            bool success = false;
            try
            {
                //Write the SQL Query to Update Qty
                string sql = "UPDATE FishDetails SET Quantity=@Quantity WHERE FishID=@FishID";

                //Create SQL Command to Pass the value into Query
                SqlCommand cmd = new SqlCommand(sql, db.con);
                //Passing the Value trhough parameters
                cmd.Parameters.AddWithValue("@Quantity", Qty);
                cmd.Parameters.AddWithValue("@FishID", ProductID);

                //Open Database Connection
                db.con.Open();

                //Create Int Variable and Check whether the query is executed Successfully or not
                int rows = cmd.ExecuteNonQuery();

                //Lets check if the query is executed Succesfully or not
                if(rows > 0)
                {
                    //Query Executed Succesfully
                    success = true;
                }
                else
                {
                    //Failed to Execute Query
                    success = false;
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

            return success;
        }
        #endregion
        #region Method to Increase Product
        public bool IncreaseProduct(int ProductID, decimal IncreaseQty)
        {
            //Create a Boolean Variable and Set it's Value to False
            bool success = false;
            try
            {
                //Get the current Qty From DataBase Based on id
                decimal currentQty = GetProductQty(ProductID);

                //Increase the Current Quantity by the qty Purchased from Dealer
                decimal NewQty = currentQty + IncreaseQty;

                //Update the product Quantity Now
                success = UpdateQuantity(ProductID, NewQty);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.con.Close();
            }

            return success;
        }
        #endregion
        #region Method to Decrase Product
        public bool DecraseProduct(int ProductID, decimal Qty)
        {
            //Create Boolear Variable and SET its Value to False
            bool success = false;

            try
            {
                //Get the Current Product Quantity
                decimal currentQty = GetProductQty(ProductID);

                //Decrase the product Quantity
                decimal NewQty = currentQty - Qty;

                //Update Product in DataBase
                success = UpdateQuantity(ProductID, NewQty);

      
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.con.Close();
            }

            return success;
        }
        #endregion
        #region Display Products Based On Categories
        public DataTable DisplayProductsByCategory(string category)
        {
            DataTable dt = new DataTable();
            try
            {
                //SQL Query to Display Product Based on Category
                string sql = "SELECT * FROM Product WHERE Category='" + category + "'";

                SqlCommand cmd = new SqlCommand(sql, db.con);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open Database Connection Here
                db.con.Open();

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

        

        public void ConnecttoFirebase()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"bfar-testproj.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            firestoreDatabase = FirestoreDb.Create("bfar-testproj");
            MessageBox.Show("Connection Success");
        }



        

        public async void AddFishtoFirebaseAsync(FishBLL fishdata)
        {
            string paths = System.Windows.Forms.Application.StartupPath.Substring(0, System.Windows.Forms.Application.StartupPath.Length - 10);
            Console.WriteLine(fishdata.Img.ToString());
            string imagePath = paths + "\\Images\\Product\\" + fishdata.Img.ToString();


            using (var stream = File.Open(imagePath, FileMode.Open))
            {
                var task = new FirebaseStorage("bfar-testproj.appspot.com")
                         .Child("data")
                         .Child("random")
                         .Child(fishdata.Img.ToString() + ".png")
                         .PutAsync(stream);

                task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

                try
                {
                    DocumentReference usercollection = firestoreDatabase.Collection("FishList").Document(fishdata.FishID.ToString());
                    Dictionary<string, object> userdata = new Dictionary<string, object>()
            {
                    { nameof(fishdata.FishID).ToString() , fishdata.FishID.ToString()},
                    { nameof(fishdata.Species).ToString() , fishdata.Species.ToString()},
                     { nameof(fishdata.ShortDescription).ToString() , fishdata.ShortDescription.ToString()},
                    { nameof(fishdata.Biology).ToString() , fishdata.Biology.ToString()},
                    { nameof(fishdata.Measurement).ToString() , fishdata.Measurement.ToString()},
                    { nameof(fishdata.OrderName).ToString() , fishdata.OrderName.ToString()},
                    { nameof(fishdata.FamilyName).ToString() , fishdata.FamilyName.ToString()},
                    { nameof(fishdata.LocalName).ToString() , fishdata.LocalName.ToString()},
                    { nameof(fishdata.Distribution).ToString() , fishdata.Distribution.ToString()},
                    { nameof(fishdata.Environment).ToString() , fishdata.Environment.ToString()},
                    { nameof(fishdata.FishBaseName).ToString() , fishdata.FishBaseName.ToString()},
                    { nameof(fishdata.Occurance).ToString() , fishdata.Occurance.ToString()},
                    { nameof(fishdata.Img).ToString(),  await task},

            };
                    Console.WriteLine(nameof(fishdata.FishID).ToString());
                    await usercollection.SetAsync(userdata);

                    MessageBox.Show("Uploading Success");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

        }


        public IList<FishBLL> GetData()
        {
            const String sql = "select FishID [FishID], Species [Species], OrderName [OrderName], FamilyName [FamilyName], LocalName [LocalName], FishBaseName [FishBaseName], ShortDescription [ShortDescription], Biology [Biology], Measurement [Measurement], Distribution [Distribution], Environment [Environment], Occurrence [Occurrence], Img, added_time [Added Time], added_by [Added By]  from FishDetails";
            db.con.Open();
            IList<FishBLL> dataholder = db.con.Query<FishBLL>(sql).ToList();
            db.con.Close();
            return dataholder;
        }

        public void deleteDatainFirestore(int id)
        {
            this.ConnecttoFirebase();
            DocumentReference docref = firestoreDatabase.Collection("FishList").Document(id.ToString());
            docref.DeleteAsync();
            MessageBox.Show("Delete Success");
        }

        public async void updateFirestore(FishBLL fishdata)
        {
            this.ConnecttoFirebase();
            string paths = System.Windows.Forms.Application.StartupPath.Substring(0, System.Windows.Forms.Application.StartupPath.Length - 10);
            Console.WriteLine(fishdata.Img.ToString());
            string imagePath = paths + "\\Images\\Product\\" + fishdata.Img.ToString();


            using (var stream = File.Open(imagePath, FileMode.Open))
            {
                var task = new FirebaseStorage("bfar-testproj.appspot.com")
                         .Child("data")
                         .Child("random")
                         .Child(fishdata.Img.ToString() + ".png")
                         .PutAsync(stream);

                task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

                try
                {
                    DocumentReference usercollection = firestoreDatabase.Collection("FishList").Document(fishdata.FishID.ToString());
                    Dictionary<string, object> userdata = new Dictionary<string, object>()
            {
                    { nameof(fishdata.FishID).ToString() , fishdata.FishID.ToString()},
                    { nameof(fishdata.Species).ToString() , fishdata.Species.ToString()},
                     { nameof(fishdata.ShortDescription).ToString() , fishdata.ShortDescription.ToString()},
                    { nameof(fishdata.Biology).ToString() , fishdata.Biology.ToString()},
                    { nameof(fishdata.Measurement).ToString() , fishdata.Measurement.ToString()},
                    { nameof(fishdata.OrderName).ToString() , fishdata.OrderName.ToString()},
                    { nameof(fishdata.FamilyName).ToString() , fishdata.FamilyName.ToString()},
                    { nameof(fishdata.LocalName).ToString() , fishdata.LocalName.ToString()},
                    { nameof(fishdata.Distribution).ToString() , fishdata.Distribution.ToString()},
                    { nameof(fishdata.Environment).ToString() , fishdata.Environment.ToString()},
                    { nameof(fishdata.FishBaseName).ToString() , fishdata.FishBaseName.ToString()},
                    { nameof(fishdata.Occurance).ToString() , fishdata.Occurance.ToString()},
                    { nameof(fishdata.Img).ToString(),  await task},

            };
                    Console.WriteLine(nameof(fishdata.FishID).ToString());
                    await usercollection.UpdateAsync(userdata);

                    MessageBox.Show("Uploading Success");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

    }



}

