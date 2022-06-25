using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookProblem
{
    public class AddressBook
    {
        private SqlConnection con;
        private void Connection()
        {
            string connectingString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AddressBookAdo;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con = new SqlConnection(connectingString);
        }
        public string AddContact(AddressBookModel obj)
        {
            try
            {
                Connection();
                SqlCommand com = new SqlCommand("spAddNewPersons", con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@FirstName", obj.FirstName);
                com.Parameters.AddWithValue("@LastName", obj.LastName);
                com.Parameters.AddWithValue("@Address", obj.Address);
                com.Parameters.AddWithValue("@City", obj.City);
                com.Parameters.AddWithValue("@State", obj.State);

                com.Parameters.AddWithValue("@ZipCode", obj.ZipCode);
                com.Parameters.AddWithValue("@PhoneNumber", obj.PhoneNumber);
                com.Parameters.AddWithValue("@Email", obj.Email);


                con.Open();
                int i = com.ExecuteNonQuery();
                con.Close();
                if (i != 0)
                {
                    return "data Added";
                }
                else
                {
                    return "data not added";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.con.Close();
            }
        }

        public List<AddressBookModel> GetAllEmployees()
        {
            Connection();
            List<AddressBookModel> EmpList = new List<AddressBookModel>();
            SqlCommand com = new SqlCommand("spViewContacts", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {
                EmpList.Add(
                    new AddressBookModel
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        FirstName = Convert.ToString(dr["FirstName"]),
                        LastName = Convert.ToString(dr["LastName"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        ZipCode = Convert.ToInt32(dr["ZipCode"]),
                        PhoneNumber = Convert.ToString(dr["PhoneNumber"]),
                        Email = Convert.ToString(dr["Email"]),

                    }
                    );
            }
            return EmpList;
        }
        //To Update Emp data   
        public bool UpdateEmp(AddressBookModel obj)
        {
            Connection();
            SqlCommand com = new SqlCommand("SPUpdateDetails", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", obj.Id);

            com.Parameters.AddWithValue("@PhoneNumber", obj.PhoneNumber);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Delete details
        public bool DeleteEmployee(int Id)
        {
            Connection();
            SqlCommand com = new SqlCommand("spDeletePersonById", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", Id);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Retrieve Data from City or State

        public List<AddressBookModel> RetrieveDataUsingCityName(string City, string State)
        {
            Connection();
            List<AddressBookModel> EmpList = new List<AddressBookModel>();
            SqlCommand com = new SqlCommand("spViewContactsUsingCityName", con);
            con.Open();
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@City", City);
            com.Parameters.AddWithValue("@State", State);
            SqlDataReader da = com.ExecuteReader();
            DataTable dt = new DataTable();
           
           
            
            if (da.HasRows)
            {
                while(da.Read())
                {
                    AddressBookModel emp = new AddressBookModel();
                    emp.FirstName= da.GetString(1);
                    emp.LastName = da.GetString(2);
                    emp.Address = da.GetString(3);
                    emp.City = da.GetString(4);
                    emp.State = da.GetString(5);
                    emp.ZipCode = da.GetInt32(6);
                    emp.PhoneNumber = da.GetString(7);
                    emp.Email=da.GetString(8);
                    List<AddressBookModel> list = new List<AddressBookModel>();
                    list.Add(emp);
                    DisplayEmployeeDetails(list);
                }
            }
            con.Close();
            //Bind EmpModel generic list using dataRow     

            return EmpList;
        }

      
        public void DisplayEmployeeDetails(List<AddressBookModel> sqlDataReader)
        {
             foreach(AddressBookModel addressBookModel in sqlDataReader)
            {
                Console.WriteLine("FirstName: "+addressBookModel.FirstName+" LastName: "+ addressBookModel.LastName+" Address: "+addressBookModel.Address+" City: "+addressBookModel.City+" State: "+addressBookModel.State+" ZipCode "+addressBookModel.ZipCode+" Phone number "+addressBookModel.PhoneNumber+" Email "+addressBookModel.Email);
            }
            //Contact.FirstName = Convert.ToString(sqlDataReader["FirstName"]);
            //Contact.LastName = Convert.ToString(sqlDataReader["LastName"]);
            //Contact.Address = Convert.ToString(sqlDataReader["Address"] + " " + sqlDataReader["City"] + " " + sqlDataReader["State"] + " " + sqlDataReader["zip"]);
            //Contact.PhoneNumber = Convert.ToString(sqlDataReader["PhoneNumber"]);
            //Contact.Email = Convert.ToString(sqlDataReader["email"]);
            //Contact.ZipCode = Convert.ToInt32(sqlDataReader["ZipCode"]);
          
            //Console.WriteLine("{0} \n {1} \n {2} \n {3} \n {4} \n {5} \n {6}", Contact.FirstName, Contact.LastName, Contact.Address, Contact.PhoneNumber, Contact.Email, Contact.ZipCode);

        }
        public string PrintCountBasedOnCityandState()
        {
            string nameList = "";
            string query = @"Select Count(*),State,City from ContactDetails Group by State,City";
            SqlCommand sqlCommand = new SqlCommand(query, con);
            con.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    Console.WriteLine("{0} \t {1} \t {2}", sqlDataReader[0], sqlDataReader[1], sqlDataReader[2]);
                    nameList += sqlDataReader[0].ToString() + " ";
                }
            }
            return nameList;
        }
    }
}
