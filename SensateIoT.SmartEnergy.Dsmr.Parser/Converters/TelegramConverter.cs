using SensateIoT.SmartEnergy.Dsmr.Common.Models;
using SensateIoT.SmartEnergy.Dsmr.Parser.DTO;

using Telegram = SensateIoT.SmartEnergy.Dsmr.Parser.DTO.Telegram;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Converters
{
	public static class TelegramConverter
	{
		public static Telegram Convert(Common.Models.Telegram input)
		{
			return new Telegram {
				GasConsumption = input.GasConsumption,
				CurrentTariff = input.CurrentTariff == PowerTariff.Normal ? "NORMAL" : "LOW",
				EnergyData =
					new EnergyData {
						EnergyConsumptionTariff1 = input.EnergyConsumptionTariff1,
						EnergyConsumptionTariff2 = input.EnergyConsumptionTariff2,
						EnergyProductionTariff1 = input.EnergyProductionTariff1,
						EnergyProductionTariff2 = input.EnergyProductionTariff2
					},
				PowerData = new PowerData {
					InstantaneousCurrent = input.InstantaneousCurrent,
					InstantaneousVoltage = input.InstantaneousVoltage,
					InstantaneousPowerProduction = input.InstantaneousPowerProduction,
					InstantaneousPowerUsage = input.InstantaneousPowerUsage
				}
			};


		}
	}
}