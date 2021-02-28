﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

using SensateIoT.SmartEnergy.Dsmr.Common.Attributes;
using SensateIoT.SmartEnergy.Dsmr.Common.Converters;

namespace SensateIoT.SmartEnergy.Dsmr.Common.Models
{
    public class Telegram
    {
        private const string LineEnding = "\r\n";

        public string MessageHeader { get; set; }
        [Obis("1-3:0.2.8")]
        public ObisVersion MessageVersion { get; set; }
        [Obis("0-0:96.1.1")]
        public string SerialNumberElectricityMeter { get; set; }
        [Obis("0-1:96.1.0")]
        public string SerialNumberGasMeter { get; set; }
        [Obis("0-0:1.0.0"), TypeConverter(typeof(ObisTimestampConverter))]
        public DateTime Timestamp { get; set; }
        [Obis("1-0:1.8.1", ValueUnit = "kWh")]
        public decimal EnergyConsumptionTariff1 { get; set; } = 0M;
        [Obis("1-0:1.8.2", ValueUnit = "kWh")]
        public decimal EnergyConsumptionTariff2 { get; set; } = 0M;
        [Obis("1-0:2.8.1", ValueUnit = "kWh")]
        public decimal EnergyProductionTariff1 { get; set; } = 0M;
        [Obis("1-0:2.8.2", ValueUnit = "kWh")]
        public decimal EnergyProductionTariff2 { get; set; } = 0M;
        [Obis("0-0:96.14.0")]
        public PowerTariff CurrentTariff { get; set; }
        [Obis("1-0:1.7.0", ValueUnit = "kW")]
        public decimal InstantaneousPowerUsage { get; set; } = 0M;
        [Obis("1-0:2.7.0", ValueUnit = "kW")]
        public decimal InstantaneousPowerProduction { get; set; } = 0M;
        [Obis("1-0:32.7.0", ValueUnit = "V")]
        public decimal InstantaneousVoltage { get; set; } = 0M;
        [Obis("1-0:31.7.0", ValueUnit = "A")]
        public decimal InstantaneousCurrent { get; set; } = 0M;
        [Obis("0-1:24.2.1", 1, "m3")]
        public decimal GasConsumption { get; set; } = 0M;
        [Obis("0-1:24.2.1", 0), TypeConverter(typeof(ObisTimestampConverter))]
        public DateTime GasTimestamp { get; set; }
        public string CRC { get; set; }
        public IList<string> Lines { get; set; } = new List<string>();

        public override string ToString()
        {
            return string.Join(LineEnding, Lines) + LineEnding;
        }
    }

    [TypeConverter(typeof(ObisTariffConverter))]
    public enum PowerTariff
    {
        Low,
        Normal
    }

    [TypeConverter(typeof(ObisVersionConverter))]
    public enum ObisVersion
    {
        V20,
        V42,
        V50
    }
}
