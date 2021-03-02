using System.ComponentModel;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Converters;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Data.Models
{ 
	[TypeConverter(typeof(ObisTariffConverter))]
    public enum PowerTariff
    {
        Low,
        Normal
    }
}