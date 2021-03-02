using System;
using System.Collections.Concurrent;

using log4net;

using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.DTO;

using Telegram = SensateIoT.SmartEnergy.Dsmr.Parser.Common.Models.Telegram;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Common.Services
{
	public class GasFlowCalculator : IGasFlowCalculator
	{
		private readonly ConcurrentDictionary<string, GasFlowCacheEntry> m_telegrams;
		private readonly ILog m_logger;
		private readonly ISystemClock m_clock;

		public GasFlowCalculator(ISystemClock clock, ILog logger)
		{
			this.m_logger = logger;
			this.m_telegrams = new ConcurrentDictionary<string, GasFlowCacheEntry>();
			this.m_clock = clock;
		}

		public decimal ComputeFlow(Telegram telegram)
		{
			var result = 0M;

			if(this.m_telegrams.TryGetValue(telegram.SerialNumberGasMeter, out var old)) {
				result = this.ComputePerMinute(old, telegram);
			}

			this.UpdateCache(telegram);
			return result;
		}

		private decimal ComputePerMinute(GasFlowCacheEntry old, Telegram @new)
		{
			var diff = this.m_clock.GetNowUtc().Subtract(old.Timestamp);
			var usage = @new.GasConsumption - old.Value;

			if(usage < 0) {
				this.m_logger.Warn("New gas m3 lower than previously received. Are telegrams being received out of order?");
				return 0M;
			}

			return usage / Convert.ToDecimal(diff.TotalMinutes);
		}

		private void UpdateCache(Telegram telegram)
		{
			var entry = new GasFlowCacheEntry {
				Timestamp = this.m_clock.GetNowUtc(),
				Value = telegram.GasConsumption
			};

			this.m_telegrams.AddOrUpdate(telegram.SerialNumberGasMeter, entry, (serial, oldValue) => entry);
		}
	}
}
