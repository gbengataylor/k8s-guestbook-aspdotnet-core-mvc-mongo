using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static Metaparticle.Package.Driver;

namespace GuestbookWebApi
{
    public class Program
    {
        const int port = 5000;
        
        //[Metaparticle.Runtime.Config(Ports = new int[] {port}, Executor = "metaparticle", Replicas = 4)]
        [Metaparticle.Runtime.Config(Ports = new int[] {port})]
        [Metaparticle.Package.Config(Repository = "docker-registry-default.52.170.196.188.nip.io/metaparticle/guestbook-summit:latest", 
            Publish = false, 
            Verbose = true)]
        //public static void Main(string[] args) => Containerize(args, () =>
        public static void Main(string[] args)
       	{
            
            var host = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
				.UseKestrel(options => { options.Listen(IPAddress.Any, port); })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseWebRoot("exe/wwwroot/")
                .UseIISIntegration()
                .UseUrls("http://*:5000")
                .Build();

                host.Run();
    	}
        //});
    }
}