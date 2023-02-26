using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace clinic
{
    public class doctor
    {
        public static string doctorid;
        public  void doctorMenu(string docUserid)
        {

            string cs = "Data Source=TECH-PC;Initial catalog=clinic;integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            string nameqry = $"select * from doctor where doc_user_id='{docUserid}'";
            SqlCommand namedisplay = new SqlCommand(nameqry, con);
            SqlDataReader reader = namedisplay.ExecuteReader();
            while (reader.Read())
            {
                string docname = reader.GetValue(1).ToString();
                Console.WriteLine("----Doctor Menu------");
                Console.WriteLine("   welcome " + docname);

                doctorid = reader.GetValue(0).ToString();
            }
            docmenu();
            reader.Close();



        }
        public void docmenu()
        {
            Console.WriteLine("1.View appontment");
            Console.WriteLine("2.logout");
            Console.WriteLine("Enter your choice");
            string ch = Console.ReadLine();
            switch (ch)
            {
                case "1":
                    viewAppointment();
                    break;
                case "2":
                    login obj = new login();
                    obj.loginDetails();
                    break;
                default:
                    break;
            }
        }
        public void viewAppointment()
        {
            Console.WriteLine("---Todays appointment---");
            Console.WriteLine();
            string cs = "Data Source=TECH-PC;Initial catalog=clinic;integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            string patientnameQuery = $"SELECT p.patient_id,p.register_id,p.first_name,a.tocken_number FROM patient p INNER JOIN appointment a " +
                $"ON p.patient_id = a.patient_id WHERE a.doctor_id ='{doctorid}' and a.status1='{"active"}' ORDER BY a.tocken_number";
            SqlCommand  patientnameCommand= new SqlCommand(patientnameQuery, con);
            SqlDataReader reader = patientnameCommand.ExecuteReader();
            Console.WriteLine("Regiser id\t Name\tToken no.");
            int flag = 0;
            while (reader.Read())
            {
                
                Console.WriteLine(reader.GetValue(1).ToString()+"\t\t"+ reader.GetValue(2).ToString()+"\t"+reader.GetValue(3).ToString());
                flag = 1;
               // doctorid = reader.GetValue(0).ToString();
            }
            reader.Close();
            if (flag == 1)
            {
                Console.WriteLine();
                Console.WriteLine("enter patient Register id to add prescription");
                string regid = Console.ReadLine();

                string idselectQuery = $"select patient_id from patient where register_id='{regid}'";
                SqlCommand idselectcommand = new SqlCommand(idselectQuery, con);
                SqlDataReader reader2 = idselectcommand.ExecuteReader();
                string id = "";
                while (reader2.Read())
                {
                    id = reader2.GetValue(0).ToString();
                }

                reader2.Close();
                addpresc(id);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("no Appointment today ");
                docmenu();
            }
            



            
            
        }
        public void addpresc(string id)
        {
            string cs = "Data Source=TECH-PC;Initial catalog=clinic;integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            doctor obj = new doctor();
            string appointqry = $"select* from patient where patient_id='{id}'";
            SqlCommand appointcmd = new SqlCommand(appointqry, con);
            SqlDataReader reader = appointcmd.ExecuteReader();

            while (reader.Read())
            {

                Console.Write("Name: " + reader.GetValue(2).ToString() + " " + reader.GetValue(3).ToString() + "\t");
                string dob = reader.GetValue(4).ToString();
                calculateAge(dob);
                Console.Write("Gender: " + reader.GetValue(5).ToString() + "\t");
                Console.WriteLine("Blood Group: " + reader.GetValue(7).ToString());
            }
            reader.Close();
            Console.WriteLine("");
            Console.WriteLine("Add diagnosis");
            string diagnosis = Console.ReadLine();
            Console.WriteLine("Prescribe Medicine");
            string medicine = Console.ReadLine();
            Console.WriteLine("Add notes");
            string notes = Console.ReadLine();
            DateTime appointdate = DateTime.Today;
            string appointdate1 = appointdate.ToString();

            string insertqry = $"insert into prescription1(med_prescription,diagnosis,doctor_note,patient_id,consult_date)values('{medicine}','{diagnosis}','{notes}','{id}','{appointdate}')";
            SqlCommand insertcmd = new SqlCommand(insertqry, con);
            insertcmd.ExecuteNonQuery();
            string status2 = "completed";
            string updateqry = $"update appointment set status1='{status2}' where patient_id='{id}' and doctor_id='{doctorid}' and status1='{"active"}'";
            SqlCommand updatecmd = new SqlCommand(updateqry, con);
            updatecmd.ExecuteNonQuery();
            Console.WriteLine("-----Consultation Completed-----");
            viewAppointment();




        }
        public void calculateAge(string dob)
        {
            var birthday = DateTime.Parse(dob);
            var age = DateTime.Now.Year - birthday.Year;
            Console.Write("Age: " + age + "\t");
        }
    } 
}
