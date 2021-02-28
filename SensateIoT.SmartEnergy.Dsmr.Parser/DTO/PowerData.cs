using System.Runtime.Serialization;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.DTO
{
	[DataContract]
	public class PowerData
	{
		[DataMember]
		public decimal InstantaneousPowerUsage { get; set; }
		[DataMember]
		public decimal InstantaneousPowerProduction { get; set; }
		[DataMember]
        public decimal InstantaneousVoltage { get; set; }
		[DataMember]
        public decimal InstantaneousCurrent { get; set; }
	}
}