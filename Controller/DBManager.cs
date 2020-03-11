using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entities;
using Model;

namespace Controller
{
    public class DBManager
    {

        IList<Employee> database;
        IOManager ioManager = new IOManager();

        public DBManager()
        {
            database = new MyList();
        }

        public bool Insert(Employee employeeToAdd)
        {
            database.Add(employeeToAdd);

            Array arr = database.ToArray<Employee>();

            return ioManager.Insert(employeeToAdd);
        }

        public List<Employee> LoadDatabase()
        {
            List<Employee> employeeList = new List<Employee>();
            
            employeeList = ioManager.LoadDatabase();

            return employeeList;
        }

        public void DeleteEmployee(int id)
        {
            ioManager.DeleteEmployee(id);
        }

        public void DeleteDatabase()
        {
            ioManager.DeleteDatabase();
        }

        public void ResetID()
        {
            ioManager.ResetID();
        }

        public void Update(Employee employeeToUpdate)
        {
            ioManager.Update(employeeToUpdate);
        }

        public void GetAndPassEventFired(bool firedEvent)
        {
            ioManager.GetPassedEvent(firedEvent);
        }
    }
}
