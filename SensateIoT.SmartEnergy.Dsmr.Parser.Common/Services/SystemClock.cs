using System;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Common.Services
{
	public class SystemClock : ISystemClock
	{
		public DateTime GetNowUtc()
		{
			return DateTime.UtcNow;
		}
	}
}
