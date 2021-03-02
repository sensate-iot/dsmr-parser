using System.Threading.Tasks;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Models;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Common.Abstract
{
	public interface IParser
	{
		Task<Telegram> Parse(string message);
	}
}
