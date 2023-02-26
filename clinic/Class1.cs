using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace clinic
{
    public class login
    {
        int flage = 0;
        int j = 0;
        string roledb;
        public void loginDetails()
        {
            string cs = "Data Source=TECH-PC;Initial catalog=clinic;integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            doctor doc = new doctor();
            receptionist rec = new receptionist();
            int i = 0;

            //Console.WriteLine("1.Doctor\n2.Receptionist\nEnter your role:");
            //string role = Console.ReadLine();
            Console.WriteLine("enter user name:  ");
            string un = Console.ReadLine();
            Console.WriteLine("enter password");
            string pw = Console.ReadLine();
            string displayQuery = $"Select * from login where user_name='{un}' and password='{pw}'";
            SqlCommand displayCommand = new SqlCommand(displayQuery, con);
            SqlDataReader dataReader = displayCommand.ExecuteReader();
            string docUserid="";
            while (dataReader.Read())
            {
                string undb = dataReader.GetValue(1).ToString();
                string pwdb = dataReader.GetValue(2).ToString();
                docUserid = dataReader.GetValue(0).ToString();
                roledb = dataReader.GetValue(3).ToString();
                if (un == undb && pw == pwdb &&roledb=="1"/*&& role==roledb*/)
                {
                    flage = 1;
                }
                else if(un == undb && pw == pwdb && roledb == "2")
                {

                    flage = 2;
                }
                    
                
            }
            //dataReader.Read();
            if (flage == 1 /*&& roledb=="1" *//*role=="1"*/)
            {

                Console.WriteLine("doctor login");
                doc.doctorMenu(docUserid);


            }
            else if(flage==2/*flage==1 && roledb=="2"*//*role == "2"*/)
            {
                Console.WriteLine("receptionist login");
                rec.receptionistMenu();
            }

            else
            {
                Console.WriteLine("login failed");
                string logmessage = $"incorrect login attempt at {DateTime.Now}. Username:{un},password:{pw}";
                File.AppendAllText(@"C:\Users\tech\Desktop\loginInfo.txt", logmessage + Environment.NewLine);
                j = j + 1;
                if (j == 3)
                {
                    Console.WriteLine("----you entered too many times///////please try again later ");
                    Environment.Exit(0);
                }
                loginDetails();
            }

        }



    }
}
