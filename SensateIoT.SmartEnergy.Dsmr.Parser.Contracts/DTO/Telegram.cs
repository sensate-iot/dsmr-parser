using System.Runtime.Serialization;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Contracts.DTO
{
	[DataContract]
	public class Telegram
	{
		[DataMember]
		public PowerData PowerData { get; set; }
		[DataMember]
		public EnergyData EnergyData { get; set; }
		[DataMember]
		public GasData GasData { get; set; }
		[DataMember]
		public string CurrentTariff { get; set; }
	}
}
