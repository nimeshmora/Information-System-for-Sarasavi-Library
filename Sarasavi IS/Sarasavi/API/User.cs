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
    class User
    {
        private String usrName;
        private String usrAdd;
        private String usrgender;
        private String usrNIC;
        public void registerUser(TextBox id,String name, String address, String nic, RadioButton radioMale, RadioButton radioFemale)
        {

           
            
             usrName = name;
             usrAdd = address;
             usrgender = "";
             usrNIC = nic;


            if (radioMale.Checked == true)
            {
                usrgender = "Male";
            }
            else if (radioFemale.Checked == true)
            {

                usrgender = "Female";
            }



            using (SqlConnection c = new SqlConnection("Data Source=MESHBOY\\MSSQLSEREVER3;Initial Catalog=Sarasavi;Integrated Security=True;Pooling=False"))
            {

               
                c.Open();
                int genId = 1;



                String coman = "SELECT MAX(idGen) FROM [dbo].[User]";

                using (SqlCommand cm = new SqlCommand(coman, c))
                {
                    cm.CommandType = CommandType.Text;

                    try
                    {
                        genId = Convert.ToInt32(cm.ExecuteScalar());
                        genId++;


                    }
                    catch (Exception e) { }

                   

                    cm.Dispose();


                }





                string commandString = "INSERT INTO [dbo].[User] VALUES('" + genId.ToString() + "',@name,@address,@gender,@nic," + genId + ")";
                SqlCommand sqlCmd = new SqlCommand(commandString);





                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Connection = c;




                

               
                sqlCmd.Parameters.AddWithValue("@name", usrName);
                sqlCmd.Parameters.AddWithValue("@gender", usrgender);
                sqlCmd.Parameters.AddWithValue("@address", usrAdd);
                sqlCmd.Parameters.AddWithValue("@nic", usrNIC);
               
              

                try
                {
                    sqlCmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                System.Windows.Forms.MessageBox.Show("Successfully Registered!");
                
                id.Text = "\t\t" + genId.ToString() +  "\r\n"+"  \r\n\n                   Welcome to the Sarasavi Library!";

                sqlCmd.Dispose();
                c.Close();
            }
        }
    }

}

    




            



                
               
