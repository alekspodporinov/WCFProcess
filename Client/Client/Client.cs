using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contract;

namespace Client
{
    class Client
    {
        static void SendMessage(object obj)
        {
            var proc = Process.GetProcesses();
            var pc = new ClientComputer
            {
                Name = Environment.MachineName + obj,
                Process = proc.Where(process => process.ProcessName != "Idle")
                    .Select(process => process.ProcessName).Distinct().ToArray()
            };

            var binding = new BasicHttpBinding
            {
                CloseTimeout = TimeSpan.FromHours(1),
                OpenTimeout = TimeSpan.FromHours(1),
                ReceiveTimeout = TimeSpan.FromHours(1),
                SendTimeout = TimeSpan.FromHours(1)
            };

            var proxy = ChannelFactory<IContract>.CreateChannel(
                    binding, new EndpointAddress("http://localhost:7000"));

            try
            {
                proxy.SendProcess(pc);
            }
            catch (Exception ex)
            {
                var path = Environment.CurrentDirectory + "\\log.txt";

                try
                {
                    using (var sw = new StreamWriter(path, false, Encoding.Unicode))
                        sw.WriteLine(ex.Message + " Date: " + DateTime.Now.ToString("g"));
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

        }

        static void Main()
        {
            var waitFromSend = 300000;
            var wait = 5000;
            Console.Title = "Client";
            var timer = new Timer(SendMessage, null, wait, waitFromSend);
            Console.WriteLine("Client started. Time: {0}",DateTime.Now.ToString("g"));
            Console.ReadKey();
        }
    }
}
