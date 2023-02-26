using System;
using System.Data.SqlClient;
using System.IO;

namespace clinic
{
    class Program
    {
        static void Main(string[] args)
        {
            //connection string;
            string cs = "Data Source=TECH-PC;Initial catalog=EmployeeMaster;integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            Console.WriteLine("Connection established successfully");

            //string insertdateQuery="insert into patient"
            //Console.WriteLine("enter roleName; ");
            //string roleName = Console.ReadLine();
            //string InsertQuery = "insert into role(deptName,categoryId) values('" + roleName + "')";
            //SqlCommand insertCommand = new SqlCommand(InsertQuery, con);
            //insertCommand.ExecuteReader();
            //Console.WriteLine("Data is successfully inserted into table");
            login log = new login();
            log.loginDetails();
        }
    }
}
