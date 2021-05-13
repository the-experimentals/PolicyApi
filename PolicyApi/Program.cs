using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PolicyApi.Services.SQLServer;

namespace PolicyApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<PolicyStore>();

                context.Database.Migrate();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("https://[*]:5003");
                    //webBuilder.ConfigureKestrel(opts =>
                    //{
                    //    opts.Listen(IPAddress.Any, 5003, listenOptions =>
                    //    {
                    //        listenOptions.Protocols = HttpProtocols.Http2;
                    //    });
                    //});
                    webBuilder.UseStartup<Startup>();
                });
    }
}
