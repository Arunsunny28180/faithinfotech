using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace clinic
{
   public class receptionist
    {
        //private SqlConnection con;

        public void receptionistMenu()
        {
            login logobj = new login();
            Console.WriteLine("---------------Receptionist Menu------------------");
            string cs = "Data Source=TECH-PC;Initial catalog=clinic;integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            Console.WriteLine("1.New patient\n2.Existing patient\n3.list of patient\n4.Todays patients\n5logout");
            Console.WriteLine("choose one:");
            int ch = Convert.ToInt32(Console.ReadLine());
            switch (ch)
            {
                case 1:
                    newPatient();
                    break;
                case 2:
                    searchPatient();
                    break;
                case 3:
                    listPatient();
                    break;
                case 4:
                    todaysPatients();
                    break;
                case 5:
                    logobj.loginDetails();
                    break;
                default:
                    Console.WriteLine("enter a valid input");
                    receptionistMenu();
                    break;
            }
            
        }
        public void newPatient()
        {
            string cs = "Data Source=TECH-PC;Initial catalog=clinic;integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            con.Open();

            
            string fname = validation1.firstname("First");
            
            string lname = validation1.lastname("Last");
            
            string dob = validation1.ValBirth();
            
            string gender = validation1.gend();
            
            string contactNo = validation1.Valph();
            
            string bloodGroup = validation1.blood();
            
            string address = validation1.address();
            string isActive = "1";
            var lastfeepaid =DateTime.Today;
            var lastvisitdate = DateTime.Today;


            string InsertQuery = "insert into patient(first_name,last_name,dob,gender,contact_number,blood_group,address_details," +
                "is_active,last_feePaid,last_visitedDate) values('" + fname + "','" + lname + "','" + dob + "','" + gender + "','" + contactNo + "','" + bloodGroup + "','" + address + "'," +
                "'" + isActive + "','"+lastfeepaid+"','"+lastvisitdate+"')";

            SqlCommand insertCommand = new SqlCommand(InsertQuery, con);
            int rowsAffected = insertCommand.ExecuteNonQuery(); // execute the INSERT query and get the number of rows affected
            if (rowsAffected == 1)
            {
                Console.WriteLine("successfully registered");
            }
            else
            {
                Console.WriteLine("registration failed");
                return; // exit the method if the registration failed
            }

            string getQuery1 = $"select patient_id from patient where first_name='{fname}' and contact_number='{contactNo}'";
            SqlCommand getCommand = new SqlCommand(getQuery1, con);

            int patientId = -1; // initialize variable with default value in case no result is returned

            using (SqlDataReader reader = getCommand.ExecuteReader())
            {
                if (reader.Read()) // check if there is at least one row in the result set
                {
                    patientId = reader.GetInt32(0); // retrieve the value of the first column (patient_id)
                }
            }

            Console.WriteLine("what would you like to do");
            Console.WriteLine("1.make appointment\n2.main menu");
            int ch = Convert.ToInt32(Console.ReadLine());
            if (ch == 1)
            {
                makeApointment(patientId);
            }
            else if (ch == 2)
            {
                receptionistMenu();
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }
        public void todaysPatients()
        {
            string cs = "Data Source=TECH-PC;Initial Catalog=clinic;Integrated Security=True ";
            SqlConnection con = new SqlConnection(cs);
            con.Open();

            DateTime today = DateTime.Today;
            string date = today.ToString();

            string selectqry = $"select * from appointment where appoint_date='{date}'";
            SqlCommand display = new SqlCommand(selectqry, con);
            SqlDataReader reader = display.ExecuteReader();

            while (reader.Read())
            {
                Console.Write("Patient Reg No: " + reader.GetValue(1).ToString());
                Console.Write("\t Token: " + reader.GetValue(3).ToString());
                string docid = reader.GetValue(4).ToString();
                doctorname(docid);
            }
            reader.Close();
            receptionistMenu();
        }
        public void doctorname(string id)
        {
            string cs = "Data Source=TECH-PC;Initial Catalog=clinic;Integrated Security=True ";
            SqlConnection con = new SqlConnection(cs);
            con.Open();

            string selectqry = $"select doctor_name,dept_id from doctor where doctor_id='{id}'";
            SqlCommand display = new SqlCommand(selectqry, con);
            SqlDataReader reader = display.ExecuteReader();
            reader.Read();
            Console.Write("\tDoctor name: "+reader.GetValue(0).ToString());
            string deptid = reader.GetValue(1).ToString();
            reader.Close();
            deptname(deptid);
        }
        public void deptname(string id)
        {
            string cs = "Data Source=TECH-PC;Initial Catalog=clinic;Integrated Security=True ";
            SqlConnection con = new SqlConnection(cs);
            con.Open();

            string selectqry = $"select dept_name from department where dept_id='{id}'";
            SqlCommand display = new SqlCommand(selectqry, con);
            SqlDataReader reader = display.ExecuteReader();
            reader.Read();
            Console.WriteLine("\tDepartment: " + reader.GetValue(0).ToString());
            reader.Close();
        }

        public void listPatient()
        {
            string displayquary = "select * from patient";
            string cs = "Data Source=TECH-PC;Initial Catalog=clinic;Integrated Security=True ";
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand displayCommand = new SqlCommand(displayquary, con);
            SqlDataReader dataReader = displayCommand.ExecuteReader();
            while (dataReader.Read())
            {
                Console.WriteLine("Patient ID:" + dataReader.GetValue(0).ToString());
                Console.WriteLine("Patient Registration ID:" + dataReader.GetValue(1).ToString());
                Console.Write("Patient Name:" + dataReader.GetValue(2).ToString());
                Console.WriteLine(" " + dataReader.GetValue(3).ToString());


                Console.WriteLine("Gender:" + dataReader.GetValue(5).ToString());
                Console.Write("Blood group :" + dataReader.GetValue(7).ToString());
                Console.WriteLine("   Date of Birth:" + dataReader.GetValue(4).ToString());
                Console.Write("Contact number:" + dataReader.GetValue(6).ToString());
                Console.WriteLine("     address:" + dataReader.GetValue(8).ToString());
            }
            dataReader.Close();
            receptionistMenu();

        }
        public void searchPatient()
        {
            Console.WriteLine("Press \n 1.Search using Registration ID  \n 2.Search using phone number");
            int ch = Convert.ToInt32(Console.ReadLine());
            if (ch == 1)
            {


                Console.WriteLine("Enter the Registration id");
                string RegId = Console.ReadLine();

                string cs = "Data Source=TECH-PC;Initial Catalog=clinic;Integrated Security=True ";
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    string displaypatient = "SELECT * FROM patient WHERE register_id = @RegId";
                    using (SqlCommand displaypatientCommand = new SqlCommand(displaypatient, con))
                    {
                        displaypatientCommand.Parameters.AddWithValue("@RegId", RegId);
                        using (SqlDataReader dataReader = displaypatientCommand.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                dataReader.Read();
                                Console.WriteLine();
                                Console.WriteLine("Patient ID:   " + dataReader.GetValue(0).ToString());
                               // Console.WriteLine("Patient Registration ID: " + dataReader.GetValue(1).ToString());
                                Console.WriteLine("Patient Name: " + dataReader.GetValue(2).ToString()+" "+dataReader.GetValue(3).ToString());
                                //Console.WriteLine(" " + dataReader.GetValue(3).ToString());
                                Console.WriteLine("Gender:       " + dataReader.GetValue(5).ToString());
                                Console.WriteLine("Blood group:  " + dataReader.GetValue(7).ToString());
                                Console.WriteLine("Date of Birth: " + dataReader.GetValue(4).ToString());
                                Console.WriteLine("Contact number: " + dataReader.GetValue(6).ToString());
                                Console.WriteLine("Address: " + dataReader.GetValue(8).ToString());

                                existingpatientMenu();
                            }
                            else
                            {
                                Console.WriteLine("Patient not found.");
                                receptionistMenu();
                            }
                        }
                    }
                }
            }





            else if (ch == 2)
            {


                Console.WriteLine("Enter the phone number:");
                string phoneNum = Console.ReadLine();

                string cs = "Data Source=TECH-PC;Initial Catalog=clinic;Integrated Security=True ";
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    string displaypatient = "SELECT * FROM patient WHERE contact_number = @phoneNum";
                    using (SqlCommand displaypatientCommand = new SqlCommand(displaypatient, con))
                    {
                        displaypatientCommand.Parameters.AddWithValue("@phoneNum", phoneNum);
                        using (SqlDataReader dataReader = displaypatientCommand.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                dataReader.Read();

                                Console.WriteLine("Patient ID:" + dataReader.GetValue(0).ToString());
                                Console.WriteLine("Patient Registration ID: " + dataReader.GetValue(1).ToString());
                                Console.WriteLine("Patient Name: " + dataReader.GetValue(2).ToString()+ " " + dataReader.GetValue(3).ToString());
                                Console.WriteLine("Gender: " + dataReader.GetValue(5).ToString());
                                Console.WriteLine("Blood group: " + dataReader.GetValue(7).ToString());
                                //Console.WriteLine("Date of Birth: " + dataReader.GetValue(4).ToString());
                                Console.WriteLine("Contact number: " + dataReader.GetValue(6).ToString());
                                Console.WriteLine("Address: " + dataReader.GetValue(8).ToString());

                                Console.WriteLine("select a option:");
                                existingpatientMenu();

                            }
                            else
                            {
                                Console.WriteLine("Patient not found.");
                                receptionistMenu();
                            }
                        }
                    }
                }
            }
        }
        public void existingpatientMenu()
        {
            Console.WriteLine("enter seleted patient id: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("1.book appointment\n2.edit patient\n3.disable/enable patient\n4.back main menu");
            int ch = Convert.ToInt32(Console.ReadLine());
            switch (ch)
            {
                case 1:
                    existingPatientAppointment(id);
                    break;
                case 2:
                    editPatient(id);
                    break;
                case 3:
                   disablePatient(id);
                    break;
                case 4:
                    receptionistMenu();
                    break;
            }
        }
        public void editPatient(int id)
        {
            string cs = "Data Source=TECH-PC;Initial catalog=clinic;integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            con.Open();

        editlabel:
            Console.WriteLine("which field you want to edit");
            Console.WriteLine("1.name");
            Console.WriteLine("2.address");
            Console.WriteLine("3.phone No");
            Console.WriteLine("enter your choice");
            string ch = Console.ReadLine();

            if (ch == "1")
            {
                Console.WriteLine("enter new name");
                string fname = Console.ReadLine();
                string insertqry = $"update patient set first_name='{fname}' where patient_id='{id}'";
                SqlCommand insertcmd = new SqlCommand(insertqry, con);
                insertcmd.ExecuteNonQuery();
                Console.WriteLine("Updated successfully");
                Console.WriteLine("do you want to edit other details: y/n");
                string choose = Console.ReadLine();
                if(choose=="y" || choose == "Y")
                {
                    goto editlabel;
                }
                else
                {
                    receptionistMenu();
                }


            }
            else if (ch == "2")
            {
                Console.WriteLine("enter new Address");
                string address = Console.ReadLine();
                string insertqry = $"update patient set address_details='{address}' where patient_id='{id}'";
                SqlCommand insertcmd = new SqlCommand(insertqry, con);
                insertcmd.ExecuteNonQuery();
                Console.WriteLine("Updated successfully");
                Console.WriteLine("do you want to edit other details: y/n");
                string choose = Console.ReadLine();
                if (choose == "y" || choose == "Y")
                {
                    goto editlabel;
                }
                else
                {
                    receptionistMenu();
                }

            }
            else if (ch == "3")
            {
                Console.WriteLine("enter new phone no");
                string phone = Console.ReadLine();
                string insertqry = $"update patient set contact_details='{phone}' where patient_id='{id}'";
                SqlCommand insertcmd = new SqlCommand(insertqry, con);
                insertcmd.ExecuteNonQuery();
                Console.WriteLine("Updated successfully");
                Console.WriteLine("do you want to edit other details: y/n");
                string choose = Console.ReadLine();
                if (choose == "y" || choose == "Y")
                {
                    goto editlabel;
                }
                else
                {
                    receptionistMenu();
                }
            }
            else
            {
                Console.WriteLine("enter correct input");

                goto editlabel;

            }
            receptionistMenu();
        }
        public void disablePatient(int id)
        {
            string cs = "Data Source=TECH-PC;Initial catalog=clinic;integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            con.Open();

            Console.WriteLine();
            Console.WriteLine("patient details:");
            string selectPatientQury = $"select * from patient where patient_id='{id}'";
            SqlCommand selectcommand = new SqlCommand(selectPatientQury, con);
            SqlDataReader dataReader = selectcommand.ExecuteReader();
            string patient="";
            string status="";
            while (dataReader.Read())
            {
                Console.WriteLine("patient register id: " + dataReader.GetValue(1).ToString());
                Console.WriteLine("patient name: " + dataReader.GetValue(2).ToString());
                Console.WriteLine("patient status: " + dataReader.GetValue(9).ToString());
                patient = dataReader.GetValue(2).ToString();
                status = dataReader.GetValue(9).ToString();
            }
            
            dataReader.Close();
            //Console.WriteLine(status);
            if (status == "False")
            {
                Console.WriteLine("patient already disabled\n Do you want to enable the patient");
                Console.WriteLine("y/n");
                string choose = Console.ReadLine();
                if (choose == "y" || choose == "Y")
                {
                    string updateQuery = $"update patient set is_active='{1}' where patient_id='{id}'";
                    SqlCommand updatecommand = new SqlCommand(updateQuery, con);
                    updatecommand.ExecuteNonQuery();
                    Console.WriteLine("patient successfully enabled!");
                    receptionistMenu();
                }
                else if (choose == "n" || choose == "N")
                {
                    Console.WriteLine("exiting.....going to main menu");
                    receptionistMenu();
                }
            }
            else
            {

            
            Console.WriteLine("do you want to disable patient "+patient);
            Console.WriteLine("y/n");
            string ch = Console.ReadLine();
            if(ch=="y" || ch == "Y")
            {
                string updateQuery = $"update patient set is_active='{0}' where patient_id='{id}'";
                SqlCommand updatecommand = new SqlCommand(updateQuery, con);
                updatecommand.ExecuteNonQuery();
                Console.WriteLine("patient successfully disabled!");
                receptionistMenu();
            }
            else if (ch=="n" || ch=="N")
            {
                Console.WriteLine("exiting.....going to main menu");
                receptionistMenu();
            }
            }

        }
        public void existingPatientAppointment(int id)
        {
            string cs = "Data Source=TECH-PC;Initial catalog=clinic;integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            con.Open();

            string displayQuery = $"Select * from patient where patient_id='{id}'";
            SqlCommand displayCommand = new SqlCommand(displayQuery, con);
            SqlDataReader dataReader = displayCommand.ExecuteReader();
            while (dataReader.Read())
            {
                Console.WriteLine("patient register id: " + dataReader.GetValue(1).ToString());
                Console.WriteLine("patient name: " + dataReader.GetValue(2).ToString());
                //Console.WriteLine(" + DateTime);
            }
            dataReader.Close();


            string displayQuery1 = "Select * from department";
            SqlCommand displayCommand1 = new SqlCommand(displayQuery1, con);
            SqlDataReader dataReader1 = displayCommand1.ExecuteReader();
            Console.WriteLine("enter department:");
            while (dataReader1.Read())
            {
                //int i = 1;
                Console.WriteLine(dataReader1.GetValue(0).ToString() + "." + dataReader1.GetValue(1).ToString());
                //i = i + 1;
                //Console.WriteLine("2." + dataReader.GetValue(2).ToString());
                //Console.WriteLine(" + DateTime);
            }
            dataReader1.Close();
            int dept = Convert.ToInt32(Console.ReadLine());
            string displaydept = $"select * from department where dept_id='{dept}'";
            SqlCommand displaydeptcommad = new SqlCommand(displaydept, con);
            SqlDataReader datareader2 = displaydeptcommad.ExecuteReader();
            datareader2.Read();
            Console.WriteLine("doctors in " + datareader2.GetValue(1).ToString() + " department");
            datareader2.Close();
            string displayDoctors = $"select * from doctor where dept_id='{dept}'";
            SqlCommand displaydocCommand = new SqlCommand(displayDoctors, con);
            SqlDataReader datareader3 = displaydocCommand.ExecuteReader();
            while (datareader3.Read())
            {
                Console.WriteLine(datareader3.GetValue(0).ToString() + "." + datareader3.GetValue(1).ToString() + " " + datareader3.GetValue(2).ToString());
            }
            datareader3.Close();
            int choose = Convert.ToInt32(Console.ReadLine());
            string displayfeequery = $"select * from doctor where doctor_id='{choose}'";
            SqlCommand displayfeecommand = new SqlCommand(displayfeequery, con);
            SqlDataReader reader4 = displayfeecommand.ExecuteReader();
            reader4.Read();
            string fee = reader4.GetValue(2).ToString();
            reader4.Close();
            Console.WriteLine();
            Console.WriteLine("Appointment confirmed");


            int patientid = id;
            DateTime apointDate = DateTime.Now;
            int doctorId = choose;
            int token1 = token(doctorId);
            DateTime lastdate = apointDate;
            Console.WriteLine("your token number is: " + token1);
            //Console.WriteLine("consultation fees= " + fee);

            //string InsertQuery = "insert into appointment(patient_id,appoint_date,tocken_number,doctor_id,last_visit)" +
            //    " values('" + patientid + "','" + apointDate + "','" + token1 + "','" + doctorId + "','" + lastdate + "')";
            //SqlCommand insertCommand = new SqlCommand(InsertQuery, con);
            //insertCommand.ExecuteReader();

            string consult = $"select * from patient where patient_id='{id}'";
            SqlCommand consultcommand = new SqlCommand(consult, con);
            SqlDataReader reader1 = consultcommand.ExecuteReader();
            int flage = 2;
            string lastfeedate = "";
            while (reader1.Read())
            {
                string lastvisit = reader1.GetValue(11).ToString();
                lastfeedate = reader1.GetValue(10).ToString();
                Console.WriteLine("last consultation date: " + lastvisit);
                Console.WriteLine("last consultation fee date:" + lastfeedate);
                flage = existConsultationFee(lastvisit, lastfeedate);
            }
            reader1.Close();
            string status1 = "active";
            if (flage == 1)
            {
                Console.WriteLine("no consultation fee");
                string InsertQuery = "insert into appointment(patient_id,appoint_date,tocken_number,doctor_id,last_visit,status1)" +
                                " values('" + patientid + "','" + apointDate + "','" + token1 + "','" + doctorId + "','" + lastdate + "','"+status1+"')";
                SqlCommand insertCommand = new SqlCommand(InsertQuery, con);
                insertCommand.ExecuteNonQuery();
                string updateQuery = $"update patient set last_visitedDate='{apointDate}',last_feePaid='{lastfeedate}'";
                SqlCommand updatecommand = new SqlCommand(updateQuery, con);
                updatecommand.ExecuteNonQuery();
            }
            else if (flage == 0)
            {
                Console.WriteLine("cunsulaton fee: "+fee);
                string InsertQuery = "insert into appointment(patient_id,appoint_date,tocken_number,doctor_id,last_visit,status1)" +
                                " values('" + patientid + "','" + apointDate + "','" + token1 + "','" + doctorId + "','" + lastdate + "','" + status1 + "')";
                SqlCommand insertCommand = new SqlCommand(InsertQuery, con);
                insertCommand.ExecuteReader();
                string updateQuery = $"update patient set last_visitedDate='{apointDate}',last_feePaid='{apointDate}'";
                SqlCommand updatecommand = new SqlCommand(updateQuery, con);
                updatecommand.ExecuteNonQuery();
            }
            else
            {
                Console.WriteLine();
            }


            receptionistMenu();

        }
        public int existConsultationFee(string lastvisit,string lastfeedate)
        {
            var lastfeedate1 = DateTime.Parse(lastfeedate);
            DateTime currentdate = DateTime.Now;
            TimeSpan difference = currentdate.Subtract(lastfeedate1);
            int flag = 0;
            if (difference.TotalDays < 7)
            {

                flag = 1;
            }
            return flag;

        }
        public void makeApointment(int id)
        {
            string cs = "Data Source=TECH-PC;Initial catalog=clinic;integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            con.Open();

            string displayQuery = $"Select * from patient where patient_id='{id}'";
            SqlCommand displayCommand = new SqlCommand(displayQuery, con);
            SqlDataReader dataReader = displayCommand.ExecuteReader();
            while (dataReader.Read())
            {
                Console.WriteLine("patient register id: " + dataReader.GetValue(1).ToString());
                Console.WriteLine("patient name: " + dataReader.GetValue(2).ToString());
                //Console.WriteLine(" + DateTime);
            }
            dataReader.Close();
            

            string displayQuery1 = "Select * from department";
            SqlCommand displayCommand1 = new SqlCommand(displayQuery1, con);
            SqlDataReader dataReader1 = displayCommand1.ExecuteReader();
            Console.WriteLine("enter department:");
            while (dataReader1.Read())
            {
                //int i = 1;
                Console.WriteLine(dataReader1.GetValue(0).ToString()+"."+dataReader1.GetValue(1).ToString());
                //i = i + 1;
                //Console.WriteLine("2." + dataReader.GetValue(2).ToString());
                //Console.WriteLine(" + DateTime);
            }
            dataReader1.Close();
            int dept = Convert.ToInt32(Console.ReadLine());
            string displaydept = $"select * from department where dept_id='{dept}'";
            SqlCommand displaydeptcommad = new SqlCommand(displaydept, con);
            SqlDataReader datareader2 = displaydeptcommad.ExecuteReader();
            datareader2.Read();
            Console.WriteLine("doctors in "+datareader2.GetValue(1).ToString()+" department");
            datareader2.Close();
            string displayDoctors = $"select * from doctor where dept_id='{dept}'";
            SqlCommand displaydocCommand = new SqlCommand(displayDoctors, con);
            SqlDataReader datareader3 = displaydocCommand.ExecuteReader();
            while (datareader3.Read())
            {
                Console.WriteLine(datareader3.GetValue(0).ToString() + "." + datareader3.GetValue(1).ToString()+" "+datareader3.GetValue(2).ToString());
            }
            datareader3.Close();
            int choose = Convert.ToInt32(Console.ReadLine());
            string displayfeequery = $"select * from doctor where doctor_id='{choose}'";
            SqlCommand displayfeecommand = new SqlCommand(displayfeequery, con);
            SqlDataReader reader4 = displayfeecommand.ExecuteReader();
            reader4.Read();
            string fee = reader4.GetValue(2).ToString();
            reader4.Close();
            Console.WriteLine();
            Console.WriteLine("Appointment confirmed");
            

            int patientid = id;
            DateTime apointDate = DateTime.Now;
            int doctorId = choose;
            int token1 = token(doctorId);
            DateTime lastdate = apointDate;
            Console.WriteLine("your token number is: " +token1);
            Console.WriteLine("consultation fees= " + fee);
            string status1 = "active";
            string InsertQuery = "insert into appointment(patient_id,appoint_date,tocken_number,doctor_id,last_visit,status1)" +
                " values('" + patientid + "','" + apointDate + "','"+ token1 + "','"+ doctorId + "','"+ lastdate + "','" + status1 + "')";
            SqlCommand insertCommand = new SqlCommand(InsertQuery, con);
            insertCommand.ExecuteReader();
            receptionistMenu();






            //Console.WriteLine("1");
            //Console.WriteLine(id);
        }
        public int token(int d_id)
        {
            string cs = "Data Source=TECH-PC;Initial catalog=clinic;integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            con.Open();

            string displayQuery = $"Select * from token where doc_id='{d_id}'";
            SqlCommand displayCommand = new SqlCommand(displayQuery, con);
            SqlDataReader dataReader = displayCommand.ExecuteReader();
            dataReader.Read();
            string a = dataReader.GetValue(1).ToString();
            int token = int.Parse(a);
            int tok_org= token+1;
            dataReader.Close();
            string updateQuery = $"update token set token_number='{tok_org}' where doc_id='{d_id}'";
            SqlCommand updatecommand = new SqlCommand(updateQuery, con);
            updatecommand.ExecuteNonQuery();
            return tok_org;
        }
    }
}
