using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MODiX.Data;
using MODiX.Services.Interfaces;
using MODiX.Services.Services;

namespace MODiX
{
    public class Startup
    {
        public void ConfigureServices()
        {
            IHost _host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddDbContextFactory<ModixDbContext>();
                services.AddSingleton<IMessageHandler, MessageHandler>();
            }).Build();

            _host.Start();
        }
    }
}
