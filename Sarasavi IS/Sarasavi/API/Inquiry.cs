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
    class Inquiry
    {
        private String idb;
        public void checkCatalogue(String idbook, TextBox catalogueData)
        {

            String idb = idbook;
           

            using (SqlConnection c = new SqlConnection("Data Source=MESHBOY\\MSSQLSEREVER3;Initial Catalog=Sarasavi;Integrated Security=True;Pooling=False"))
            {

                c.Open();

                String pattern = "%" + idb + "%";

                string commandString = "SELECT bTitle,bType,copies FROM [dbo].[Book] WHERE bTitle LIKE'" + pattern + "'";



                try
                {
                    String clasify = "";
                    int copyCount = 0;
                    String title = "";

                    SqlCommand sqlCmd = new SqlCommand(commandString);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Connection = c;

                    sqlCmd.ExecuteNonQuery();

                    SqlDataReader read = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    Boolean hasrows = read.HasRows;

                   
                    catalogueData.Text = "";

                    if (hasrows)
                    {

                        while (read.Read())
                        {
                            clasify = read["bType"].ToString().Trim();
                            copyCount = (int)read["copies"];
                            title = read["bTitle"].ToString();

                            if (clasify == "borrowable")
                            {

                                catalogueData.Text += "\r\n" + " Book : " + title + " Book is Borrowable ." + "\r\n " + copyCount + " Copy/Copies Available! \r\n";

                            }
                            else
                            {

                                catalogueData.Text += "Book :" + title + " Book is Reference one. " + "\r\n " + copyCount + " Copy/Copies Available!";
                            }
                        }

                        read.Close();
                    }
                    else {
                        MessageBox.Show("There is no such book in Library!");
                    }
                   
                    
                }
                catch (Exception e) { }
                c.Dispose();
            }
        }
    }
}
