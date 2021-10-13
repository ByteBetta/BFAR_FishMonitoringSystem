using Project.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                string sql = "INSERT INTO TransDetails (FishID, transno, weight, quantity, DealID, added_date) VALUES (@FishID, @transno, @weight, @quantity, @DealID, @added_date)";

                SqlCommand cmd = new SqlCommand(sql, db.con);

                cmd.Parameters.AddWithValue("@FishID", td.FishID);
                cmd.Parameters.AddWithValue("@transno", td.transno);
                cmd.Parameters.AddWithValue("@weight", td.FishWeight);
                cmd.Parameters.AddWithValue("@quantity", td.FishQuantity);
                cmd.Parameters.AddWithValue("@DealID", td.DealID);
                cmd.Parameters.AddWithValue("@added_date", td.added_date);

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
                Console.WriteLine("Error in TransDetails");
            }
            finally
            {
                db.con.Close();
            }
            return isSuccess;
        }
        #endregion
    }
}
