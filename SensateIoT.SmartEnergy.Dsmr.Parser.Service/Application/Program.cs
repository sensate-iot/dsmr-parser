using System;
using System.ServiceProcess;

using log4net;

using SensateIoT.SmartEnergy.Dsmr.Parser.Service.Services;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Service.Application
{
    public class Program
    {
	    private static readonly ILog logger = LogManager.GetLogger("DsmrParserService");

        public static void Main(string[] args)
        {
			logger.Warn("Starting DSMR parser service.");

	        if(Environment.UserInteractive) {
				RunInteractive();
	        } else {
				logger.Info("Starting Windows service.");
		        using(var service = new Services.Service(new Server())) {
					ServiceBase.Run(service);
		        }
	        }
        }

        private static void RunInteractive()
        {
			logger.Info("Starting as console application.");
			var server = new Server();
			server.Start();
			Console.WriteLine();
			Console.ReadLine();
			server.Stop();
        }
    }
}
