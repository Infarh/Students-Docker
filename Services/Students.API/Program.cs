using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Students.API
{
    public static class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(host => host
                   .UseStartup<Startup>()
                )
            ;
    }
}
