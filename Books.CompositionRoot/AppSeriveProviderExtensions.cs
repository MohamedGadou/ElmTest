using BookStore.Application.IService;
using BookStore.Application.Service;
using BookStore.Domain.Repos;
using BookStore.Infrastructure.Repos;
using Microsoft.Extensions.DependencyInjection;

namespace Books.CompositionRoot
{
    public static class AppSeriveProviderExtensions
    {
        public static void RegisterApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IBookAppService, BookAppService>();
            serviceCollection.AddScoped<IBookRepository, BookRepository>();

        }
    }
}
