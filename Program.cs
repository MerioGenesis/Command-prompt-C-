using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
using static System.Reflection.Metadata.BlobBuilder;


namespace Array_and_Data_Types_Assignemnt
{
    class Program
    {

        //Customer structure with data types

        struct Customer
        {
            //Customer properties that make up the "Customer" structure


            public int ID;
            public string FirstName;
            public string Surname;
            public String Adress;
            public String TelNum;
            public DateTime DoB;
            public int PlanNumber;
            public Tariff[] month;

            //Nested structure that makes up a "bill"
            public struct Tariff
            {
                public double costByMonth;
                public int minUsed;
                public int textUsed;
                public int internetUsed;
                public int monthNum;

            }



        }


        //Main Method

        const string filename = "CustomerRecord.txt";
        static void Main(string[] args)
        {

            Customer[] CustomerList = new Customer[1];
            loaddata(ref CustomerList);

            //memory allocation

            for (int i = 0; i < CustomerList.Length; i++)
                CustomerList[i].month = new Customer.Tariff[12];

            int choice;


            //Menu

            do
            {
                Console.Clear();
                Console.WriteLine("1. Load Data");
                Console.WriteLine("2. Add a Customer");
                Console.WriteLine("3. Edit a Customer");
                Console.WriteLine("4. Delete a Customer");
                Console.WriteLine("5. Display Customers");
                Console.WriteLine("6. Add a Bill");
                Console.WriteLine("7. Display Bill");
                Console.WriteLine("8. Put in Order");
                Console.WriteLine("9. Exit");

                Console.Write("Choose between 1-9 ");
                choice = Convert.ToInt32(Console.ReadLine());


                switch (choice)
                {
                    case 1:

                        GreatestID(ref CustomerList);
                        Console.WriteLine(GreatestID(ref CustomerList));

                        Console.ReadLine();
                        break;



                    case 2:

                        addCustomer(ref CustomerList);
                        Console.ReadLine();
                        break;

                    case 3:
                        EditCustomer(CustomerList, ChooseCustomer(CustomerList));
                        break;

                    case 4:
                        break;

                    case 5:

                        displayCustomers(CustomerList);
                        Console.ReadLine();
                        break;

                    case 6:
                        BillCalc1(CustomerList);
                        Console.ReadLine();
                        break;

                    case 7:
                        DisplayBill(ref CustomerList);
                        break;

                    case 8:

                        OrderCustomersByID(CustomerList);
                        break;                  

                    case 9:
                        Environment.Exit(0);
                        break;

                    default:

                        break;
                }
                Console.Clear();

            }

            while (choice != 9);
        }


        static void loaddata(ref Customer[] CustomerList)
        {

            if (!File.Exists(filename))
            {

                StreamWriter Writer = new StreamWriter(filename);
                Writer.WriteLine(0);
                Writer.Close();

            }


            StreamReader reader = new StreamReader(filename);
            int size = Convert.ToInt32(reader.ReadLine());
            Console.WriteLine("# of Customer records is: " + size); //Count how many entries are there in the text file
            CustomerList = new Customer[size];


            for (int Index = 0; Index < CustomerList.Length; Index++)
            {
                CustomerList[Index] = new Customer();
                CustomerList[Index].ID = Convert.ToInt32(reader.ReadLine());
                CustomerList[Index].FirstName = reader.ReadLine();
                CustomerList[Index].Surname = reader.ReadLine();
                CustomerList[Index].Adress = reader.ReadLine();
                CustomerList[Index].TelNum = reader.ReadLine();
                CustomerList[Index].DoB = Convert.ToDateTime(reader.ReadLine());
                CustomerList[Index].PlanNumber = Convert.ToInt32(reader.ReadLine());


            }
            reader.Close();

            //allocating memory

            for (int i = 0; i < CustomerList.Length; i++)
                CustomerList[i].month = new Customer.Tariff[12];

        }

