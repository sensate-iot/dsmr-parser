﻿using System;
using System.ComponentModel;
using System.Globalization;

using SensateIoT.SmartEnergy.Dsmr.Common.Models;

namespace SensateIoT.SmartEnergy.Dsmr.Common.Converters
{
    public class ObisTariffConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var stringValue = value as string;

            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                switch (stringValue)
                {
                    case "0001":
                        return PowerTariff.Low;
                    case "0002":
                        return PowerTariff.Normal;
                    default:
                        throw new NotSupportedException($"Value {stringValue} is not a recognized ObisTariff");
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
