using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class SystemSchema
        {
            public partial class API
            {
                public class Program
                {
                    public void Main(string[] args, string APIName)
                    {
                        var hostEnvironment = GetHostEnvironment(args);
                        var password = GetPassword(hostEnvironment);
                        new Methods().InitialiseDatabaseInteraction(hostEnvironment, APIName, password);
                    }

                    public void BuildIWebHostBuilder<TStartup>(IWebHostBuilder webBuilder, string[] args, string APIGUID) where TStartup : class
                    {
                        var hostEnvironment = GetHostEnvironment(args);
                        var password = GetPassword(hostEnvironment);

                        webBuilder.UseSetting("Password", password);
                        webBuilder.UseSetting("HostEnvironment", hostEnvironment);
                        webBuilder.UseUrls(new Methods.SystemSchema.API().GetAPIStartupURLs(hostEnvironment, APIGUID));
                        webBuilder.UseStartup<TStartup>();
                    }

                    private string GetHostEnvironment(string[] args)
                    {
                        //convert args to dictionary to allow easy calling
                        var argsDictionary = args.ToDictionary(a => a.Split(":")[0], a => a.Split(":")[1]);
                        return argsDictionary["HostEnvironment"];
                    }

                    private string GetPassword(string hostEnvironment)
                    {
                        //get the appsettings.json that relates to the environment being run
                        var builder = new ConfigurationBuilder()
                            .AddJsonFile($"appsettings.{hostEnvironment}.json", optional: true, reloadOnChange: true);

                        var configuration = builder.Build();
                        return configuration["Password"];
                    }
                }
            }
        }
    }
}