        // Add Customer
        static void addCustomer(ref Customer[] CustomerList)
        {
            Console.Clear();
            StreamWriter Writer = new StreamWriter(filename);
            Writer.WriteLine(CustomerList.Length + 1);

            //Temporary Customer structure
            Customer temp = new Customer();

            //Assigns ID based on the number of entries in the "CustomerList"
            Console.WriteLine("Registering ID... ");
            temp.ID = CustomerList.Length + 1;

            //Create temporary customer structure

            Console.Write("Enter Customer First Name: ");
            temp.FirstName = Console.ReadLine();

            Console.Write("Enter Customer Surname: ");
            temp.Surname = Console.ReadLine();

            Console.Write("Enter Customer Adress: ");
            temp.Adress = Console.ReadLine();

            Console.Write("Enter Customer Telephone Number: ");
            temp.TelNum = Console.ReadLine();
            
            DateTime dateOfBirth;
            while (true)
            {
                Console.Write("Enter Customer Date of Birth: ");
                string dobInput = Console.ReadLine();

                if (DateTime.TryParse(dobInput, out dateOfBirth))
                {
                    // Correct dob has been entered, loop will exit
                    temp.DoB = Convert.ToDateTime(dobInput);
                    break;
                }
                else
                {
                    // dob is not valid
                    Console.WriteLine("Invalid date of birth. Please try again.");
                }
                
            }
            
            
            Console.Write("Choose plan from: 3G (£10), 4G (£20), 5G(£30)");
            int plan;

            for (int i = 0; i < 3; i++)
            {
                Console.Write("Enter a number between 1 to 3: ");
                string planType = Console.ReadLine();

                if (int.TryParse(planType, out plan))
                {
                    if (plan >= 1 && plan <= 3)

                    {
                        temp.PlanNumber = Convert.ToInt32(planType);
                        break;
                    }
                    
                    
                }

                Console.WriteLine("Select correct plan. Please try again.");
            }

            



            // Adds entries to "CustomerRecords" text file

            Writer.WriteLine(temp.ID);
            Writer.WriteLine(temp.FirstName);
            Writer.WriteLine(temp.Surname);
            Writer.WriteLine(temp.Adress);
            Writer.WriteLine(temp.TelNum);
            Writer.WriteLine(temp.DoB);
            Writer.WriteLine(temp.PlanNumber);


            // Puts old customer record back to to "CustomerRecords" text file

            for (int Index = 0; Index < CustomerList.Length; Index++)

            {

                Writer.WriteLine(CustomerList[Index].ID);
                Writer.WriteLine(CustomerList[Index].FirstName);
                Writer.WriteLine(CustomerList[Index].Surname);
                Writer.WriteLine(CustomerList[Index].Adress);
                Writer.WriteLine(CustomerList[Index].TelNum);
                Writer.WriteLine(CustomerList[Index].DoB);
                Writer.WriteLine(CustomerList[Index].PlanNumber);
            }
            Writer.Close();


            //An array update

            loaddata(ref CustomerList);

        }

        //Display Customers
        static void displayCustomers(Customer[] CustomerList)
        {

            for (int Index = 0; Index < CustomerList.Length; Index++)

            {
                displayInfo(CustomerList[Index]);

            }

        }


        static void displayInfo(Customer s)
        {
            int i;
            i = s.PlanNumber;
            if (s.PlanNumber == 1)
            {
                i = 3;
                                                            // Sets i value to display plan type text message, based on plan type in the customer structure
            }
            else if (s.PlanNumber == 2)
            {
                i = 4;
            }
            else if (s.PlanNumber == 3)

            {
                i = 5;
            }
            
            Console.WriteLine("===================");
            Console.WriteLine("Customer ID: " + s.ID);
            Console.WriteLine("Customer First Name: " + s.FirstName);
            Console.WriteLine("Customer Surname: " + s.Surname);
            Console.WriteLine("Customer Adress: " + s.Adress);
            Console.WriteLine("Customer #Telephone: " + s.TelNum);
            Console.WriteLine("Customer Date of Birth: " + s.DoB);
            Console.WriteLine("Customer Plan Type: " + i + "G");
            Console.WriteLine("===================");

        }

        static int GreatestID(ref Customer[] CustomerList)
        {

            if (CustomerList.Length == 0)
            {

                return 0;

            }

            return CustomerList[CustomerList.Length - 1].ID;
        }

