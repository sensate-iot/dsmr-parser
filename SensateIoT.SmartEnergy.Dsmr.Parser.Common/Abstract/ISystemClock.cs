using System;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Common.Abstract
{
	public interface ISystemClock
	{
		DateTime GetNowUtc();
	}
}