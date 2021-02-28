using System;
using System.Diagnostics;
using System.ServiceModel;

using log4net;

using SensateIoT.SmartEnergy.Dsmr.Parser.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Parser.Converters;
using SensateIoT.SmartEnergy.Dsmr.Parser.DTO;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Services
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class ParserService : IParserService
	{
		private readonly Common.Parser m_parser;
		private readonly ILog m_logger;

		public ParserService(ILog logger)
		{
			this.m_parser = new Common.Parser();
			this.m_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public ParserService() { this.m_parser = new Common.Parser(); }

		public Telegram Parse(string frame)
		{
			Telegram result;

			try {
				this.m_logger.Info("Parsing telegram...");
				this.m_logger.Debug($"Telegram data: {frame}");
				var sw = Stopwatch.StartNew();

				var parsed = this.m_parser.Parse(frame).GetAwaiter().GetResult();
				sw.Stop();
				this.m_logger.Info($"Parsing finished in {sw.Elapsed:c}");

				result = TelegramConverter.Convert(parsed);
			} catch(Exception ex) {
				this.m_logger.Error("Unable to parse telegram", ex);
				throw ex;
			}

			return result;
		}
	}
}
