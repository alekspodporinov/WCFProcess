using System.ServiceModel;
using Contract;
using Model;
using System;

namespace Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceProcess : IContract
    {
        private readonly Statistic _statistic;

        private ServiceProcess()
        {
            _statistic = new Statistic();
        }

        public bool SendProcess(ClientComputer computer)
        {
            if (computer == null) return false;
            _statistic.Update(computer);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Computer name: {0} sent. Time: {1}", computer.Name, DateTime.Now.ToString("g"));
            Console.ResetColor();
            return true;
        }
    }
}
