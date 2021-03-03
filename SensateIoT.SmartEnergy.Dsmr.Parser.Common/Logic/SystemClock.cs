using System;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Common.Logic
{
	public class SystemClock : ISystemClock
	{
		public DateTime GetNowUtc()
		{
			return DateTime.UtcNow;
		}
	}
}
