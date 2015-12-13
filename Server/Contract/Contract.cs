using System.Runtime.Serialization;
using System.ServiceModel;

namespace Contract
{
    [ServiceContract]
    public interface IContract
    {
        [OperationContract]
        bool SendProcess(ClientComputer computer);
    }

    [DataContract]
    public class ClientComputer
    {
        private string _name;
        private string[] _process;

        public ClientComputer()
        {
            _name = string.Empty;
            _process = null;
        }

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [DataMember]
        public string[] Process
        {
            get { return _process; }
            set { _process = value; }
        }
    }
}
