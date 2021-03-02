using SensateIoT.SmartEnergy.Dsmr.Parser.Contracts.DTO;
using SensateIoT.SmartEnergy.Dsmr.Parser.Data.Models;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Common.Converters
{
	public static class TelegramConverter
	{
		public static Telegram Convert(Models.Telegram input)
		{
			return new Telegram() {
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