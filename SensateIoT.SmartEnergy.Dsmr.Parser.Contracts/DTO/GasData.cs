using System.Runtime.Serialization;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Contracts.DTO
{
	public class GasData
	{
		
		[DataMember]
		public decimal GasConsumption { get; set; }
		[DataMember]
		public decimal GasFlow { get; set; }
	}
}
