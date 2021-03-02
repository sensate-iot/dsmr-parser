using System;
using System.Collections.Concurrent;

using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Abstract;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.DTO;

using Telegram = SensateIoT.SmartEnergy.Dsmr.Parser.Common.Models.Telegram;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Common.Services
{
	public class GasFlowCalculator : IGasFlowCalculator
	{
		private readonly ConcurrentDictionary<string, GasFlowCacheEntry> m_telegrams;

		public GasFlowCalculator()
		{
			this.m_telegrams = new ConcurrentDictionary<string, GasFlowCacheEntry>();
		}

		public decimal ComputeFlow(Telegram telegram)
		{
			var result = 0M;

			if(this.m_telegrams.TryGetValue(telegram.SerialNumberGasMeter, out var old)) {
				result = ComputePerMinute(old, telegram);
			}

			this.UpdateCache(telegram);
			return result;
		}

		private static decimal ComputePerMinute(GasFlowCacheEntry old, Telegram @new)
		{
			var diff = DateTime.UtcNow.Subtract(old.Timestamp);
			var usage = @new.GasConsumption - old.Value;

			return usage / Convert.ToDecimal(diff.TotalMinutes);
		}

		private void UpdateCache(Telegram telegram)
		{
			var entry = new GasFlowCacheEntry {
				Timestamp = DateTime.UtcNow,
				Value = telegram.GasConsumption
			};

			this.m_telegrams.AddOrUpdate(telegram.SerialNumberGasMeter, entry, (serial, oldValue) => entry);
		}
	}
}
