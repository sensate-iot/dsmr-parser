using System.ComponentModel;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Converters;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Common.Models
{
	[TypeConverter(typeof(ObisVersionConverter))]
    public enum ObisVersion
    {
        V20,
        V42,
        V50
    }
}