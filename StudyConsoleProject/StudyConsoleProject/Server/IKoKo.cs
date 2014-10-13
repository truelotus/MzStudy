using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;

namespace StudyConsoleProject.Server
{
    [ServiceContract]
    public interface IKoKo
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "test")]
        string Test();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "test2?text={text}")]
        string Test2(string text);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "test3?text={text}")]
        Stream Test3(string text);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "GetMyDocumentList?move={path}")]
        Stream Move(string path);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "fordericon")]
        Stream GetFolderIcon();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "fileicon")]
        Stream GetFileIcon();
    }
}
