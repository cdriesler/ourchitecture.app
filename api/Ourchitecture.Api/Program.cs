using System;
using Topshelf;
using System.Diagnostics;
using Nancy.Hosting.Self;

namespace Ourchitecture.Api
{
    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(x =>
            {
                var init = new Startup();
           
                //x.UseLinuxIfAvailable();
                x.Service<NancySelfHost>(s =>
                {
                    s.ConstructUsing(name => new NancySelfHost());

                    s.WhenStarted(tc =>
                    {
                        tc.Start();
                        init.InitializeRhino();
                    });
                    s.WhenStopped(tc =>
                    {
                        tc.Stop();
                        init.CleanupRhino();
                    });
                });

                x.RunAsLocalSystem();
                x.SetDescription("Geometric logic server for the ourchitecture app.");
                x.SetDisplayName("Ourchitecture.Api");
                x.SetServiceName("Ourchitecture");
            });
        }
    }

    public class NancySelfHost
    {
        private NancyHost m_nancyHost;

        public void Start()
        {
            m_nancyHost = new NancyHost(new Uri("http://localhost:88"));
            m_nancyHost.Start();

        }

        public void Stop()
        {
            m_nancyHost.Stop();
            Console.WriteLine("Stopped. Good bye!");
        }
    }

    /*
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseStartup<Startup>()
                .UseUrls("http://localhost:88/")
                .Build();

            host.Run();
        }
    }
    */
}