using EShop.Areas.Admin.Hubs;
using EShop.Models;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.EventArgs;

namespace EShop.SubcribeTalbeDependecies
{
    public class SubscribeOrderTableDependency : ISubscribeTableDependecy
    {
        private readonly IConfiguration _configuration;
        private readonly DashboardHub _dashboardHub;
        private SqlTableDependency<OrderDetailModel> _tableDependency;

        public SubscribeOrderTableDependency(IConfiguration configuration, DashboardHub dashboardHub)
        {
            _configuration = configuration;
            _dashboardHub = dashboardHub;
        }
        public void SubscribeTableDependency(string connectionString)
        {
            try
            {
                // Tạo mapping cho bảng (nếu tên các cột khác với properties trong model)
                var mapper = new ModelToTableMapper<OrderDetailModel>();
                mapper.AddMapping(model => model.Id, "Id");
                // Thêm các mapping khác nếu cần

                _tableDependency = new SqlTableDependency<OrderDetailModel>(
                    connectionString,
                    tableName: "OrderDetails", // Tên bảng trong SQL
                    mapper: mapper,
                    includeOldValues: true // Set true nếu bạn cần giá trị cũ trong event
                );

                _tableDependency.OnChanged += TableDependency_OnChanged;
                _tableDependency.OnError += TableDependency_OnError;
                _tableDependency.OnStatusChanged += TableDependency_OnStatusChanged;

                _tableDependency.Start();
                Console.WriteLine("SQL Table Order Dependency started successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting SQL Table Dependency: {ex.Message}");
                throw;
            }
        }

        private void TableDependency_OnChanged(object sender, RecordChangedEventArgs<OrderDetailModel> e)
        {
            try
            {
                if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
                {
                    _dashboardHub.SendOrders();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnChanged: {ex.Message}");
            }
        }

        private void TableDependency_OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            Console.WriteLine($"Status changed: {e.Status}");
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(OrderDetailModel)} SqlTableDependency error: {e.Error.Message}");
        }


    }
}

