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
    class Book
    {
        private String bookTitle;
        private String bookPub;
        private String bookClasi;
        private String bookType;
        public void registerBook(String bTitle, String bPub, String bClasi, RadioButton radioBorrow, RadioButton radioRefer)
        {

            bookTitle = bTitle;
            bookPub = bPub;
            bookClasi = bClasi;
            bookType = "";
            

            if (radioBorrow.Checked == true)
            {
                bookType = "borrowable";
            }


            if (radioRefer.Checked == true)
            {
                bookType = "reference";
            }

            using (SqlConnection c = new SqlConnection("Data Source=MESHBOY\\MSSQLSEREVER3;Initial Catalog=Sarasavi;Integrated Security=True;Pooling=False"))
            {


                SqlCommand sqlCmd;
                SqlCommand sqlCmdbook;
              
                int id = 1;
                String bIdnew = "0001";

                c.Open();



                string commandString = "SELECT max(bid) FROM [dbo].[Book] WHERE bClasification ='" + bookClasi + "'";
                string commandStringBook = "INSERT INTO [dbo].[Book] VALUES(@idint,@idstr,@title,@pub,@type,@clasify,10)";

                using (sqlCmd = new SqlCommand(commandString, c))
                {
                    try
                    {
                        sqlCmd.CommandType = CommandType.Text;


                    


                        id = Convert.ToInt32(sqlCmd.ExecuteScalar());

                        id++;

                      


                        bIdnew = (id).ToString().PadLeft(4, '0');



                        sqlCmd.Dispose();




                    }
                    catch (Exception ex)
                    {
                    }
                }

                using (sqlCmdbook = new SqlCommand(commandStringBook, c))
                {

                    try
                    {

                       
                        sqlCmdbook.CommandType = CommandType.Text;
                        sqlCmdbook.Connection = c;



                        sqlCmdbook.Parameters.AddWithValue("@idstr", bookClasi + bIdnew);
                        sqlCmdbook.Parameters.AddWithValue("@idint", id);
                        sqlCmdbook.Parameters.AddWithValue("@title", bookTitle);
                        sqlCmdbook.Parameters.AddWithValue("@pub", bookPub);
                        sqlCmdbook.Parameters.AddWithValue("@clasify", bookClasi);
                        sqlCmdbook.Parameters.AddWithValue("@type", bookType);



                        sqlCmdbook.ExecuteNonQuery();
                        System.Windows.Forms.MessageBox.Show("Successfully Registered!");

                        sqlCmdbook.Dispose();

                    }
                    catch (Exception exc) { }

                }



                c.Dispose();
            }
        }
    }
}
