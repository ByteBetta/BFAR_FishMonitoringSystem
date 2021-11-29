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
    class TransDetailsDAL
    {
        MyConnection db = new MyConnection();
        #region Insert Method for Transactions Details
        public bool InsertTransDetails(TransDetailsBLL td)
        {
            bool isSuccess = false;

            try
            {
                string sql = "INSERT INTO TransDetails (Species, transno, weight, length, added_date, vessels, fisherman, quantity, UID, gearUsed, landingSite) VALUES ( @Species, @transno, @weight, @length, @added_date, @vessels, @fisherman, @quantity, @UID, @gearUsed, @landingSite)";

                SqlCommand cmd = new SqlCommand(sql, db.con);

               
                cmd.Parameters.AddWithValue("@Species", td.Species);
                cmd.Parameters.AddWithValue("@transno", td.transno);
                cmd.Parameters.AddWithValue("@weight", td.weight);
                cmd.Parameters.AddWithValue("@length", td.length);
                cmd.Parameters.AddWithValue("@added_date", td.added_date);
                cmd.Parameters.AddWithValue("@DealID", td.DealID);
                cmd.Parameters.AddWithValue("@vessels", td.vessels);
                cmd.Parameters.AddWithValue("@fisherman", td.fisherman);
                cmd.Parameters.AddWithValue("@quantity", td.quantity);
                cmd.Parameters.AddWithValue("@UID", td.UID);
                cmd.Parameters.AddWithValue("@gearUsed", td.gearUsed);
                cmd.Parameters.AddWithValue("@landingSite", td.landingSite);


                db.con.Open();

                int rows = cmd.ExecuteNonQuery();

                if(rows > 0)
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
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.con.Close();
            }
            return isSuccess;
        }
        #endregion

        public DataTable DisplayAllTransactions()
        {

            //Create DataTable to hold the dataform database temporarily
            DataTable dt = new DataTable();
            try
            {
                //Write the SQL Query to Display all Transactions
                string sql = "SELECT TransDetID [ID], fisherman [Fisherman], added_date [Transaction Time], transno [Transaction Number] FROM TransDetails";

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
    }
}
