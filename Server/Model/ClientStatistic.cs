using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    [Serializable]
    public class ClientStatistic
    {
        private string _clientName;
        private List<ClientProcess> _processStatistics;

        public ClientStatistic(string name, string[] processes)
        {
            _processStatistics = new List<ClientProcess>();
            _clientName = name;
            foreach (var process in processes)
                _processStatistics.Add(new ClientProcess(process));
        }

        public string ClientName
        {
            get { return _clientName; }
            set { _clientName = value; }
        }

        public List<ClientProcess> ProcessStatistics
        {
            get { return _processStatistics; }
            set { _processStatistics = value; }
        }


        public void Update(string[] processes)
        {
            var dateNow = DateTime.Now;
            foreach (var process in from process in _processStatistics from pr in processes where process.Name == pr select process)
            {
                if (dateNow.Hour > 11 && dateNow.Hour < 20)
                    process.TotalWorkTimeProcess += TimeSpan.FromMinutes(5);
                else
                    process.TotalFreeTimeProcess += TimeSpan.FromMinutes(5);
            }


        }
    }

    [Serializable]
    public class ClientProcess
    {
        private string _name;
        private TimeSpan _totalWorkTimeProcess;
        private TimeSpan _totalFreeTimeProcess;

        public ClientProcess(string name)
        {
            _name = name;
            _totalWorkTimeProcess = TimeSpan.Zero;
            _totalFreeTimeProcess = TimeSpan.Zero;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public TimeSpan TotalWorkTimeProcess
        {
            get { return _totalWorkTimeProcess; }
            set { _totalWorkTimeProcess = value; }
        }

        public TimeSpan TotalFreeTimeProcess
        {
            get { return _totalFreeTimeProcess; }
            set { _totalFreeTimeProcess = value; }
        }
    }
}



