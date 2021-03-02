using System;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Models;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Services;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Tests.TelegramParser
{
	[TestClass]
	public class GasFlowCalculatorTests
	{
		[TestMethod]
		public void CanComputeUsagePerMinute()
		{
			var input1 = new Telegram {
				GasConsumption = 1,
				SerialNumberGasMeter = "1234"
			};

			var input2 = new Telegram {
				GasConsumption = 2,
				SerialNumberGasMeter = "1234"
			};

			var calc = new GasFlowCalculator();
			calc.ComputeFlow(input1);
			Thread.Sleep(TimeSpan.FromMilliseconds(1000));
			var diff = calc.ComputeFlow(input2);

			Assert.AreEqual(60, Math.Round(diff), "Unable to compute gas flow!");
		}
	}
}
