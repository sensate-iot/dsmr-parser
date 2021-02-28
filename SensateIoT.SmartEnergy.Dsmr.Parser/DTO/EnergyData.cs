using System.Runtime.Serialization;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.DTO
{
	[DataContract]
	public class EnergyData
	{
		[DataMember]
		public decimal EnergyConsumptionTariff1 { get; set; }
		[DataMember]
		public decimal EnergyConsumptionTariff2 { get; set; }
		[DataMember]
		public decimal EnergyProductionTariff1 { get; set; }
		[DataMember]
		public decimal EnergyProductionTariff2 { get; set; }
	}
}
