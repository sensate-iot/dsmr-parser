using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Models;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Common.Abstract
{
	public interface IGasFlowCalculator
	{
		decimal ComputeFlow(Telegram telegram);
	}
}