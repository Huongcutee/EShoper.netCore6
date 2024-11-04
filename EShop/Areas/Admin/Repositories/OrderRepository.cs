using EShop.Areas.Admin.Models;
using EShop.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EShop.Areas.Admin.Repositories
{
    public class OrderRepository
    {
        private readonly string connectionString;
        public OrderRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public DataTable GetOrderDetailFormDb()
        {
            var query = "Select  UserName,TotalPrice, CreateDate from Orders";
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            dataTable.Load(dataReader);
                        }
                    }
                    return dataTable;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        public List<OrderModel> GetOrders()
        {
            List<OrderModel> orders = new List<OrderModel>();
            OrderModel order;
            var data = GetOrderDetailFormDb();
            foreach (DataRow row in data.Rows)
            {
                order = new OrderModel
                {
                    UserName = row["UserName"].ToString(),
                    TotalPrice = Convert.ToDecimal(row["TotalPrice"]),
                    CreateDate = Convert.ToDateTime(row["CreateDate"]),
                };
                orders.Add(order);
            }
            return orders;
        }

        private DataTable GetOrdersForGraphFromDb()
        {
            var query = "SELECT CONVERT(DATE, CreateDate) AS CreateDate , SUM(TotalPrice) TotalPrice FROM Orders GROUP BY CONVERT(DATE, CreateDate)";
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }

                    return dataTable;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public List<OrderProductForGraph> GetOrderForGraph()
        {
            List<OrderProductForGraph> salesForGraph = new List<OrderProductForGraph>();
            OrderProductForGraph orderForGraph;

            var data = GetOrdersForGraphFromDb();

            foreach (DataRow row in data.Rows)
            {
                orderForGraph = new OrderProductForGraph
                {
                    PurchaseOn = Convert.ToDateTime(row["CreateDate"]),
                    TotalPrice = Convert.ToDecimal(row["TotalPrice"])
                };

                salesForGraph.Add(orderForGraph);
            }

            return salesForGraph;
        }

       
    }

}
