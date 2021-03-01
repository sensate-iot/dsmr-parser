using System.ServiceModel;
using SensateIoT.SmartEnergy.Dsmr.Parser.Contracts.DTO;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Contracts.Abstract
{
	[ServiceContract]
	public interface IParserService 
	{
		[OperationContract]
		Telegram Parse(string frame);
	}
}
