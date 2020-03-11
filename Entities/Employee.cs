using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Employee
    {
        int id;
        string firstName;
        string lastName;
        int salary;
        string gender;
        string hiredStatus;

        public Employee()
        {
            FirstName = "DefaultFirstName";
            LastName = "DefaultLastName";
            Gender = "Male";
            Salary = 1000;
            HiredStatus = "Hired";
            Id = id;
        }

        public Employee(ref int id, ref string firstName, ref string lastName, ref string gender, ref int salary, ref string hiredStatus)
        {
            id = Id;
            firstName = FirstName;
            lastName = LastName;
            gender = Gender;
            salary = Salary;
            hiredStatus = HiredStatus;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
       
        public int Salary
        {
            get { return salary; }
            set { salary = value; }
        }
        
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }
       
        public string HiredStatus
        {
            get { return hiredStatus; }
            set { hiredStatus = value; }
        }
    }
}
