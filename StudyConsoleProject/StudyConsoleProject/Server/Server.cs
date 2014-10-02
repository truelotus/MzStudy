using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudyConsoleProject.Server
{
    // --------------------------------------------------------------------------------------------------------------------
    // <copyright file="Server.cs" company="">
    //   
    // </copyright>
    // <summary>
    //   The server.
    // </summary>
    // --------------------------------------------------------------------------------------------------------------------

    namespace CameraControllerCore.Server
    {
        #region

        using System;
        using System.Collections.Generic;
        using System.Configuration;
        using System.Diagnostics;
        using System.Linq;
        using System.Net;
        using System.ServiceModel;
        using System.ServiceModel.Description;
        using System.ServiceModel.Web;

        #endregion

        /// <summary>
        /// The server.
        /// </summary>
        public class KoServer : IDisposable
        {
            #region Constants and Fields

            /// <summary>
            ///   The default service port.
            /// </summary>
            private const int DefaultServicePort = 80;

            /// <summary>
            /// The rest service address prefix.
            /// </summary>
            private const string RestServiceAddressPrefix = "http://localhost:{0}";

            /// <summary>
            /// The hosts.
            /// </summary>
            private readonly List<ServiceHost> hosts = new List<ServiceHost>();

            /// <summary>
            ///   The port.
            /// </summary>
            private readonly int port;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///   Initializes a new instance of the <see cref = "Server" /> class.
            /// </summary>
            public KoServer()
            {
                ServicePointManager.DefaultConnectionLimit = 100;

                var portString = ConfigurationManager.AppSettings["ServicePort"];

                int outPort;

                if (!int.TryParse(portString, out outPort))
                {
                    Console.WriteLine("Port is Worng : {0} - Use Default port : {1}", portString, DefaultServicePort);
                    outPort = DefaultServicePort;
                }

                this.port = outPort;

                var hostForRestPtz = new WebServiceHost(typeof(KoKo), new Uri(this.RestServiceAddress));
                var webHttpBinding = new WebHttpBinding { CrossDomainScriptAccessEnabled = true };
                var restServiceEndpoint = hostForRestPtz.AddServiceEndpoint(
                    typeof(IKoKo), webHttpBinding, new Uri(this.RestServiceAddress));
                restServiceEndpoint.Behaviors.Add(
                    new WebHttpBehavior { HelpEnabled = true, AutomaticFormatSelectionEnabled = false, });

                this.hosts.Add(hostForRestPtz);
            }

            #endregion

            #region Properties

            /// <summary>
            ///   Gets BaseAddresses.
            /// </summary>
            public List<Uri> BaseAddresses
            {
                get
                {
                    if (this.hosts == null || this.hosts.Count == 0)
                    {
                        return null;
                    }

                    var addresses = new List<Uri>();

                    foreach (var serviceHost in this.hosts)
                    {
                        addresses.AddRange(serviceHost.BaseAddresses);
                    }

                    return addresses;
                }
            }
            
            /// <summary>
            /// Gets RestServiceAddress.
            /// </summary>
            private string RestServiceAddress
            {
                get
                {
                    return string.Format(RestServiceAddressPrefix, this.port);
                }
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// The close.
            /// </summary>
            public void Close()
            {
                if (this.hosts != null && this.hosts.Count != 0)
                {
                    this.hosts.AsParallel().ForAll(host => host.Close());
                    this.hosts.Clear();
                }
            }

            /// <summary>
            /// The run.
            /// </summary>
            public void Run()
            {
                if (this.hosts == null || this.hosts.Count == 0)
                {
                    throw new Exception("Run Server Fail. There are no host Exists.");
                }

                foreach (var serviceHost in this.hosts)
                {
                    serviceHost.Open();
                    serviceHost.Description.Endpoints.AsParallel().ForAll(
                        ep => Console.WriteLine("Service is Running on {0}", ep.Address));
                }
            }

            #endregion

            #region Implemented Interfaces

            #region IDisposable

            /// <summary>
            /// The dispose.
            /// </summary>
            public void Dispose()
            {
                this.Close();
            }

            #endregion

            #endregion
        }
    }
}
