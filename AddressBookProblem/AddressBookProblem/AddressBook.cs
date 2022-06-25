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
                while (da.Read())
                {
                    AddressBookModel emp = new AddressBookModel();
                    emp.FirstName = da.GetString(1);
                    emp.LastName = da.GetString(2);
                    emp.Address = da.GetString(3);
                    emp.City = da.GetString(4);
                    emp.State = da.GetString(5);
                    emp.ZipCode = da.GetInt32(6);
                    emp.PhoneNumber = da.GetString(7);
                    emp.Email = da.GetString(8);
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
            foreach (AddressBookModel addressBookModel in sqlDataReader)
            {
                Console.WriteLine("FirstName: " + addressBookModel.FirstName + " LastName: " + addressBookModel.LastName + " Address: " + addressBookModel.Address + " City: " + addressBookModel.City + " State: " + addressBookModel.State + " ZipCode " + addressBookModel.ZipCode + " Phone number " + addressBookModel.PhoneNumber + " Email " + addressBookModel.Email);
            }


        }


        public bool CountDataFromCityAndState(AddressBookModel address)
        {
            string connectingString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AddressBookAdo;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con = new SqlConnection(connectingString);

            List<AddressBookModel> list = new List<AddressBookModel>();
            SqlConnection connection = new SqlConnection(connectingString);
            using (connection)
            {
                SqlCommand command = new SqlCommand("CountByCityState", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue(@"City", address.City);
                command.Parameters.AddWithValue(@"State", address.State);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        address.Id = reader.GetInt32(0);
                        address.FirstName = reader.GetString(1);
                        address.LastName = reader.GetString(2);
                        address.Address = reader.GetString(3);
                        address.City = reader.GetString(4);
                        address.State = reader.GetString(5);
                        address.ZipCode = reader.GetInt32(6);
                        address.PhoneNumber = reader.GetString(7);
                        address.Email = reader.GetString(8);
                        list.Add(address);
                       
                    }
                    Console.WriteLine("Count the Address");
                    Console.WriteLine(list.Count());
                    return true;
                }
                else
                {
                    Console.WriteLine("No Data Found");
                    return false;
                }
                connection.Close();
            }
        }
    }

}
    
