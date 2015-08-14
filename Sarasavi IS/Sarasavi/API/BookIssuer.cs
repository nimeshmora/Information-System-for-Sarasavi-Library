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
    class BookIssuer
    {
        private int copyCount = 0;
        int count = 0;
        public void checkOverdues(String userId)
        {



            using (SqlConnection c = new SqlConnection("Data Source=MESHBOY\\MSSQLSEREVER3;Initial Catalog=Sarasavi;Integrated Security=True;Pooling=False"))
            {
                c.Open();
                String id = userId;
                string commandString = "SELECT COUNT(*) FROM [dbo].[Borrow] WHERE userID ='" + id + "'";
                String comandnewstring = "SELECT bookId FROM [dbo].[Borrow] WHERE userID ='" + id + "'";
                Boolean hasbook = false;

                using (SqlCommand cmnd = new SqlCommand(comandnewstring, c))
                {

                    cmnd.CommandType = CommandType.Text;
                    cmnd.Connection = c;

                    try
                    {
                        cmnd.ExecuteNonQuery();

                        SqlDataReader read = cmnd.ExecuteReader();
                        hasbook = read.HasRows;
                        read.Close();
                    }
                    catch (Exception e)
                    {

                    }


                }


                using (SqlCommand sqlCmd = new SqlCommand(commandString, c))
                {
                    try
                    {
                        count = Convert.ToInt32(sqlCmd.ExecuteScalar());


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    if (hasbook == true)
                    {



                        if (count == 5)
                        {

                            System.Windows.Forms.MessageBox.Show("Overdue of books for User " + id + "!\n Return Books!");

                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("No Overdue of Books! \n" + (5 - count) + " more books can be Borrowed");
                        }

                        sqlCmd.CommandType = CommandType.Text;


                        sqlCmd.Dispose();









                    }
                    else
                    {
                        MessageBox.Show("Not Yet borrowed any Books!");

                    }

                }
            }

        }




        public void checkAvailability(String bid)
        {

            String bNo = bid;
            SqlDataReader read;

            using (SqlConnection c = new SqlConnection("Data Source=MESHBOY\\MSSQLSEREVER3;Initial Catalog=Sarasavi;Integrated Security=True;Pooling=False"))
            {

                c.Open();

                string commandString = "SELECT bType,copies FROM [dbo].[Book] WHERE bookNo='" + bNo + "'";
                Boolean hasBook = false;


                try
                {
                    String clasify = "";

                    SqlCommand sqlCmd = new SqlCommand(commandString);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Connection = c;

                    sqlCmd.ExecuteNonQuery();

                    read = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    hasBook = read.HasRows;

                    if (hasBook)
                    {
                        while (read.Read())
                        {
                            clasify = read["bType"].ToString().Trim();
                            copyCount = (int)read["copies"];


                        }


                        read.Close();

                        if (clasify == "borrowable")
                        {

                            if (copyCount > 0)
                            {
                                if (copyCount < 2)
                                {
                                    System.Windows.Forms.MessageBox.Show("Book can be Borrowable! \r\n" + copyCount + " Copy is Available!");
                                }
                                else
                                {
                                    System.Windows.Forms.MessageBox.Show("Book can be Borrowable! \r\n" + copyCount + " Copies are Available!");
                                }
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("No Copies Available! \n Reserve your book");

                            }

                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("This book can be referenced only!");
                        }

                    }
                    else
                    {
                        MessageBox.Show("There is no book for this Book number. first Register book !");
                    }

                }
                catch (Exception ex) { }



            }

        }

        public void issueLoan(String uId, String bId, TextBox date)
        {

            String idUser = uId;
            String idBook = bId;
            String borrowedbooksid;
            SqlDataReader read;
            Boolean exception = false;




            using (SqlConnection c = new SqlConnection("Data Source=MESHBOY\\MSSQLSEREVER3;Initial Catalog=Sarasavi;Integrated Security=True;Pooling=False"))
            {

                c.Open();
                try
                {

                    string commandString = "INSERT INTO [dbo].[Borrow] VALUES(@idu,@idb)";

                    SqlCommand sqlCmd = new SqlCommand(commandString);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Connection = c;
                    sqlCmd.Parameters.AddWithValue("@idu", idUser);
                    sqlCmd.Parameters.AddWithValue("@idb", idBook);
                    sqlCmd.ExecuteNonQuery();
                    System.Windows.Forms.MessageBox.Show("Issue Success!");




                }
                catch (SqlException ex)
                {

                    MessageBox.Show("User already borrowed this book or Wrong User Number/Book Number!");

                    exception = true;
                }

                if (!exception)
                {
                    String cmdString = "SELECT copies FROM [dbo].[Book] WHERE bookNo='" + idBook + "'";
                    int updatedCopy = 0;

                    using (SqlCommand sqlCmand = new SqlCommand(cmdString))
                    {
                        int copyCountnew = 0;
                        sqlCmand.CommandType = CommandType.Text;
                        sqlCmand.Connection = c;

                        sqlCmand.ExecuteNonQuery();

                        read = sqlCmand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                        while (read.Read())
                        {
                            copyCountnew = (int)read["copies"];


                        }

                        read.Close();

                        updatedCopy = --copyCountnew;







                    }



                    String cmdStringupdate = "UPDATE [dbo].[Book] SET copies=" + updatedCopy + " WHERE bookNo='" + idBook + "'";




                    try
                    {
                        SqlCommand sqlCmandupdate = new SqlCommand(cmdStringupdate);
                        sqlCmandupdate.CommandType = CommandType.Text;

                        sqlCmandupdate.Connection = c;
                        c.Open();
                        sqlCmandupdate.ExecuteNonQuery();

                    }
                    catch (Exception excep) { }



                    DateTime now = DateTime.Now;

                    date.Text += "\r\nBorrower: " + idUser;
                    date.Text += "\r\nBook: " + idBook;
                    date.Text += "\r\nIssued Date: " + now.ToString();
                    date.Text += "\r\r\n\nReturn Date: " + now.AddDays(14).ToString();

                }
            }


        }
    }
}



              

