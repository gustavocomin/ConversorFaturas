using Financeiro.Ioc.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Financeiro.Ioc
{
    public static class Services
    {
        public static void ConfigureServices(this ServiceCollection services)
        {
            services.AddSingleton<Program>();
            services.ConfigureApplications();
        }
    }
}