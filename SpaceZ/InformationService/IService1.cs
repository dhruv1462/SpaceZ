using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace InformationService
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ICallbackService))]
    public interface IService1
    {
        [OperationContract(IsOneWay = true)]
        void Connect();

        /* [OperationContract(IsOneWay = true)]
         [FaultContract(typeof(Telemetry))]
         Telemetry getTelemetry(String spacecraftName);
        */
        [OperationContract(IsOneWay = true)]
        void ShowMessage(string message);
    }

}
