using Part1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace part2
{
    internal class Employee
    {
        // create a Employee class representing a line from the file.  It shoudl have public properties of the correct type
        // for each of the columns in the file

        public double iD;
        public string name;
        public string stateCode;
        public double hrsWorked;
        public double rate;
        public double taxDue;
        public double income;
        // define a list of states to validate the employees state
        List<string> states = new List<string>() {"AK", "AL", "AR", "AS", "AZ", "CA", "CO", "CT", "DC", "DE",
            "FL", "GA", "GU", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MI", "MN", "MO",
            "MP", "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "PR", "RI",
            "SC", "SD", "TN", "TX", "UM", "UT", "VA", "VI", "VT", "WA", "WI", "WV", "WY"};

        //  Create a ctor taking a single string (a csv) and use it to load the record

        public Employee(string data)
        {
            try
            {
                //split data and store in appropriate property name
                string[] words;
                words = data.Split(',');

                //check that the string is formatted correctly
                if (words.Length != 5)
                {
                    throw new Exception("Employee data is not formatted correctly. Data must contain 5 cells seperated by ',' this line has been skipped");
                }

                this.iD = double.Parse(words[0]);
                this.name = words[1];
                this.stateCode = words[2];
                this.hrsWorked = double.Parse(words[3]);
                this.rate = double.Parse(words[4]);
                this.income = hrsWorked * rate;

                if (!states.Contains(this.stateCode))
                {
                    throw new Exception($"Employee #{this.iD} Name:{this.name}  state code:'{this.stateCode}' is not valid, this line has been skipped");
                }
                
                //compute tax due
                this.taxDue = (double)TaxCalculator.ComputeTaxFor(this.stateCode, this.income, false);
               
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}\n" +
                    $"----------------------------------------------------------------");
               
                this.iD = double.NaN;
                this.name = null;
                this.stateCode = null;
                this.hrsWorked = double.NaN;
                this.rate = double.NaN;
                this.income = double.NaN;
                this.taxDue = double.NaN;


            }
        }

    }

    /// seperation for visibility
    /// ///////////////////////////////////////////////////////
    /// 
     class EmployeeRecord
    {
        // Create a static dictionary field that holds a List of Employee and is keyed by a string

        internal static Dictionary<string, List<Employee>> employees = new Dictionary<string, List<Employee>>();

        // create a static constructor that:

        public EmployeeRecord()
        {
            // enter a try/catch block for the entire static constructor to print out a message if an error occurs

            try
            {

                // declare a streamreader to read a file
                StreamReader sr;
                // initialize the static dictionary to a newly create empty one
                employees = new Dictionary<string, List<Employee>>() { };
                // open the taxtable.csv file into the streamreader


                using (sr = new StreamReader(@"C:\Users\user\source\repos\finalProject1\finalProject1\employees.csv"))
                {
                    //Create a Variable to call ReadLine, each time you call ReadLine it will skip down to the next line.
                    string myString;
                    // loop over the lines from the streamreader
                    // Read the stream as a string, and write the string to the console. 

                    while ((myString = sr.ReadLine()) != null)
                    {

                        // constuct an instance of employee record from the (csv) line in the file
                        Employee record = new Employee(myString);

                        //if an error was found in myString, all values will be placed to null, just check for name, skip this line
                        //if (record.name == null)
                        //{
                            
                        //    continue;
                        //}
                        if (!employees.ContainsKey(record.iD.ToString()) && record.name != null)

                        {
                            //Add employee to our list
                            employees.Add(record.iD.ToString(), new List<Employee>() { record });
                        }
                    }

                    
                    //provide a way to get out of the loop when you are done with the file....
                    // catch any exceptions while processing each line in another try/catch block located INSIDE the loop
                    //   this way if the line in the CSV file is incorrect, you will continue to process the next line
                    // make sure the streamreader is disposed no matter what happens
                    sr.Dispose();
                    Console.WriteLine("All Employees have been added from the CSV\n " +
                        "---------------------------------------------------------------------\n");
                    foreach (var list in employees)
                    {
                        foreach (var entry in list.Value)
                        {
                            Console.WriteLine($"Name:{entry.name} Tax Due:{entry.taxDue}");
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"\n{e}" +
                    $"--------------------------------------------------------");
            }


        }
        

        public List<Employee> SortedEmps(string sort)
        {
            var employees = EmployeeRecord.employees;
            List<Employee> sortedEmps = new List<Employee>();
            if (sort.Contains("STATE"))
            {
                foreach (var list in employees.Values)
                {
                    sortedEmps.AddRange(list);
                }
                sortedEmps = sortedEmps.OrderBy(x => x.stateCode).ToList();
                
            }
            else if (sort.Contains("INCOME"))
            {
                foreach (var list in employees.Values)
                {
                    sortedEmps.AddRange(list);
                }
                sortedEmps = sortedEmps.OrderBy(x => x.income).ToList();
                
            }
            else if (sort.Contains("ID"))
            {
                foreach (var list in employees.Values)
                {
                    sortedEmps.AddRange(list);
                }
                sortedEmps = sortedEmps.OrderBy(x => x.iD).ToList();
                
            }
            else if (sort.Contains("NAME"))
            {
                foreach (var list in employees.Values)
                {
                    sortedEmps.AddRange(list);
                }
                sortedEmps = sortedEmps.OrderBy(x => x.name).ToList();
                
            }
            else if (sort.Contains("TAX"))
            {
                foreach (var list in employees.Values)
                {
                    sortedEmps.AddRange(list);
                }
                sortedEmps = sortedEmps.OrderBy(x => x.taxDue).ToList();
                
            }
            return sortedEmps;
            
        }


    }
   
}
