using BlazorWebApp.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWebApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            
            //Registering syncfusionblazor service
            builder.Services.AddSyncfusionBlazor();

            //productkey registration
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDk0Njk2QDMxMzkyZTMyMmUzMEZtbXE0bXRWUUhNYnJqcEZrOVNjQ1RFTTRJOE5UOEJ5cUpLNG1VUk");

            //configure IHttpClient and related services
            builder.Services.AddHttpClient<IEmployeeService, EmployeeService>(client =>
               {
                   client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
               });

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}
