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

    class TransactionDAL
    {
        public frmCashierDashboard frm;
        MyConnection db = new MyConnection();

    
        #region Insert Transaction Method
        public bool Insert_Transaction(TransactionBLL t)
        {
            //Create a boolean value and set its default value to false
            bool isSuccess = false;
            //Set the out transactionID value to negative 1 i.e. -1
          
            //Create a SqlConnection first
           
                
                //SQL Query to Insert Transactions
                string sql = "INSERT INTO tblTransaction (transno, fisherman, transaction_date , remark, vessels, gearUsed, landingSite, totalBox, totalSampleBox, totalWeightBox, totalSampleWeightBox, added_by) " +
                "VALUES (@transno, @fisherman, @transaction_date , @remark, @vessels, @gearUsed, @landingSite, @totalBox, @totalSampleBox,@totalWeightBox, @totalSampleWeightBox, @added_by);";

                //Sql Commandto pass the value in sql query
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //Passing the value to sql query using cmd
                cmd.Parameters.AddWithValue("@transno", t.transno);
                cmd.Parameters.AddWithValue("@fisherman", t.fisherman);
                cmd.Parameters.AddWithValue("@transaction_date", t.transactiondate);
                cmd.Parameters.AddWithValue("@remark", t.remark);
                cmd.Parameters.AddWithValue("@vessels", t.vessels);
                cmd.Parameters.AddWithValue("@gearUsed", t.gearUsed);
                cmd.Parameters.AddWithValue("@landingSite", t.landingSite);
                cmd.Parameters.AddWithValue("@totalBox", t.totalBox);
                cmd.Parameters.AddWithValue("@totalSampleBox", t.totalSampleBox);
                cmd.Parameters.AddWithValue("@totalWeightBox", t.totalWeightBox);
                cmd.Parameters.AddWithValue("@totalSampleWeightBox", t.totalSampleWeightBox);
                cmd.Parameters.AddWithValue("@added_by", t.added_by);

                //Open Database Connection
                db.con.Open();

                //Execute the Query
                object o = cmd.ExecuteScalar();

                //If the query is executed successfully then the value will not be null else it will be null
                if (o != null)
                {
                    //Query Executed Successfully
                    
                    isSuccess = true;
                }
                else
                {
                    //failed to execute query
                    isSuccess = false;
                }
            
                //Close the connection 
                db.con.Close();
            

            return isSuccess;
        }
        #endregion
      
        #region Method Display ALL The Transaction
        public DataTable DisplayAllTransactions()
        {

            //Create DataTable to hold the dataform database temporarily
            DataTable dt = new DataTable();
            try
            {
                //Write the SQL Query to Display all Transactions
                string sql = "SELECT tblTransaction.transno, Login.UserName FROM tblTransaction INNER JOIN Login ON tblTransaction.added_by = Login.UserID";

                //SQLCommand to Execute Query
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //SqlDataAdapter to Hold The Data from DataBase
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open Database Connection

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


        #region generate Reports

        public DataTable getNumberofBoats(string datemonth, string year, string landingsite)
        {
            // Create DataTable to hold the dataform database temporarily
             DataTable dt = new DataTable();
            try
            {
                //Write the SQL Query to Display all Transactions
                string sql = " SELECT Count(tblTransaction.transno) as NumberofBoat " +
                   " FROM[project].[dbo].tblTransaction " +
                      " WHERE MONTH(tblTransaction.[transaction_date]) = '" + datemonth + "' AND YEAR(tblTransaction.[transaction_date]) ='" + year + "'" +
                      landingsite.ToString();

                //SQLCommand to Execute Query
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //SqlDataAdapter to Hold The Data from DataBase
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open Database Connection

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

        public DataTable fishvolumeandweight(string datemonth, string year, string landingsite)
        {
            DataTable dt = new DataTable();
            try
            {
                //Write the SQL Query to Display all Transactions
                string sql = " SELECT Count(TransDetails.transno) as FishTotal, SUM(TransDetails.weight) as FishMeasured " +
                   " FROM [project].[dbo].TransDetails " +
                      " WHERE MONTH(TransDetails.[added_date]) = '" + datemonth + "' AND YEAR(TransDetails.[added_date]) ='" + year + "'" +
                       landingsite.ToString();

                //SQLCommand to Execute Query
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //SqlDataAdapter to Hold The Data from DataBase
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open Database Connection

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

        public DataTable topSpecies(string datemonth, string year, string landingsite)
        {
            DataTable dt = new DataTable();
            try
            {
                //Write the SQL Query to Display all Transactions
                string sql = "SELECT TOP 8 Species, SUM(quantity) as Total " +
                   " FROM [project].[dbo].TransDetails " +
                      " WHERE MONTH(TransDetails.[added_date]) = '" + datemonth + "' AND YEAR(TransDetails.[added_date]) ='" + year + "'" +
                      landingsite.ToString() +
                      " GROUP BY Species" +
                      "   ORDER BY Total desc";

                //SQLCommand to Execute Query
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //SqlDataAdapter to Hold The Data from DataBase
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open Database Connection

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

 
        public DataTable gearSortation(string datemonth, string year, string landingsite)
        {
            DataTable dt = new DataTable();
            try
            {
                //Write the SQL Query to Display all Transactions
                string sql = " SELECT tblTransaction.gearUsed as [Gear Used], Count(tblTransaction.vessels) as [Number Of Boats Landed], SUM(tblTransaction.totalWeightBox) as [Total Boat Catch (Kgs.)]" +
                   " FROM[project].[dbo].tblTransaction " +
                      " WHERE MONTH(tblTransaction.[transaction_date]) = '" + datemonth + "' AND YEAR(tblTransaction.[transaction_date]) ='" + year + "'" +
                      landingsite.ToString() +
                      "  GROUP by tblTransaction.gearUsed";

               
                //SQLCommand to Execute Query
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //SqlDataAdapter to Hold The Data from DataBase
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open Database Connection

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
        #region Method To Display Transaction Based On Transaction Type
        public DataTable DisplayTransactionByType(string type)
        {
            //Create DataTable 
            DataTable dt = new DataTable();

            try
            {
                //Write SQL Query
                string sql = "SELECT id [ID], type [Type], DealCustID, grandTotal [Grand Total], transaction_date [Transaction Time], tax [TAX], discount [Discount] FROM tblTransaction WHERE type ='"+type+"'";

                //SQL Command to Execute Query
                SqlCommand cmd = new SqlCommand(sql, db.con);
                //SqlDataAdapter to hold the data from Database
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open DataBase Connection
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
        #region Method To Display Transaction Based On Transaction Date
        public DataTable DisplayTransactionByDate(string transaction_date)
        {

            //Create DataTable to hold the dataform database temporarily
            DataTable dt = new DataTable();
            try
            {
                //Write the SQL Query to Display all Transactions
                string sql = "SELECT id [ID], type [Type], DealCustID, grandTotal [Grand Total], transaction_date [Transaction Time], tax [TAX], discount [Discount] FROM tblTransaction WHERE type ='" + transaction_date + "'";

                //SQLCommand to Execute Query
                SqlCommand cmd = new SqlCommand(sql, db.con);

                //SqlDataAdapter to Hold The Data from DataBase
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open Database Connection

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


      


    }
}