        //Menu to access Bill Calculation. First user chooses month, then selects customer from a list.
        static void BillCalc1(Customer[] CustomerList)
        {

            int monthSelect;

           // do
            //{
                Console.Clear();
                Console.WriteLine("Press 1-12 to select a month. Press 13 to Quit \n 1 - January, 2 - February, 3 - March, 4 - April, 5 - May, 6 - June,\n 7- July, 8 - August, 9 - September, 10 - October, 11 - November, 12 - December \n\n 13 - Quit  ");

                monthSelect = Convert.ToInt32(Console.ReadLine());

                switch (monthSelect)
                {

                    case 1:
                        PlanNum(ref CustomerList, ChooseCustomer(CustomerList), 0);
                        break;

                    case 2:
                        PlanNum(ref CustomerList, ChooseCustomer(CustomerList), 1);
                        break;

                    case 3:
                        PlanNum(ref CustomerList, ChooseCustomer(CustomerList), 2);
                        break;

                    case 4:
                        PlanNum(ref CustomerList, ChooseCustomer(CustomerList), 3);
                        break;
                    case 5:
                        PlanNum(ref CustomerList, ChooseCustomer(CustomerList), 4);
                        break;
                    case 6:
                        PlanNum(ref CustomerList, ChooseCustomer(CustomerList), 5);
                        break;
                    case 7:
                        PlanNum(ref CustomerList, ChooseCustomer(CustomerList), 6);
                        break;
                    case 8:
                        PlanNum(ref CustomerList, ChooseCustomer(CustomerList), 7);
                        break;
                    case 9:
                        PlanNum(ref CustomerList, ChooseCustomer(CustomerList), 8);
                        break;
                    case 10:
                        PlanNum(ref CustomerList, ChooseCustomer(CustomerList), 9);
                        break;
                    case 11:
                        PlanNum(ref CustomerList, ChooseCustomer(CustomerList), 10);
                        break;
                    case 12:
                        PlanNum(ref CustomerList, ChooseCustomer(CustomerList), 11);
                        break;

                    default:

                        break;

                }

            }

      
        //Sorts customer Array
        static void OrderCustomersByID(Customer[] CustomerList)
        {

            bool swapped;

            do
            {
                swapped = false;
                for (int n = 0; n < CustomerList.Length - 1; n++)
                {
                    if (CustomerList[n].ID > CustomerList[n + 1].ID)
                    {

                        int cust = CustomerList[n].ID;
                        CustomerList[n].ID = CustomerList[n + 1].ID;
                        CustomerList[n + 1].ID = cust;
                        swapped = true;

                    }
                }


            } while (swapped);

            for (int Index = 0; Index < (CustomerList.Length); Index++)

            {

                Console.WriteLine(CustomerList[Index].ID);

            }
            Console.ReadLine();
        }

        //Displays bill based on month and customer selected.
        static void DisplayBill(ref Customer[] CustomerList)
        {
            int monthSelect;
            Console.Clear();
            Console.WriteLine("Press 1-12 to select a month. Press 13 to Quit \n 1 - January, 2 - February, 3 - March, 4 - April, 5 - May, 6 - June," +
                "\n 7- July, 8 - August, 9 - September, 10 - October, 11 - November, 12 - December \n\n 13 - Quit  ");

            monthSelect = Convert.ToInt32(Console.ReadLine());

            switch (monthSelect)
            {

                case 1:
                    MonthBillDisplay(CustomerList, ChooseCustomer(CustomerList), 0);
                    break;

                case 2:
                    MonthBillDisplay(CustomerList, ChooseCustomer(CustomerList), 1);
                    break;

                case 3:
                    MonthBillDisplay(CustomerList, ChooseCustomer(CustomerList), 2);
                    break;

                case 4:
                    MonthBillDisplay(CustomerList, ChooseCustomer(CustomerList), 3);
                    break;
                case 5:
                    MonthBillDisplay(CustomerList, ChooseCustomer(CustomerList), 4);
                    break;
                case 6:
                    MonthBillDisplay(CustomerList, ChooseCustomer(CustomerList), 5);
                    break;
                case 7:
                    MonthBillDisplay(CustomerList, ChooseCustomer(CustomerList), 6);
                    break;
                case 8:
                    MonthBillDisplay(CustomerList, ChooseCustomer(CustomerList), 7);
                    break;
                case 9:
                    MonthBillDisplay(CustomerList, ChooseCustomer(CustomerList), 8);
                    break;
                case 10:
                    MonthBillDisplay(CustomerList, ChooseCustomer(CustomerList), 9);
                    break;
                case 11:
                    MonthBillDisplay(CustomerList, ChooseCustomer(CustomerList), 10);
                    break;
                case 12:
                    MonthBillDisplay(CustomerList, ChooseCustomer(CustomerList), 11);
                    break;

                default:

                    break;

            }


        }


        //Main method that displays monthly bill
        static void MonthBillDisplay(Customer[] I, int index, int month)
        {
            
            
            Console.WriteLine("Your cost for used minutes is " + I[index].month[month].minUsed + "£");
            Console.WriteLine("Your cost for used texts is " + I[index].month[month].textUsed + "£");
            Console.WriteLine("Your cost for used data is " + I[index].month[month].internetUsed + "£");
            Console.WriteLine("Final bill for this month is " + I[index].month[month].costByMonth + "£");
            Console.ReadLine();


        }
        
