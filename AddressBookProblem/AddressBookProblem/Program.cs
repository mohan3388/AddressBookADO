using System;
using AddressBookProblem;

namespace AddressBookProblem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AddressBook addressBook = new AddressBook();
            addressBook.SetConnection();
        }
    }
}