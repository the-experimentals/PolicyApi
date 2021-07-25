using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace PolicyApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options =>
                    {
                        options.ListenAnyIP(5003, config =>
                        {
                            config.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
                        });
                        options.ListenAnyIP(6002, config =>
                        {
                            config.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
