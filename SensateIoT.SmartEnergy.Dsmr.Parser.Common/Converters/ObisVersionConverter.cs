using System;
using System.ComponentModel;
using System.Globalization;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Models;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Common.Converters
{
    public class ObisVersionConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var stringValue = value as string;

            if (!string.IsNullOrWhiteSpace(stringValue)) {
                switch (stringValue) {
                    case "20":
                        return ObisVersion.V20;
                    case "42":
                        return ObisVersion.V42;
                    case "50":
                        return ObisVersion.V50;
                    default:
                        throw new NotSupportedException($"Value {stringValue} is not a recognized ObisVersion");
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
