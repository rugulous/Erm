using System;
using System.Collections.Generic;

namespace Erm.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start: " + DateTime.Now);
            List<Student> studs = Db.GetAll<Student>().Where(stud => stud.AcademicYearID == "21/22" && stud.FirstForename.Substring(3) == "e").Execute();
            Console.WriteLine("End: " + DateTime.Now);

            Console.WriteLine(studs.Count);

            foreach (var stud in studs)
            {
                Console.WriteLine($"{stud.StudentDetailID} - {stud.FirstForename} {stud.Surname} ({stud.Age})");
            }

            //while (true) ;

            List<Customer> test = Db.GetAll<Customer>().Where(cust => cust.CustomerID == "AROUT").Execute();
            Console.WriteLine(test.Count);

            //test[0].CompanyName = "TEST COMPANY";
            //Db.Update(test[0]);
        }
    }
}
