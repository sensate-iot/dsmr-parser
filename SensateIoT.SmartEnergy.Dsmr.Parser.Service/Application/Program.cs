using System;
using System.Diagnostics;
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
			var server = new Server();
			var program = new ConsoleHost(server);

			program.Run();

			if(!Debugger.IsAttached) {
				return;
			}

			Console.WriteLine("Press <ENTER> key to close...");
			Console.ReadLine();
        }
    }
}
