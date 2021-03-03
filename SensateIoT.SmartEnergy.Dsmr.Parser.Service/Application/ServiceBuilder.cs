using System;
using System.ServiceModel;

using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Parser.Service.Services;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Service.Application
{
    public class ServiceBuilder
    {
        private string m_hostname;
        private string m_path;
        private int m_port;
        private IParser m_parser;

        public static ServiceBuilder Create()
        {
	        var builder = new ServiceBuilder {m_hostname = "localhost", m_port = 80, m_path = ""};
	        return builder;
        }

        public ServiceHost Build()
        {
            var uri = new Uri($"http://{this.m_hostname}:{this.m_port}/{this.m_path}");
            return new ServiceHost(new ParserService(this.m_parser), uri);
        }

        public ServiceBuilder WithParser(IParser parser)
        {
	        this.m_parser = parser;
	        return this;
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
