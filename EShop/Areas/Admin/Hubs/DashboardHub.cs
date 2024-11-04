using EShop.Areas.Admin.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace EShop.Areas.Admin.Hubs
{
    public class DashboardHub : Hub
    {
        ProductRepository productRepository;
        OrderRepository orderRepository;
        public DashboardHub(IConfiguration configuration) {

            var connectionString = configuration.GetConnectionString("ConnectedDb");
            productRepository = new ProductRepository(connectionString);
            orderRepository = new OrderRepository(connectionString);
        }
        public async Task SendProducts()
        {
            var products = productRepository.GetProducts();
            await Clients.All.SendAsync("ReceivedProducts", products);

            var productsForGraph = productRepository.GetProductsForGraph();
            await Clients.All.SendAsync("ReceivedProductsForGraph", productsForGraph);
        }
        public async Task SendOrders()
        {
            var orders = orderRepository.GetOrders();
            await Clients.All.SendAsync("ReceivedOrders", orders);

            var ordersForGraph = orderRepository.GetOrderForGraph();
            await Clients.All.SendAsync("ReceivedOrdersForGraph", ordersForGraph);
        }
    }
}
