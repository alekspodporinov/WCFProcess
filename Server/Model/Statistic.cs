using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Contract;
using System.Text;

namespace Model
{
    [Serializable]
    public class Statistic
    {
        private List<ClientStatistic> _statistics;
        private readonly string _path;
        private readonly string _pathLog;
        private Timer _timer;

        public Statistic()
        {
            var threadLock = new object();
            _timer = new Timer(UpdateToFile, threadLock, TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(4));
            _path = Assembly.GetExecutingAssembly().Location;
            _path = _path.Remove(_path.LastIndexOf("\\")) + "\\data.dat";
            _pathLog = Assembly.GetExecutingAssembly().Location;
            _pathLog = _path.Remove(_path.LastIndexOf("\\")) + "\\log.txt";    
            _statistics = new List<ClientStatistic>();

            if (File.Exists(_path))
                ReadStatistics();
        }

        public List<ClientStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        public void Update(ClientComputer computer)
        {
            foreach (var statistic in _statistics.Where(statistic => statistic.ClientName == computer.Name))
            {
                statistic.Update(computer.Process);
                return;
            }
            _statistics.Add(new ClientStatistic(computer.Name, computer.Process));
        }

        public void ReadStatistics()
        {
            try
            {
                using (var fileStream = new FileStream(_path, FileMode.OpenOrCreate))
                {
                    var formater = new BinaryFormatter();
                    _statistics = (List<ClientStatistic>)formater.Deserialize(fileStream);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(ex.Message);
                Console.ResetColor();

                try
                {
                    using (var sw = new StreamWriter(_pathLog, false, Encoding.Unicode))
                        sw.WriteLine(ex.Message + " Date: " + DateTime.Now.ToString("g"));
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

        }

        public void WriteStatistics()
        {
            try
            {
                using (var fileStream = new FileStream(_path, FileMode.OpenOrCreate))
                    new BinaryFormatter().Serialize(fileStream, _statistics);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Data saved. Time: {0}", DateTime.Now.ToString("g"));
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(ex.Message);
                Console.ResetColor();

                try
                {
                    using (var sw = new StreamWriter(_pathLog, false, Encoding.Unicode))
                        sw.WriteLine(ex.Message + " Date: " + DateTime.Now.ToString("g"));
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
        }


        public void UpdateToFile(object obj)
        {
            lock (obj)
                WriteStatistics();

        }
    }
}