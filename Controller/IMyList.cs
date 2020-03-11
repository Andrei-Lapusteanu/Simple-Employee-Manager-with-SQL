using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entities;

namespace Controller
{
    public class MyList : IList<Employee>
    {
        List<Employee> employeeList = new List<Employee>();

        int IList<Employee>.IndexOf(Employee item)
        {
            return employeeList.IndexOf(item);
        }

        void IList<Employee>.Insert(int index, Employee item)
        {
            employeeList.Insert(index, item);
        }

        void IList<Employee>.RemoveAt(int index)
        {
            employeeList.RemoveAt(index);
        }

        Employee IList<Employee>.this[int index]
        {
            get
            {
                return employeeList[index];
            }
            set
            {
                employeeList[index] = value;
            }
        }

        void ICollection<Employee>.Add(Employee item)
        {
            employeeList.Add(item);
        }

        void ICollection<Employee>.Clear()
        {
            employeeList.Clear();
        }

        bool ICollection<Employee>.Contains(Employee item)
        {
            return employeeList.Contains(item);
        }

        void ICollection<Employee>.CopyTo(Employee[] array, int arrayIndex)
        {
            employeeList.CopyTo(array, arrayIndex);
        }

        int ICollection<Employee>.Count
        {
            get { return employeeList.Count; }
        }

        bool ICollection<Employee>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<Employee>.Remove(Employee item)
        {
            return employeeList.Remove(item);
        }

        IEnumerator<Employee> IEnumerable<Employee>.GetEnumerator()
        {
            return employeeList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return employeeList.GetEnumerator();
        }
    }
}
