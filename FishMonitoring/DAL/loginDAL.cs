using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using Project.BLL;
using System.Windows.Forms;
using System.Data;
using WpfPosApp;

namespace Project.DAL
{
    class loginDAL
    {
        // Static String to connect Database
        MyConnection db = new MyConnection();
        public bool loginCheck(loginBLL l)
        {
            bool isSuccess = false;
            MyConnection db = new MyConnection();

            try
            {
                string sql = "SELECT * FROM Login WHERE UserName=@Username AND Password=@Password";

                SqlCommand cmd = new SqlCommand(sql, db.con);

                cmd.Parameters.AddWithValue("@UserName", l.UserName);
                cmd.Parameters.AddWithValue("@Password", l.Password);
                
                cmd.Parameters.AddWithValue("@Added_Date", DateTime.Now);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    //login succesfull
                    isSuccess = true;
                }
                else
                {
                    //login failed
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

        #region Getting User ID from Username
        public loginBLL GetIDFromUsername(string username)
        {
            loginBLL u = new loginBLL();
            MyConnection db = new MyConnection();
            DataTable dt = new DataTable();

            try
            {
                string sql = "SELECT UserID, UserType FROM Login WHERE UserName='" + username + "'";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, db.con);
                db.con.Open();

                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    u.UserID = int.Parse(dt.Rows[0]["UserID"].ToString());
                    u.UserType = dt.Rows[0]["UserType"].ToString();
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
