using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace StudyConsoleProject.Server
{
    [ServiceContract]
    public interface IKoKo
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "test")]
        string Test();
    }
}
