using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Models;

namespace ConsoleApplication1
{
    class Program
    {
        static string cns = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NORTHWND;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        static void Main(string[] args)
        {
            //ProductsByCategoryName("Confections");
            //SalesByTerritory();
            //EmployeesPerRegion();
            //OrdersPerEmployee();
            //CustomersWithNamesLongerThan25Characters();

        }

        private static void CustomersWithNamesLongerThan25Characters()
        {
            using(NorthwindContext cx=new NorthwindContext())
            {
                var customer = (from cust in cx.Customers
                               where cust.CompanyName.Length >= 26
                               select cust);
                foreach(var cus in customer)
                {
                    Console.WriteLine(cus.CompanyName);
                }

            }
        }

        private static void OrdersPerEmployee()
        {
            using(NorthwindContext cx=new NorthwindContext())
            {

            }
        }

        private static void EmployeesPerRegion()
        {
            using (SqlConnection cn = new SqlConnection(cns))
            {
                cn.Open();
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "SELECT        Region.RegionDescription, COUNT(*) AS NumberOrEmployees FROM Employees INNER JOIN EmployeeTerritories ON Employees.EmployeeID = EmployeeTerritories.EmployeeID INNER JOIN Territories ON EmployeeTerritories.TerritoryID = Territories.TerritoryID INNER JOIN Region ON Territories.RegionID = Region.RegionID GROUP BY Region.RegionDescription";
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        Console.WriteLine("Rgion: {0}  Anställda: {1}", rd.GetString(0), rd.GetInt32(1));
                    }
                }
            }
        }

        private static void SalesByTerritory()
        {
            using (SqlConnection cn = new SqlConnection(cns))
            {
                cn.Open();
                using (SqlCommand cmd = cn.CreateCommand())
                {
                   cmd.CommandText= ("SELECT Territories.TerritoryDescription, SUM([Order Details].UnitPrice *[Order Details].Quantity * (1 -[Order Details].Discount)) AS TotalSales"+
                        "FROM Employees INNER JOIN"+
                        "EmployeeTerritories ON Employees.EmployeeID = EmployeeTerritories.EmployeeID INNER JOIN"+
                        "Orders ON Employees.EmployeeID = Orders.EmployeeID INNER JOIN"+
                        "Order Details] ON Orders.OrderID = [Order Details].OrderID INNER JOIN"+
                        "Territories ON EmployeeTerritories.TerritoryID = Territories.TerritoryID GROUP BY Territories.TerritoryDescription");
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        Console.WriteLine("{0} - {1}", rd.GetString(0), rd.GetDecimal(1));
                    }
                
            }
            }
        }

        private static void ProductsByCategoryName(string categoryName)
        {
            using(SqlConnection cn=new SqlConnection(cns))
            {
                cn.Open();

                using (SqlCommand cmd = cn.CreateCommand()) {
                    cmd.CommandText = "SELECT        Products.ProductName, Products.UnitPrice, Products.UnitsInStock FROM Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID GROUP BY Products.ProductName, Products.UnitPrice, Products.UnitsInStock, Categories.CategoryName HAVING(Categories.CategoryName = N'Confections')";
                    cmd.Parameters.AddWithValue("@categoryName", categoryName);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    Console.WriteLine("{0} - {1} - {2}",rd.GetString(0),rd.GetDecimal(1),rd.GetInt16(2));
                }
                }


            }
        }
    }
}
