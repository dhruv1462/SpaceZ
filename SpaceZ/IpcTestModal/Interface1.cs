using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IpcTestModel
{
    public interface IStateChangeNotification
    {
        [OperationContract(IsOneWay = true)]
        void StateChanged(string newState);
    }


    [ServiceContract(CallbackContract = typeof(IStateChangeNotification))]
    public interface ISubscribeForStateChanges
    {
        [OperationContract(IsOneWay = true)]
        void SubscribeClient(string appKey);
    }

}
