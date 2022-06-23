using System;
using AddressBookProblem;

namespace AddressBookProblem
{
    public class Program
    {
        public static void Main(String[] args)
        {
            AddressBook empservice = new AddressBook();
            Console.WriteLine("Welcome in the Employee Pay Roll Service");
            AddressBook payrollService = new AddressBook();
            bool check = true;


            while (check)
            {
                Console.WriteLine("1. To Insert the Data in Data Base \n");
                Console.WriteLine("Enter the Above Option");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        AddressBookModel empModel = new AddressBookModel();
                        //empModel.Id = 1;
                        empModel.FirstName = "Mohan";
                        empModel.LastName = "Sahu";
                        empModel.Address = "Bera";
                        empModel.City = "Bemetara";
                        empModel.State = "CG";

                        empModel.ZipCode = 491335;
                        empModel.PhoneNumber = "7898625487";
                        empModel.Email = "Mohan@12gmail.com";

                        payrollService.AddContact(empModel);
                        break;
                    case 2:
                        List<AddressBookModel> empList = empservice.GetAllEmployees();
                        foreach (AddressBookModel data in empList)
                        {
                            Console.WriteLine(data.Id + " " + data.FirstName + " " + data.LastName + " " + data.Address + " " + data.City + " " + data.State + " " + data.ZipCode + " " + data.PhoneNumber + " " + data.Email);
                        }
                        break;
                    case 0:
                        check = false;
                        break;
                    default:
                        Console.WriteLine("Please Enter the Correct option");
                        break;
                }
            }
        }
    }
}