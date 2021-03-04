using System;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Common.DTO
{
	public class GasFlowCacheEntry
	{
		public DateTime Timestamp { get; set; }
		public decimal Value { get; set; }
		public decimal LastGasFlowResult { get; set; }
	}
}
