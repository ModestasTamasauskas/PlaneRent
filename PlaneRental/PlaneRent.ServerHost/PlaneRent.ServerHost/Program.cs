using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Core.Common.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using PlaneRental.Business.Bootstrapper;

namespace PlaneRent.ServerHost
{
    public class Program
    {
        public static void Main(string[] args)
        {

            

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://localhost:5001")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
            string s = "";
            Console.WriteLine("wtf");
            var config = new HttpConfiguration();
            IApiExplorer apiExplorer = config.Services.GetApiExplorer();
            foreach (ApiDescription api in apiExplorer.ApiDescriptions)
            {
                Console.WriteLine("Uri path: {0}", api.RelativePath);
                Console.WriteLine("HTTP method: {0}", api.HttpMethod);
                foreach (ApiParameterDescription parameter in api.ParameterDescriptions)
                {
                    Console.WriteLine("Parameter: {0} - {1}", parameter.Name, parameter.Source);
                }
                Console.WriteLine();
            }
            host.Run();
            
        }
    }
}
