using EShop.Areas.Admin.Models;
using EShop.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EShop.Areas.Admin.Repositories
{
    public class ProductRepository
    {
        private readonly string connectionString;
        public ProductRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public DataTable GetProductDetailFormDb()
        {
            var query = "Select  Id,Name, Price, Quantity  from Products";
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

        public List<ProductModel> GetProducts()
        {
            List<ProductModel> products = new List<ProductModel>();
            ProductModel product;
            var data = GetProductDetailFormDb();
            foreach (DataRow row in data.Rows)
            {
                product = new ProductModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Price = Convert.ToInt32(row["Price"]),
                    Quantity = Convert.ToInt32(row["Quantity"]),
                };
                products.Add(product);
            }
            return products;
        }

        public List<ProductForGraph> GetProductsForGraph()
        {
            List<ProductForGraph> productsForGraph = new List<ProductForGraph>();
            ProductForGraph productForGraph;

            var data = GetProductsForGraphFromDb();

            foreach (DataRow row in data.Rows)
            {
                productForGraph = new ProductForGraph
                {
                    Category = row["Name"].ToString(),
                    Products = Convert.ToInt32(row["Products"])
                };
                productsForGraph.Add(productForGraph);
            }

            return productsForGraph;
        }
        private DataTable GetProductsForGraphFromDb()
        {
            var query = @"SELECT c.Name, COUNT(p.Id) AS Products 
                  FROM Products p
                  JOIN Categories c ON p.CategoryId = c.Id
                  GROUP BY c.Name;";

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
    }
}
     
