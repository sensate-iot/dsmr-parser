﻿using System;
using System.ServiceModel;

using log4net;

using SensateIoT.SmartEnergy.Dsmr.Parser.Service.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Parser.Service.Application;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Service.Services
{
    public class Server : IServer
    {
		private ServiceHost m_host;
		private static ILog logger = LogManager.GetLogger("Server");

		public void Start()
		{
            var builder = ServiceBuilder.Create();

            builder.WithHostname("localhost");
            builder.WithPort(8080);
            builder.WithPath("Dsmr");

            var service = builder.Build();

            this.m_host = service;
            this.m_host.Open();
            logger.Info("Server started.");
		}

		public void Stop()
		{
            logger.Warn("Stopping DSMR parser server.");
            this.m_host.Close(TimeSpan.FromSeconds(2));
		}
    }
}
