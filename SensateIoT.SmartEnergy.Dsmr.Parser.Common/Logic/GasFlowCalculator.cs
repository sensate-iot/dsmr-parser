using System;
using System.Collections.Concurrent;

using log4net;

using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.DTO;

using Telegram = SensateIoT.SmartEnergy.Dsmr.Parser.Common.Models.Telegram;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Common.Logic
{
	public class GasFlowCalculator : IGasFlowCalculator
	{
		private readonly ConcurrentDictionary<string, GasFlowCacheEntry> m_telegrams;
		private readonly ILog m_logger;

		public GasFlowCalculator(ILog logger)
		{
			this.m_logger = logger;
			this.m_telegrams = new ConcurrentDictionary<string, GasFlowCacheEntry>();
		}

		public decimal ComputeFlow(Telegram telegram)
		{
			var result = 0M;

			if(this.m_telegrams.TryGetValue(telegram.SerialNumberGasMeter, out var old)) {
				result = this.ComputePerMinute(old, telegram);
			}

			this.UpdateCache(telegram, result);
			return result;
		}

		private decimal ComputePerMinute(GasFlowCacheEntry old, Telegram @new)
		{
			if(@new.GasTimestamp == old.Timestamp) {
				return old.LastGasFlowResult;
			}

			var diff = @new.GasTimestamp.Subtract(old.Timestamp);
			var usage = @new.GasConsumption - old.Value;

			if(usage < 0) {
				this.m_logger.Warn("New gas m3 lower than previously received. Are telegrams being received out of order?");
				return 0M;
			}

			return usage / Convert.ToDecimal(diff.TotalMinutes);
		}

		private void UpdateCache(Telegram telegram, decimal current)
		{
			var entry = new GasFlowCacheEntry {
				Timestamp = telegram.GasTimestamp,
				Value = telegram.GasConsumption,
				LastGasFlowResult = current
			};

			this.m_telegrams.AddOrUpdate(telegram.SerialNumberGasMeter, entry, (serial, oldValue) => entry);
		}
	}
}
