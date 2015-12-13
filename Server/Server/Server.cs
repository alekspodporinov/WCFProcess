using System;
using System.ServiceModel;
using Contract;
using Service;
using System.IO;
using System.Text;

namespace Hosting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";

            var host = new ServiceHost(typeof(ServiceProcess));


            var binding = new BasicHttpBinding
            {
                CloseTimeout = TimeSpan.MaxValue,
                OpenTimeout = TimeSpan.MaxValue,
                ReceiveTimeout = TimeSpan.MaxValue
            };
            try
            {
                host.AddServiceEndpoint(typeof(IContract), binding, "http://localhost:7000");
                host.Open();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Server started... Time: {0}", DateTime.Now.ToString("g"));
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Host fell!!! Read more in log.txt... Time: {0}", DateTime.Now.ToString("g"));
                Console.ResetColor();

                string path = Environment.CurrentDirectory + "\\log.txt";

                try
                {
                    using (var sw = new StreamWriter(path, false, Encoding.Unicode))
                        sw.WriteLine(ex.Message + " Date: " + DateTime.Now.ToString("g"));
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            Console.ReadKey();

            host.Close();
        }
    }
}
