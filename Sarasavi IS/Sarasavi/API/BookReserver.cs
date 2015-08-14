using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sarasavi
{
    class BookReserver
    {
        private String iduser;
        private String idbook;
        public void reserveBook(String uid, String bid)
        {

            iduser = uid;
            idbook = bid;



            using (SqlConnection c = new SqlConnection("Data Source=MESHBOY\\MSSQLSEREVER3;Initial Catalog=Sarasavi;Integrated Security=True;Pooling=False"))
            {
                int usridres = 1;
                c.Open();

                string commandStringBook = "INSERT INTO [dbo].[Reservation] VALUES(@uid,'" + iduser + "','" + idbook + "')";
                string commandStringcopy = "SELECT max(TimeStamp) FROM [dbo].[Reservation]";



                using (SqlCommand sqlCmdcopy = new SqlCommand(commandStringcopy))
                {
                    try
                    {
                        sqlCmdcopy.CommandType = CommandType.Text;
                        sqlCmdcopy.Connection = c;
                        usridres = (int)sqlCmdcopy.ExecuteScalar();

                        ++usridres;



                        sqlCmdcopy.Dispose();
                    }
                    catch (Exception e) {
                        
                    }
                }
                try
                {
                    SqlCommand sqlCmd = new SqlCommand(commandStringBook);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Connection = c;
                    sqlCmd.Parameters.AddWithValue("@uid", usridres);
                    sqlCmd.ExecuteNonQuery();
                    System.Windows.Forms.MessageBox.Show("Successfully Reserved!");
                    sqlCmd.Dispose();

                }
                catch (Exception exec) {
                    MessageBox.Show("First Register Book or User! ");
                }


            }
        }

    }
}












