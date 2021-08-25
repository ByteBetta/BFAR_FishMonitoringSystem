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
                string sql = "INSERT INTO TransDetails (ProdID, transno, price, qty, total_price, type, DealCustID, added_date) VALUES (@ProdID, @transno, @price, @qty, @total_price, @type, @DealCustID, @added_date)";

                SqlCommand cmd = new SqlCommand(sql, db.con);

                cmd.Parameters.AddWithValue("@ProdID", td.ProdID);
                cmd.Parameters.AddWithValue("@transno", td.transno);
                cmd.Parameters.AddWithValue("@price", td.price);
                cmd.Parameters.AddWithValue("@qty", td.qty);
                cmd.Parameters.AddWithValue("@total_price", td.total_price);
                cmd.Parameters.AddWithValue("@type", td.type);
                cmd.Parameters.AddWithValue("@DealCustID", td.DealCustID);
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
