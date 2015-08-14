using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sarasavi
{
    public partial class UserReg : Form
    {
       
        
        public UserReg()
        {
            Thread t = new Thread(new ThreadStart(getSplashScreen));
            t.Start();
            Thread.Sleep(8000);
            InitializeComponent();
            t.Abort();
        }

        public void getSplashScreen() {
            Application.Run(new SplashScreen());
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Register_Click(object sender, EventArgs e)
        {
            String uname = "";
            String uaddress = "";
            String uNic = "";

            if( name.Text==""|| nic.Text==""||address.Text=="" ){

                System.Windows.Forms.MessageBox.Show("Enter correct values to all Fields!");
               
            }else{

                uname = name.Text;
                uaddress = address.Text;
                uNic = nic.Text;

            User user = new User();
            user.registerUser( id,uname, uaddress, uNic, radioButton1, radioButton2);
            name.Text = "";
            address.Text = "";
            nic.Text = "";
            }
        } 

        

        private void button1_Click(object sender, EventArgs e)
        {
            id.Text = "";
            name.Text = "";
            address.Text = "";
            nic.Text = "";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (bTitle.Text == "" || bPub.Text == "" || bClasi.Text == "")
            {
                MessageBox.Show("Enter Correct values to all fields!");
            }
            else
            {
               String btitle = bTitle.Text;
               String bpub = bPub.Text;
               String bcla = bClasi.Text;

                Book book = new Book();
                book.registerBook(btitle, bpub, bcla, radioButton3, radioButton4);

                bTitle.Text = "";
                bPub.Text = "";
                bClasi.Text = "";

            }
         }

       

        private void button2_Click(object sender, EventArgs e)
        {
            bTitle.Text = "";
            bClasi.Text = "";
            bPub.Text = "";
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (uidtb.Text == "")
            {
                MessageBox.Show("Enter correct user ID! ");

            }
            else{
                String id = uidtb.Text;
            BookIssuer bi = new BookIssuer();
            bi.checkOverdues(id);
            }
        }

        private void availablebtn_Click(object sender, EventArgs e)
        {
            if (bidtb.Text == "")
            {
                MessageBox.Show("Enter correct book Number!");
            }
            else
            {
                String bid = bidtb.Text;
                BookIssuer bi = new BookIssuer();
                bi.checkAvailability(bid);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (issueuid.Text == "" || issuebid.Text == "")
            {
                MessageBox.Show("Enter Correct User Number and Book ID!");
            }
            else
            {
                String uid = issueuid.Text;
                String bno = issuebid.Text;

                Detailsissue.Text = "";
                BookIssuer bi = new BookIssuer();
                bi.issueLoan(uid, bno, Detailsissue);

                uidtb.Text = "";
                bidtb.Text = "";
            }
           
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void Detailsissue_TextChanged(object sender, EventArgs e)
        {

        }

        private void reservebtn_Click(object sender, EventArgs e)
        {
            if (idtb.Text == "" || bNotb.Text == "")
            {
                MessageBox.Show("Enter Correct User Number and Book Number!");
            }
            else
            {
                String id = idtb.Text;
                String bno = bNotb.Text;

                BookReserver br = new BookReserver();
                br.reserveBook(id, bno);

            }
        }

        private void checkBorrow_Click(object sender, EventArgs e)
        {

            if (idUsertb.Text == "") { MessageBox.Show("Enter Correct User ID!"); }

            else
            {
                String id = idUsertb.Text;
                
                borrowstb.Text = "";
                BookReturner br = new BookReturner();
                br.checkBorrowedBook(id, borrowstb);
            }
        }

        private void returnOk_Click(object sender, EventArgs e)
        {
            if (uid.Text == "" || bid.Text == "") { MessageBox.Show("Enter Correct User ID and Book Number!"); }
            else
            {
                String id = uid.Text;
                String idbook = bid.Text;

                BookReturner br = new BookReturner();
                br.returnAccept(id, idbook,tbRetrn);

                borrowstb.Text = "";
            }
        }

        private void inquiry_Click(object sender, EventArgs e)
        {
            if (title.Text == "")
            {
                MessageBox.Show("Enter Full or Part of Book Name/Publisher/Author!");
            }
            else
            {
                String titlebook = title.Text;
                Inquiry i = new Inquiry();
                i.checkCatalogue(titlebook, cataloguetb);
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            issueuid.Text = "";
            issuebid.Text = "";
        }

        private void UserReg_FormClosed(object sender, FormClosedEventArgs e)
        {
            //exit application when form is closed
            Application.Exit();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }


    }


}
    
      
