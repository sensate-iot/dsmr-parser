using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using log4net;
using SensateIoT.SmartEnergy.Dsmr.Parser.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Service.Application
{
    public class ServiceBuilder
    {
        private string m_hostname;
        private string m_path;
        private int m_port;

        public static ServiceBuilder Create()
        {
            var builder = new ServiceBuilder();

            builder.m_hostname = "localhost";
            builder.m_port = 80;
            builder.m_path = "";

            return builder;
        }

        public ServiceHost Build()
        {
            var uri = new Uri($"http://{this.m_hostname}:{this.m_port}/{this.m_path}");
            //var service = new DsmrParserServiceHost(LogManager.GetLogger("DsmrParserService"), typeof(Parser.Services.ParserService), uri);
            var service = new ServiceHost(new Parser.Services.ParserService(LogManager.GetLogger("ParserService")), uri);
            //Parser.Services.ParserService.logger = LogManager.GetLogger("DsmrParserService");
            //var service = new ServiceHost(typeof(Parser.Services.ParserService), uri);

            service.AddServiceEndpoint(typeof(IParserService), new WSHttpBinding(), "ParserService");
            var smb = new ServiceMetadataBehavior {HttpGetEnabled = true};
            service.Description.Behaviors.Add(smb);

            return service;
        }

        public ServiceBuilder WithHostname(string hostname)
        {
            this.m_hostname = hostname;
            return this;
        }

        public ServiceBuilder WithPort(int port)
        {
            this.m_port = port;
            return this;
        }

        public ServiceBuilder WithPath(string path)
        {
            this.m_path = path;
            return this;
        }
    }
}
