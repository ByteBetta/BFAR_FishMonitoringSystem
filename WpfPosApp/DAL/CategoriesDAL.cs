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
    class CategoriesDAL
    {
        //Static String Method for Database Connection String
       


        #region Select Method
        public DataTable Select()
        {
            //Creating Database Connection
            MyConnection db = new MyConnection();

            DataTable dt = new DataTable();

            try
            {
                //Wrting SQL Query to get all the data from DAtabase
                string sql = "SELECT CatID, title [Title], description [Description], added_date [Added Time], added_by [Added By]  FROM Category";

                SqlCommand cmd = new SqlCommand(sql, db.con);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //Open DAtabase Connection
                db.con.Open();
                //Adding the value from adapter to Data TAble dt
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
        #region Insert New CAtegory
        public bool Insert(CategoriesBLL c)
        {
            //Creating A Boolean VAriable and set its default value to false
            bool isSucces = false;

            //Connecting to Database
            MyConnection db = new MyConnection();


            try
            {
                //Writing Query to Add New Category
                string sql = "INSERT INTO Category (title, description, added_date, added_by) VALUES (@title, @description, @added_date, @added_by)";

                //Creating SQL Command to pass values in our query
                SqlCommand cmd = new SqlCommand(sql, db.con);
                //Passing Values through parameter
                cmd.Parameters.AddWithValue("@title", c.title);
                cmd.Parameters.AddWithValue("@description", c.description);
                cmd.Parameters.AddWithValue("@added_date", c.added_date);
                cmd.Parameters.AddWithValue("@added_by", c.added_by);

                //Open Database Connection
                db.con.Open();

                //Creating the int variable to execute query
                int rows = cmd.ExecuteNonQuery();

                //If the query is executed successfully then its value will be greater than 0 else it will be less than 0

                if (rows > 0)
                {
                    //Query Executed Succesfully
                    isSucces = true;
                }
                else
                {
                    //Failed to Execute Query
                    isSucces = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Closing Database Connection
                db.con.Close();
            }

            return isSucces;
        }
        #endregion
        #region Update Method
        public bool Update(CategoriesBLL c)
        {
            //Creating Boolean variable and set its default value to false
            bool isSuccess = false;

            //Creating SQL Connection
            MyConnection db = new MyConnection();

            try
            {
                if (MessageBox.Show("Click YES to save the changes", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //Query to Update Category
                    string sql = "UPDATE Category SET title=@title, description=@description, added_date=@added_date, added_by=@added_by WHERE CatID=@CatID";

                //SQl Command to Pass the Value on Sql Query
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //Passing Value using cmd
                cmd.Parameters.AddWithValue("@title", c.title);
                cmd.Parameters.AddWithValue("@description", c.description);
                cmd.Parameters.AddWithValue("@added_date", c.added_date);
                cmd.Parameters.AddWithValue("@added_by", c.added_by);
                cmd.Parameters.AddWithValue("@CatID", c.CatID);

                //Open DAtabase Connection
                db.con.Open();

                //Create Int Variable to execute query
                int rows = cmd.ExecuteNonQuery();

                    //if the query is successfully executed then the value will be grater than zero 
                    if (rows > 0)
                    {
                        //Query Executed Successfully
                        isSuccess = true;
                    }
                    else
                    {
                        //Failed to Execute Query
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
        #region Delete Category Method
        public bool Delete(CategoriesBLL c)
        {
            //Create a Boolean variable and set its value to false
            bool isSuccess = false;
            MyConnection db = new MyConnection();
            try
            {
                if (MessageBox.Show("Are you sure you want to delete this Record?  Click YES to confirm", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //SQL Query to Delete from Database
                    string sql = "DELETE FROM Category WHERE CatID=@CatID";

                    SqlCommand cmd = new SqlCommand(sql, db.con);
                    //Passing the value using cmd
                    cmd.Parameters.AddWithValue("@CatID", c.CatID);

                    //Open SqlConnection
                    db.con.Open();

                    int rows = cmd.ExecuteNonQuery();

                    //If the query is executd successfully then the value of rows will be greater than zero else it will be less than 0
                    if (rows > 0)
                    {
                        //Query Executed Successfully
                        isSuccess = true;
                    }
                    else
                    {
                        //Faied to Execute Query
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
        #region Method for Search Funtionality
        public DataTable Search(string keywords)
        {
            //SQL Connection For Database Connection
            MyConnection db = new MyConnection();

            //Creating Data TAble to hold the data from database temporarily
            DataTable dt = new DataTable();

            try
            {
                //SQL Query To Search Categories from DAtabase
                String sql = "SELECT * FROM Category WHERE CatID LIKE '%" + keywords + "%' OR title LIKE '%" + keywords + "%' OR description LIKE '%" + keywords + "%'";
                //Creating SQL Command to Execute the Query
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //Getting DAta From DAtabase
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open DatabaseConnection
                db.con.Open();
                //Passing values from adapter to Data Table dt
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
    }
}
