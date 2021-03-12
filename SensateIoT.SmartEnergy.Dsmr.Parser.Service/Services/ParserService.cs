using System;
using System.Diagnostics;
using System.ServiceModel;

using log4net;

using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Converters;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Logic;
using SensateIoT.SmartEnergy.Dsmr.Parser.Contracts.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Parser.Contracts.DTO;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Service.Services
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public class ParserService : IParserService
	{
		private static readonly ILog logger = LogManager.GetLogger(nameof(ParserService));

		private readonly IGasFlowCalculator m_calc;
		private readonly IParser m_parser;

		public ParserService(IParser parser)
		{
			this.m_calc = new GasFlowCalculator(LogManager.GetLogger(nameof(GasFlowCalculator)));
			this.m_parser = parser;
		}

		public Telegram Parse(string frame)
		{
			Telegram result;

			try {
				logger.Info("Parsing telegram...");
				logger.Debug($"Telegram data: {frame}");
				var sw = Stopwatch.StartNew();

				var parsed = this.m_parser.Parse(frame).GetAwaiter().GetResult();
				sw.Stop();
				logger.Info($"Parsing finished in {sw.Elapsed:c}");

				result = TelegramConverter.Convert(parsed);
				this.ComputeGasFlow(result, parsed);
			} catch(Exception ex) {
				logger.Error("Unable to parse telegram", ex);
				throw ex;
			}

			return result;
		}

		private void ComputeGasFlow(Telegram telegram, Common.Models.Telegram parsed)
		{
			if(telegram.GasData == null) {
				return;
			}

			telegram.GasData.GasFlow = this.m_calc.ComputeFlow(parsed);
		}
	}
}
