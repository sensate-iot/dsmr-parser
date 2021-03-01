using System;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Common.Attributes
{
    //[Obis("1-0:1.8.1", valueIndex=1, valueUnit="kWh")]
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ObisAttribute : Attribute
    {
        public string ObisIdentifier { get; set; }
        public int ValueIndex { get; set; }
        public string ValueUnit { get; set; }

        public ObisAttribute(string obisIdentifier, int valueIndex = 0, string valueUnit = null)
        {
            if (string.IsNullOrWhiteSpace(obisIdentifier))
            {
                throw new ArgumentException("Value cannot be null or empty", nameof(obisIdentifier));
            }

            if (valueIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(valueIndex), "Value cannot be negative");
            }

            this.ObisIdentifier = obisIdentifier;
            this.ValueIndex = valueIndex;
            this.ValueUnit = valueUnit;
        }
    }
}
