using Project.BLL;
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

namespace Project.DAL
{
    class ProductDAL : DealCustDAL
    {
        MyConnection db = new MyConnection();
        #region Select Method for Product Module
        public DataTable Select()
        {

            DataTable dt = new DataTable();
            try
            {
                String sql = "select ProdID [PID], PCode, Barcode, Manufactor [Brand], Model, Full_Name [Full Name], Price, Category, Description, Year, Warranty, Quantity, Reorder [Reorder Level], Dealer, Img, added_time [Added Time], added_by [Added By]  from Product";

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
                String sql = "select ProdID [PID], Manufactor, Model, Full_Name [Full Name], Price, Category, Description, Year, Warranty, Quantity, Reorder [ Reorder Level], Dealer from vwCriticalItems";
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

        public bool Insert(ProductBLL p)
        {
            bool isSuccess = false;


            try
            {
                String sql = "INSERT INTO Product (PCode, Barcode, Manufactor, Model,Full_Name,Price,Category,Description,Year,Warranty,Quantity,Dealer, Img,added_time,added_by,Reorder) VALUES (@PCode, @Barcode, @Manufactor,@Model,@Full_Name,@Price,@Category,@Description,@Year,@Warranty,@Quantity,@Dealer,@Img,@added_time,@added_by,@Reorder)";
                SqlCommand cmd = new SqlCommand(sql, db.con);

                cmd.Parameters.AddWithValue("@PCode", p.PCode);
                cmd.Parameters.AddWithValue("@Barcode", p.Barcode);
                cmd.Parameters.AddWithValue("@Manufactor", p.Manufactor);
                cmd.Parameters.AddWithValue("@Model", p.Model);
                cmd.Parameters.AddWithValue("@Full_Name", p.Full_Name);
                cmd.Parameters.AddWithValue("@Price", p.Price);
                cmd.Parameters.AddWithValue("@Category", p.Category);
                cmd.Parameters.AddWithValue("@Description", p.Description);
                cmd.Parameters.AddWithValue("@Year", p.Year);
                cmd.Parameters.AddWithValue("@Warranty", p.Warranty);
                cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                cmd.Parameters.AddWithValue("@Dealer", p.Dealer);
                cmd.Parameters.AddWithValue("@Img", p.Img);
                cmd.Parameters.AddWithValue("@added_time", p.added_time);
                cmd.Parameters.AddWithValue("@added_by", p.added_by);
                cmd.Parameters.AddWithValue("@Reorder", p.Reorder);
                

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

        public bool Update(ProductBLL p)
        {
            bool isSuccess = false;
            try
            {
                if (MessageBox.Show("Click YES to save the changes", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    String sql = "UPDATE Product SET PCode=@PCode, Barcode=@Barcode, Manufactor=@Manufactor,Model=@Model,Full_Name=@Full_Name,Price=@Price,Category=@Category,Description=@Description,Year=@Year,Warranty=@Warranty,Dealer=@Dealer,Img = @Img,added_time=@added_time,added_by=@added_by,Reorder=@Reorder  WHERE ProdID=@ProdID";


                    SqlCommand cmd = new SqlCommand(sql, db.con);

                    cmd.Parameters.AddWithValue("@PCode", p.PCode);
                    cmd.Parameters.AddWithValue("@Barcode", p.Barcode);
                    cmd.Parameters.AddWithValue("@Manufactor", p.Manufactor);
                    cmd.Parameters.AddWithValue("@Model", p.Model);
                    cmd.Parameters.AddWithValue("@Full_Name", p.Full_Name);
                    cmd.Parameters.AddWithValue("@Price", p.Price);
                    cmd.Parameters.AddWithValue("@Category", p.Category);
                    cmd.Parameters.AddWithValue("@Description", p.Description);
                    cmd.Parameters.AddWithValue("@Year", p.Year);
                    cmd.Parameters.AddWithValue("@Warranty", p.Warranty);
                    cmd.Parameters.AddWithValue("@Dealer", p.Dealer);
                    cmd.Parameters.AddWithValue("@ProdID", p.ProdID);
                    cmd.Parameters.AddWithValue("@Img", p.Img);
                    cmd.Parameters.AddWithValue("@added_time", p.added_time);
                    cmd.Parameters.AddWithValue("@added_by", p.added_by);
                    cmd.Parameters.AddWithValue("@Reorder", p.Reorder);

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

        public bool Delete(ProductBLL p)
        {
            bool isSuccess = false;
            try
            {
                if(MessageBox.Show("Are you sure you want to Delete this record?  Click YES To Confirm",  "CONFIRM", MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                { 
                    String sql = "DELETE FROM Product WHERE ProdID=@ProdID";
                    SqlCommand cmd = new SqlCommand(sql, db.con);

                    cmd.Parameters.AddWithValue("@ProdID", p.ProdID);
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
        #region Search Product Method in Transaction Module
        public ProductBLL GetProductsForTransaction(string keyword)
        {
            ProductBLL p = new ProductBLL();
            DataTable dt = new DataTable();

            try
            {
                string sql = "SELECT Full_Name, Price, Quantity, Img FROM Product WHERE Barcode LIKE '%" + keyword + "%' OR PCode Like '%" + keyword + "%' OR Full_Name LIKE '%" + keyword + "%'";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, db.con);

                db.con.Open();

                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    p.Full_Name = dt.Rows[0]["Full_Name"].ToString();
                    p.Price = decimal.Parse(dt.Rows[0]["Price"].ToString());
                    p.Quantity = int.Parse(dt.Rows[0]["Quantity"].ToString());
                    p.Img = dt.Rows[0]["Img"].ToString();
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
        #region Method to get Product ID Base on Product Name
        public ProductBLL GetProductIDFromName(string ProductName)
        {
            //Create an object of DealCustBll 
            ProductBLL p = new ProductBLL();
            DataTable dt = new DataTable();

            try
            {
                string sql = "SELECT ProdID FROM Product WHERE Full_Name='" + ProductName + "'";
                //Create SQL Data Adapter
                SqlDataAdapter adapter = new SqlDataAdapter(sql, db.con);

                db.con.Open();

                //Passing the Value from Adapter to Datatabl
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //Pass the value from dt to DalCustBLL dc
                    p.ProdID = int.Parse(dt.Rows[0]["ProdID"].ToString());
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
                string sql = "SELECT Quantity FROM Product WHERE ProdID = " + ProductID;
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
                string sql = "UPDATE Product SET Quantity=@Quantity WHERE ProdID=@ProdID";

                //Create SQL Command to Pass the value into Query
                SqlCommand cmd = new SqlCommand(sql, db.con);
                //Passing the Value trhough parameters
                cmd.Parameters.AddWithValue("@Quantity", Qty);
                cmd.Parameters.AddWithValue("@ProdID", ProductID);

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



    }
}