        // Plan Calculation
        static void PlanNum(ref Customer[] I, int index, int month)
        {
            int tariffNum;
            int usedMin;
            int usedText;
            int usedData;
            double minCost;
            double textCost;
            double dataCost;
            double bill;
            int g;
            

            //Takes the tariff number and sets tariffNum variable for later calculations. Also sets "g" to appropriate text, so the display message
            //is correct
            
            if (I[index].PlanNumber == 1)
            {
                tariffNum = 10;
                g = 3;


            }else if (I[index].PlanNumber == 2)
            {
                tariffNum = 20;
                g = 4;
            }
            else
            {
                tariffNum = 30;
                g = 5;
            }

                Console.WriteLine("\nCalculating bill for " + g + "G plan...");
                
                Console.Write("\nType # of minutes used: ");
                usedMin = Convert.ToInt32(Console.ReadLine());

                Console.Write("Type # of Texts used: ");
                usedText = Convert.ToInt32(Console.ReadLine());

                Console.Write("Type # of Data used: ");
                usedData = Convert.ToInt32(Console.ReadLine());


                if (500 < usedMin)
                {
                    minCost = (usedMin - 500) * 0.2;
                    Console.WriteLine("Your extra minute cost is: " + minCost + " £");
                    
                }

                else
                {
                    Console.WriteLine("You do not have any extra charges on your minutes");
                    minCost = 0;

                }

                if (500 < usedText)
                {
                    textCost = (usedText - 500) * 0.1;
                    Console.WriteLine("Your extra texts cost is: " + textCost + " £");

                }

                else
                {
                    Console.WriteLine("You do not have any extra charges on your texts");
                    textCost = 0;

                }

                if (500 < usedData)
                {

                    dataCost = (usedData - 500) * 0.5;
                    Console.WriteLine("Your extra data cost is: " + dataCost + " £");
                }

                else
                {
                    Console.WriteLine("You do not have any extra charges on your data");
                    dataCost = 0;

                }

                
                // Puts cost of each variable back into customer array
                I[index].month[month].minUsed = (int)minCost;
                I[index].month[month].textUsed = (int)textCost;
                I[index].month[month].internetUsed = (int)dataCost;
                

                // Final bill calculation with 20% VAT
                bill = ((minCost + textCost + dataCost) + tariffNum) * 1.2;

                //Stores bill amount into array
                I[index].month[month].costByMonth = bill;


                Console.WriteLine("\nYour final bill is: " + Math.Round(bill, 2) + " £");
                Console.ReadLine();

            
            

        }

        //Customer Selection
        static int ChooseCustomer(Customer[] CustomerList)
        {
            int CustomerID;
            Console.Clear();

            for (int i = 0; i < CustomerList.Length; i++)
            {
                Console.WriteLine("#ID: {0} Customer Name: {1} Customer Surname: {2}", CustomerList[i].ID, CustomerList[i].FirstName, CustomerList[i].Surname);

            }

            Console.Write("\n Please enter Customer ID: ");
            bool idCorrect = int.TryParse(Console.ReadLine(), out CustomerID);
            if (idCorrect == false)
            {
                Console.WriteLine("Enter a valid ID");
                Console.ReadKey();

            }

            for (int i = 0; i < CustomerList.Length; i++)
            {
                if (CustomerList[i].ID == CustomerID)

                {
                    Console.WriteLine("Customer Selected: {0} {1}", CustomerList[i].FirstName, CustomerList[i].Surname);
                    return i;
                }
            }


            return -1;

        }


        //Edits an existing customer record
        static void EditCustomer(Customer[] I, int index)
        {
            

            Console.Write("Edit Customer First Name: ");
            I[index].FirstName = Console.ReadLine();

            Console.Write("Edit Customer Surname: ");
            I[index].Surname = Console.ReadLine();

            Console.Write("Edit Customer Adress: ");
            I[index].Adress = Console.ReadLine();

            Console.Write("Edit Customer Telephone Number: ");
            I[index].TelNum = Console.ReadLine();

            DateTime dateOfBirth;
            while (true)
            {
                Console.Write("Enter Customer Date of Birth: ");
                string dobInput = Console.ReadLine();

                if (DateTime.TryParse(dobInput, out dateOfBirth))
                {
                    // Correct dob has been entered, loop will exit
                    I[index].DoB = Convert.ToDateTime(dobInput);
                    break;
                }
                else
                {
                    // dob is not valid
                    Console.WriteLine("Invalid date of birth. Please try again.");
                }

            }


        }

       

    }


}
