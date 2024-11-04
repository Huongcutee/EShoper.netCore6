using EShop.SubcribeTalbeDependecies;
using EShop.SubscribeTableDepemdemcies;

namespace EShop.MiddlewawreExtensions
{
    public static class ApplicationBuilderExtension
    {
        public static void UseSqlTableDependency<T>(this IApplicationBuilder applicationBuilder, string connectionString)
            where T : ISubscribeTableDependecy
        {
            var serviceProvider = applicationBuilder.ApplicationServices;
            var service = serviceProvider.GetService<T>();
            service.SubscribeTableDependency(connectionString);
        }
    }
}
