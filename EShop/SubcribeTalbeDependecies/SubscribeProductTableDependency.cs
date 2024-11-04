using EShop.Areas.Admin.Hubs;
using EShop.Models;
using EShop.SubcribeTalbeDependecies;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;

namespace EShop.SubscribeTableDepemdemcies
{
    public class SubscribeProductTableDependency  : ISubscribeTableDependecy
    {
        private readonly DashboardHub _dashboardHub;
        private SqlTableDependency<ProductModel> _tableDependency;
        private bool _disposed;

        public SubscribeProductTableDependency(DashboardHub dashboardHub)
        {
            _dashboardHub = dashboardHub;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            try
            {
                // Tạo mapping cho bảng (nếu tên các cột khác với properties trong model)
                var mapper = new ModelToTableMapper<ProductModel>();
                mapper.AddMapping(model => model.Id, "Id");
                // Thêm các mapping khác nếu cần

                _tableDependency = new SqlTableDependency<ProductModel>(
                    connectionString,
                    tableName: "Products", // Tên bảng trong SQL
                    mapper: mapper,
                    includeOldValues: true // Set true nếu bạn cần giá trị cũ trong event
                );

                _tableDependency.OnChanged += TableDependency_OnChanged;
                _tableDependency.OnError += TableDependency_OnError;
                _tableDependency.OnStatusChanged += TableDependency_OnStatusChanged;

                _tableDependency.Start();
                Console.WriteLine("SQL Table Dependency started successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting SQL Table Dependency: {ex.Message}");
                throw;
            }
        }

        private void TableDependency_OnStatusChanged(object sender, TableDependency.SqlClient.Base.EventArgs.StatusChangedEventArgs e)
        {
            Console.WriteLine($"Status changed: {e.Status}");
        }

        private void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<ProductModel> e)
        {
            try
            {
                if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
                {
                    _dashboardHub.SendProducts();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnChanged: {ex.Message}");
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(ProductModel)} SqlTableDependency error: {e.Error.Message}");
        }
    }
}
