using System;
using System.ComponentModel;
using System.Globalization;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Common.Converters
{
    public class ObisTimestampConverter : TypeConverter
    {
        //Timestamps in format: YYMMddHHmmss[W|S]

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var stringValue = value as string;

            if(string.IsNullOrWhiteSpace(stringValue)) {
	            return base.ConvertFrom(context, culture, value);
            }

            stringValue = stringValue.Substring(0, stringValue.Length - 1); //remove 'W' or 'S'
            return DateTime.ParseExact(stringValue, "yyMMddHHmmss", CultureInfo.InvariantCulture);

        }
    }
}
