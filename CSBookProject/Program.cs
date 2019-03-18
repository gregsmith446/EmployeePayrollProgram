using System;
using System.Collections.Generic;
using System.IO;

namespace CSBookProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    // basic staff info + calculates their pay
    class Staff
    {
        // fields
        private float hourlyRate;
        private int hWorked;

        // properties 
        // all have public get, protected set
        public float TotalPay { get; protected set; }
        public float BasicPay { get; protected set; }
        public string NameOfStaff { get; protected set; }

        // simple getter + setter
        // gest the hWorked value and sets HoursWorked to hWorked if the value is more than 0
        public int HoursWorked
        {
            get
            {
                return hWorked;
            }
            set
            {
                if (value > 0)
                {
                    hWorked = value;
                }
                else
                {
                    hWorked = 0;
                }
            }
        }

        // constructor
        // the make-a-staff 3000
        public Staff(string name, float rate)
        {
            NameOfStaff = name;
            hourlyRate = rate;
        }

        // methods
        // why virtual?
        public virtual void CalculatePay()
        {
            Console.WriteLine("Calculating Pay...");

            BasicPay = hWorked * hourlyRate;
            TotalPay = BasicPay;
        }

        // print all the data to console as strings
        // why do I need 'override'?
        public override string ToString()
        {
            return "\nStaff Name = " + NameOfStaff + "\nHourly Rate = " + hourlyRate 
                + "\nHours Worked = " + HoursWorked + "\nBasic Pay = " + BasicPay + "\nTotal Pay = " + TotalPay;
        }
    }

    // inherit parent staff class + override pay calc method with manager name & hourlyRate
    class Manager : Staff
    {
        // fields
        // set mangager hourly pay rate 
        private const float managerHourlyRate = 50;

        // properties
        public int Allowance { get; private set; }

        // constructor
        // purpose is to call the base constructor, Staff - to pass the employee name and pay rate
        // {} brackets are empty as it does nothing else
        public Manager(string name) : base(name, managerHourlyRate) { }

        // methods
        // must be an 'override' as it overrides CalculatePay() in parent class 'Staff' 
        // inside we call the CalculatePay() from our base class 'Staff ' but with the manager's info, setting the values of BasicPay & TotalPay
        public override void CalculatePay()
        {
            base.CalculatePay();

            Allowance = 1000;

            if (HoursWorked >= 160)
            {
                TotalPay = BasicPay + Allowance;
            }
        }

        public override string ToString()
        {
            return "\nStaff Name = " + NameOfStaff + "\nHourly Rate = " + managerHourlyRate
                + "\nHours Worked = " + HoursWorked + "\nBasic Pay = " + BasicPay + "\nTotal Pay = " + TotalPay;
        }
    }

    // inherit staff class + override pay calc method with admin's pay info
    class Admin : Staff
    {
        // fields - they are variables with assigned levels of access?
        // create 2 constants which are of the floats type
        private const float overtimeRate = 15.5f;
        private const float adminHourlyRate = 30f;

        // properties
        public float Overtime { get; private set; }

        // constructor
        // its job is to call the base constructor & pass the parameters of an admin --> name & hourly rate
        public Admin(string name) : base(name, adminHourlyRate) { }

        // methods
        public override void CalculatePay()
        {
            base.CalculatePay();

            // if they work 160+, they receive the OT rate only for the hrs over 160
            if (HoursWorked > 160)
            {
                Overtime = overtimeRate * (HoursWorked - 160);
            }
        }

        public override string ToString()
        {
            return "\nStaff Name = " + NameOfStaff + "\nHourly Rate = " + adminHourlyRate
                + "\nHours Worked = " + HoursWorked + "\nBasic Pay = " + BasicPay + "\nOvertime" + Overtime + 
                "\nTotal Pay = " + TotalPay;
        }
    }

    // reads from a txt file + creates a list of staff objects based on contents of txt file
    class FileReader
    {
        // method called ReadFile() that takes no parameters & returns a list of staff objects
        // check if the file to be read exists - if it does read it line by line
        // after each line we read,use the split method - creating 2 parts
        // the result is stored in the results array ex/ results[0]
        // based on whether results[0] is manager or admin, we create that type of object/employee
        // close the txt file, returning the list - if theres no file CW that error
        public List<Staff> ReadFile()
        {
            List<Staff> myStaff = new List<Staff>();
            string[] result = new string[2];
            string path = "staff.txt";
            string[] separator = {", "};

            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        result = sr.ReadLine().Split(separator, StringSplitOptions.RemoveEmptyEntries);

                        if (result[1] == "Manager")
                        {
                            myStaff.Add(new Manager(result[0]));
                        } else if (result[1] == "Admin")
                        {
                            myStaff.Add(new Admin(result[1]));
                        }
                    }
                    sr.Close();
                }
            } else
            {
                Console.WriteLine("Error: File Does Not Exist");
            }

            return myStaff;
        }
    }

    // generates payslip of each employee + a report of staff who worked <10 hrs in a month
    class PaySlip
    {
        // fields
        private int month, year;

        // enum (declared within a class is private by default)
        enum MonthsOfYear { JAN = 1, FEB = 2, MAR = 3, APR = 4, MAY = 5, JUN = 6, JUL = 7, AUG = 8, SEP = 9, OCT = 10, NOV = 11, DEC = 12 }

        // constructor
        public PaySlip(int payMonth, int payYear)
        {
            month = payMonth;
            year = payYear;
        }

        // methods
        // takes in a list of staff objects
        public void GeneratePaySlip(List<Staff> myStaff)
        {
            string path;

            foreach (Staff f in myStaff)
            {
                path = f.NameOfStaff + ".txt";

                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine("PAYSLIP FOR {0} {1}", (MonthsOfYear) month, year);
                    sw.WriteLine("====================");
                    sw.WriteLine("Name of Staff: {0}", f.NameOfStaff);
                    sw.WriteLine("Hours Worked: {0}", f.HoursWorked);
                    sw.WriteLine("");
                    sw.WriteLine("Basic Pay: {0:C}", f.BasicPay);

                    if (f.GetType() == typeof(Manager))
                    {
                        sw.WriteLine("Allowance: {0:C}", ((Manager)f).Allowance);
                    }
                    else if (f.GetType() == typeof(Admin))
                    {
                        sw.WriteLine("Overtime: {0:C}", ((Admin)f).Overtime));
                    }

                    sw.WriteLine("");
                    sw.WriteLine("====================");

                    sw.WriteLine("Total Pay: {0:C}", f.TotalPay);
                    sw.WriteLine("====================");

                    sw.Close();
                }
            }
        }

        public void GenerateSummary(List<Staff> myStaff)
        {

        }

    }
}
