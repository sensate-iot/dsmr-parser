using System.ServiceModel;
using SensateIoT.SmartEnergy.Dsmr.Parser.DTO;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Abstract
{
	[ServiceContract]
	public interface IParserService 
	{
		[OperationContract]
		Telegram Parse(string frame);
	}
}
