using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.IO;
namespace clinic
{
    class validation1
    {
        public static string firstname(string f1)
        {
        name:
            Console.Write($"\n\tEnter Your {f1} Name: ");
            String name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("\n *** Name cannot be empty");
                goto name;
            }

            if (name.Length < 3 || name.Length > 20)
            {
                Console.WriteLine("\n *** Name must be between 3 and 20 characters.");
                goto name;
            }

            foreach (char c in name)
            {
                if (!Char.IsLetter(c) && c != ' ')
                {
                    Console.WriteLine("\n *** Name should only contain letters");
                    goto name;
                }
            }
            return name;
        }
        public static string lastname(string f1)
        {
        name:
            Console.Write($"\n\tEnter Your {f1} Name: ");
            String name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("\n *** Name cannot be empty");
                goto name;
            }
            if (name.Length < 1 || name.Length > 20)
            {
                Console.WriteLine("\n *** Name must be between 1 and 20 characters.");
                goto name;
            }


            foreach (char c in name)
            {
                if (!Char.IsLetter(c) && c != ' ')
                {
                    Console.WriteLine("\n *** Name should only contain letters");
                    goto name;
                }
            }
            return name;
        }

        public static string autoage(string date)
        {

        doo:
            //Console.Write("\n\tEnter your DOB (dd/mm/yyyy): ");
            //string dob = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(date))
            {
                Console.WriteLine("Date of birth cannot be empty or whitespace.");
                goto doo;
            }

            DateTime dateOfBirth;

            if (!DateTime.TryParse(date, out dateOfBirth))
            {
                Console.WriteLine("Invalid date of birth format. Please enter the date in the following format: MM/dd/yyyy");
                goto doo;
            }

            if (dateOfBirth >= DateTime.Now.Date)
            {
                Console.WriteLine("Date of birth cannot be today or a future date.");
                goto doo;
            }

            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.Month < dateOfBirth.Month || (DateTime.Now.Month == dateOfBirth.Month && DateTime.Now.Day < dateOfBirth.Day))
            {
                age--;
                Console.WriteLine($"\n\tAge is {age}.");
            }

            if (age < 0 || age > 150)
            {
                Console.WriteLine("Invalid age calculated.");
                goto doo;
            }
            int myInt = age;
            string mString = myInt.ToString();
            return mString;


            //return age;

        }

        public static string gend()
        {
        gend:
            Console.Write("\n\tEnter the Gender: ");
            string G = Console.ReadLine().ToLower();
            if (string.IsNullOrWhiteSpace(G))
            {
                Console.WriteLine("\n *** Gender cannot be empty or whitespace.");
                goto gend;
            }

            G = G.Trim().ToLower();

            if (G != "male" && G != "female" && G != "other")
            {
                Console.WriteLine("\n *** Gender must be either Male, Female, or Other.");
                goto gend;
            }

            return G;
        }

        public static string blood()
        {
        blog:
            Console.Write("\n\tEnter the Blood Group: ");
            string BG = Console.ReadLine().ToUpper();
            if (string.IsNullOrWhiteSpace(BG))
            {
                Console.WriteLine("\n *** Blood group cannot be empty or whitespace.");
                goto blog;
            }

            BG = BG.Trim().ToUpper();

            string[] validBloodGroups = { "A+", "B+", "AB+", "O+", "A-", "B-", "AB-", "O-" };
            if (!validBloodGroups.Contains(BG))
            {
                Console.WriteLine("Blood group must be one of the following: A+, B+, AB+, O+, A-, B-, AB-, O-");
                goto blog;
            }

            return BG;
        }


        public static string address()
        {
        ads1:
            Console.Write("\n\tEnter the Address: ");
            string ads = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(ads))
            {
                Console.WriteLine("\n *** Home address cannot be empty or whitespace.");
                goto ads1;

            }

            ads = ads.Trim();

            if (ads.Length < 10 || ads.Length > 100)
            {
                Console.WriteLine("\n *** Home address must be between 10 and 100 characters.");
                goto ads1;

            }

            return ads;
        }

        public static string ValBirth()
        {
        doo:
            Console.Write("\n\tEnter your DOB (dd/mm/yy): ");
            string dob = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(dob))
            {
                Console.WriteLine("Date of birth cannot be empty or whitespace.");
                goto doo;
            }

            DateTime dateOfBirth;

            if (!DateTime.TryParse(dob, out dateOfBirth))
            {
                Console.WriteLine("Invalid date of birth format. Please enter the date in the following format: MM/dd/yyyy");
                goto doo;
            }

            if (dateOfBirth >= DateTime.Now.Date)
            {
                Console.WriteLine("Date of birth cannot be today or a future date.");
                goto doo;
            }

            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.Month < dateOfBirth.Month || (DateTime.Now.Month == dateOfBirth.Month && DateTime.Now.Day < dateOfBirth.Day))
            {
                age--;
                //Console.WriteLine($"\n \tAge is {age}.");
            }

            if (age < 0 || age > 150)
            {
                Console.WriteLine("Invalid age calculated.");
                goto doo;
            }

            return dob;
        }

        public static string Valph()
        {
        ph1:
            Console.Write("\n\tEnter the Phone number:");
            string ph = Console.ReadLine();
            if (string.IsNullOrEmpty(ph))
            {
                Console.WriteLine("Phone number cannot be empty.");
                goto ph1;
            }

            if (!Regex.IsMatch(ph, "^[6-9]\\d{9}$"))
            {
                Console.WriteLine("Phone number must be 10 digits long and start with 6, 7, 8, or 9.");
                goto ph1;
            }

            return ph;
        }
    }
}