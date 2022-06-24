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
                Console.WriteLine("1. To Insert the Data in Data Base \n2. Retrieve data from databse\n3.Update COntact Details in Databsen\n4. Delete Data from Database");
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
                    case 3:
                        AddressBookModel emp = new AddressBookModel();
                        emp.Id = 1;
                       
                        emp.PhoneNumber = "7847850147";
                        empservice.UpdateEmp(emp);
                        break;
                    case 4:
                        List<AddressBookModel> eList = payrollService.GetAllEmployees();
                        Console.WriteLine("Enter the Contact Id to Delete the Record  From the Table");
                        int empId = Convert.ToInt32(Console.ReadLine());
                        foreach (AddressBookModel data in eList)
                        {
                            if (data.Id == empId)
                            {
                                payrollService.DeleteEmployee(empId);
                                Console.WriteLine("Record Successfully Deleted");
                            }
                            else
                            {
                                Console.WriteLine(empId + "is Not present int he Data base");
                            }
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