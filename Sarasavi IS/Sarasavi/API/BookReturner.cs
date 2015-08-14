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
    class BookReturner
    {
        private String uid;
        private String usrid;
        private String bid;
        public void checkBorrowedBook(String idtb, TextBox borrows)
        {

            String uid = idtb;
            SqlDataReader read;
            Boolean checkBook = false;

            using (SqlConnection c = new SqlConnection("Data Source=MESHBOY\\MSSQLSEREVER3;Initial Catalog=Sarasavi;Integrated Security=True;Pooling=False"))
            {
                c.Open();


                string commandString = "SELECT bookNo,bTitle FROM [dbo].[Book] WHERE bookNo IN (SELECT bookId FROM [dbo].[Borrow] WHERE userID ='" + uid + "') ";
                try
                {
                    SqlCommand sqlCmd = new SqlCommand(commandString);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Connection = c;

                    read = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    checkBook = read.HasRows;

                    if (checkBook)
                    {
                        while (read.Read())
                        {
                            String bid = read["bookNo"].ToString().Trim();
                            String title = read["bTitle"].ToString().Trim();

                            borrows.Text += "\r\n" + bid + " :   " + title;



                        }

                        read.Close();
                    }
                    else {
                        MessageBox.Show("There are no books borrowed by this User!");
                    }
                }
                catch (Exception ex) { }
            }

        }


        public void returnAccept(String uid, String bookid,TextBox rtrnTb)
        {

            String usrid = uid;
            String bid = bookid;
            int minid = 0;
            Boolean checkRows = false;
            
            
            using (SqlConnection c = new SqlConnection("Data Source=MESHBOY\\MSSQLSEREVER3;Initial Catalog=Sarasavi;Integrated Security=True;Pooling=False"))
            {
                c.Open();

                string commandStringdelete = "DELETE FROM [dbo].[Borrow] WHERE userID='" + usrid + "' AND bookId = '" + bid + "'";
                string commandStringcopy = "DECLARE @IncrementValue int SET @IncrementValue = 1 UPDATE [dbo].[Book] SET copies = copies+@IncrementValue WHERE bookNo='" + bid + "'";
                string commandStringnotify = "SELECT min(TimeStamp) FROM [dbo].[Reservation] WHERE BookId = '" + bid + "'";
                string commandStringselect = "SELECT userID,bookId FROM [dbo].[Borrow] WHERE UserId='" + usrid + "' AND BookId = '" + bid + "'";

                try
                {
                    SqlCommand sqlCmdnewone = new SqlCommand(commandStringselect);
                    sqlCmdnewone.CommandType = CommandType.Text;
                    sqlCmdnewone.Connection = c;
                    sqlCmdnewone.ExecuteNonQuery();

                    SqlDataReader read = sqlCmdnewone.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    checkRows = read.HasRows;


                }
                catch (Exception e)
                {

                }




                if (checkRows)
                {
                    using (SqlConnection conec = new SqlConnection("Data Source=MESHBOY\\MSSQLSEREVER3;Initial Catalog=Sarasavi;Integrated Security=True;Pooling=False"))
                    {
                        conec.Open();
                        SqlCommand sqlCmd = new SqlCommand(commandStringdelete, conec);
                        sqlCmd.CommandType = CommandType.Text;


                        try
                        {
                            sqlCmd.ExecuteNonQuery();
                            sqlCmd.Dispose();



                        }
                        catch (Exception execp)
                        {

                           
                        }

                        conec.Dispose();
                    }


                    using (SqlConnection conect = new SqlConnection("Data Source=MESHBOY\\MSSQLSEREVER3;Initial Catalog=Sarasavi;Integrated Security=True;Pooling=False"))
                    {
                        conect.Open();
                        SqlCommand sqlCmdcopy = new SqlCommand(commandStringcopy);
                        sqlCmdcopy.CommandType = CommandType.Text;
                        sqlCmdcopy.Connection = conect;

                        try
                        {
                            sqlCmdcopy.ExecuteNonQuery();
                            sqlCmdcopy.Dispose();

                        }
                        catch (Exception ec) { }

                        conect.Dispose();
                    }

                    using (SqlConnection conection = new SqlConnection("Data Source=MESHBOY\\MSSQLSEREVER3;Initial Catalog=Sarasavi;Integrated Security=True;Pooling=False"))
                    {
                        try
                        {

                            conection.Open();
                            SqlCommand sqlCmdnoti = new SqlCommand(commandStringnotify);
                            sqlCmdnoti.CommandType = CommandType.Text;
                            sqlCmdnoti.Connection = conection;
                            minid = (int)sqlCmdnoti.ExecuteScalar();




                            


                        }catch(Exception e){}
                    }
                         
                    
                    
                    using (SqlConnection conectionselect = new SqlConnection("Data Source=MESHBOY\\MSSQLSEREVER3;Initial Catalog=Sarasavi;Integrated Security=True;Pooling=False"))
                    {   
                    conectionselect.Open();
                    
                             String commandgetNoti = "SELECT Uid,Uname FROM [dbo].[User] WHERE Uid IN (SELECT UserId FROM [dbo].[Reservation] WHERE TimeStamp=" + minid + ")";

                            SqlCommand sqlCmdnotify = new SqlCommand(commandgetNoti);
                            sqlCmdnotify.CommandType = CommandType.Text;
                            sqlCmdnotify.Connection = conectionselect;

                            try
                            {
                                sqlCmdnotify.ExecuteNonQuery();


                           
                            SqlDataReader read = sqlCmdnotify.ExecuteReader(System.Data.CommandBehavior.CloseConnection);


                            while (read.Read())
                            {
                                String name = read["Uname"].ToString().Trim();
                                System.Windows.Forms.MessageBox.Show("Book is reserved by: "+name+" Inform User!");
                                rtrnTb.Text += "\r\n"+"Books is reserved by: "+ name + " Keep Book Away!";


                            }


                            }
                            catch (Exception exc) { }
                            sqlCmdnotify.Dispose();


                        
                        conectionselect.Dispose();
                         }
                
                    using (SqlConnection con = new SqlConnection("Data Source=MESHBOY\\MSSQLSEREVER3;Initial Catalog=Sarasavi;Integrated Security=True;Pooling=False"))
                    {

                        con.Open();
                        String commanddelete = "DELETE FROM [dbo].[Reservation] WHERE TimeStamp=" + minid;
                        using (SqlCommand sqlCmddeletereserve = new SqlCommand(commanddelete, con))
                        {
                            sqlCmddeletereserve.CommandType = CommandType.Text;
                            sqlCmddeletereserve.Connection = con;

                            try
                            {
                                sqlCmddeletereserve.ExecuteNonQuery();
                                
                            }
                            catch (Exception e)
                            {
                               

                            }

                            sqlCmddeletereserve.Dispose();
                        }



                    }

                    System.Windows.Forms.MessageBox.Show("Book returned Successfully!");

                }
                else
                {
                    MessageBox.Show("This book is never borrowed by this User!!");
                }
            }





        }

    }

}


                    
