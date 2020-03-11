using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using Entities;
using System.Windows.Forms;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Data.Common;

using System.Data;
using System.ComponentModel;

namespace Model
{


    public class IOManager
    {
        
        bool firedAbortEvent = new bool();
        
        public IOManager()
        {
            firedAbortEvent = false;
        }

        public void GetPassedEvent(bool firedAbortEvent)
        {
            this.firedAbortEvent = true;
        }

        public bool Insert(Employee employeeToAdd)
        {
            using(SqlCeConnection conn = new SqlCeConnection())

            using (SqlCeCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.ConnectionString = @"Data Source=C:\Users\Andrei\Documents\Visual Studio 2013\Projects\MVC Database with SQL\Database file\EmployeeDatabase.sdf";

                    conn.Open();

                    cmd.CommandText =

                    "INSERT INTO [Employees] (FirstName, LastName, Gender, Salary, HiredStatus) VALUES ('" + employeeToAdd.FirstName +
                                                                                                     "','" + employeeToAdd.LastName +
                                                                                                     "','" + employeeToAdd.Gender +
                                                                                                     "','" + employeeToAdd.Salary +
                                                                                                     "','" + employeeToAdd.HiredStatus + "')";
                    cmd.ExecuteNonQuery();

                    return true;
                }
                catch (Exception ex)
                {
                    //To do log

                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        public List<Employee> LoadDatabase()
        {
            using(SqlCeConnection conn = new SqlCeConnection())

            using (SqlCeCommand cmd = conn.CreateCommand())
            {
                try
                {
                    List<Employee> employeeList = new List<Employee>();
                    
                    conn.ConnectionString = @"Data Source=C:\Users\Andrei\Documents\Visual Studio 2013\Projects\MVC Database with SQL\Database file\EmployeeDatabase.sdf";

                    conn.Open();

                    cmd.CommandText = "SELECT * FROM [Employees]";

                    SqlCeDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read() && firedAbortEvent != true)
                    {
                        Employee employeeToBeRead = new Employee();

                        employeeToBeRead.Id = (int)reader["ID"];
                        employeeToBeRead.FirstName = (string)reader["FirstName"];
                        employeeToBeRead.LastName = (string)reader["LastName"];
                        employeeToBeRead.Gender = (string)reader["Gender"];
                        employeeToBeRead.Salary = (int)reader["Salary"];
                        employeeToBeRead.HiredStatus = (string)reader["HiredStatus"];

                        employeeList.Add(employeeToBeRead);
                    }
                    if (firedAbortEvent == false)
                        return employeeList;
                    else return null;
                }
                catch
                {
                    //to do log
                    return null;
                }
                finally
                {
                    if(conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        firedAbortEvent = false;
                    }
                }
            }
        }

        public void Update(Employee employeeToUpdate)
        {
            using (SqlCeConnection conn = new SqlCeConnection())

            using (SqlCeCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.ConnectionString = @"Data Source=C:\Users\Andrei\Documents\Visual Studio 2013\Projects\MVC Database with SQL\Database file\EmployeeDatabase.sdf";

                    conn.Open();

                    cmd.CommandText = "UPDATE [Employees] SET FirstName = '" + employeeToUpdate.FirstName
                                                    + "', LastName = '" + employeeToUpdate.LastName
                                                    + "', Gender = '" + employeeToUpdate.Gender
                                                    + "', Salary = " + employeeToUpdate.Salary
                                                    + ", HiredStatus = '" + employeeToUpdate.HiredStatus + "'";
                    cmd.ExecuteNonQuery();            
                }
                catch
                {
                    //to do log
                }
                finally
                {
                    if(conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        public void DeleteEmployee(int id)
        {
            using (SqlCeConnection conn = new SqlCeConnection())

            using (SqlCeCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.ConnectionString = @"Data Source=C:\Users\Andrei\Documents\Visual Studio 2013\Projects\MVC Database with SQL\Database file\EmployeeDatabase.sdf";

                    conn.Open();

                    cmd.CommandText = "DELETE FROM Employees WHERE ID=" + id;

                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    //to do log
                }
                finally
                {
                    if(conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }


        }

        public void DeleteDatabase()
        {
            using (SqlCeConnection conn = new SqlCeConnection())

            using (SqlCeCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.ConnectionString = @"Data Source=C:\Users\Andrei\Documents\Visual Studio 2013\Projects\MVC Database with SQL\Database file\EmployeeDatabase.sdf";

                    conn.Open();

                    cmd.CommandText = "DELETE FROM Employees";

                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    //To do log
                }
                finally
                {
                    if(conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        public void ResetID()
        {
            using (SqlCeConnection conn = new SqlCeConnection())

            using (SqlCeCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.ConnectionString = @"Data Source=C:\Users\Andrei\Documents\Visual Studio 2013\Projects\MVC Database with SQL\Database file\EmployeeDatabase.sdf";

                    conn.Open();

                    cmd.CommandText = "ALTER TABLE Employees ALTER COLUMN [ID] IDENTITY (1,1)";

                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    //to do log
                }
                finally
                {
                    if(conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}
