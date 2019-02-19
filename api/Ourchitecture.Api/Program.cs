using System;
using System.Windows.Forms;
using Topshelf;
using System.Diagnostics;
using Nancy.Hosting.Self;

using Rhino.Geometry;
using Newtonsoft.Json;

namespace Ourchitecture.Api
{
    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(x =>
            {
                x.Service<NancySelfHost>(s =>
                {
                    s.ConstructUsing(name => new NancySelfHost());

                    s.WhenStarted(tc => { tc.Start(); });
                    s.WhenStopped(tc => { tc.Stop(); });
                });

                x.RunAsLocalSystem();
                x.OnException(e => MessageBox.Show(e.Message));
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
            Startup.LaunchInProcess(Startup.LoadMode.Headless, 0);
            Console.WriteLine("Rhino loaded at port 88.");
            m_nancyHost = new NancyHost(new Uri("http://localhost:88"));
            m_nancyHost.Start();

        }

        public void Stop()
        {
            MessageBox.Show("Stopped!");
            m_nancyHost.Stop();
            Console.WriteLine("Stopped. Good bye!");
        }
    }
}