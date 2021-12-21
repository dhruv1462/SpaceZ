using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace InformationService
{
    public interface ICallbackService
    {
        [OperationContract(IsOneWay = true)]
        void SendCallbackMessage(string message);
    }
}
