using System;

using log4net;
using Moq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Logic;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Models;

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
				SerialNumberGasMeter = "1234",
				GasTimestamp = DateTime.UtcNow
			};

			var input2 = new Telegram {
				GasConsumption = 2,
				SerialNumberGasMeter = "1234",
				GasTimestamp = DateTime.UtcNow.AddSeconds(1)
			};

			var log = new Mock<ILog>();
			var calc = new GasFlowCalculator(log.Object);

			calc.ComputeFlow(input1);
			var diff = calc.ComputeFlow(input2);

			Assert.AreEqual(60, Math.Round(diff));
		}

		[TestMethod]
		public void ComputeReturnsZeroOnFirstTelegram()
		{
			var input1 = new Telegram {
				GasConsumption = 1,
				SerialNumberGasMeter = "1234"
			};

			var calc = createGasFlowCalculator();
			var result = calc.ComputeFlow(input1);

			Assert.AreEqual(0M, result);
		}

		[TestMethod]
		public void CannotComputeNegativeFlow()
		{
			var input1 = new Telegram {
				GasConsumption = 1,
				SerialNumberGasMeter = "1234",
				GasTimestamp = DateTime.UtcNow
			};
			var input2 = new Telegram {
				GasConsumption = 0.5M,
				SerialNumberGasMeter = "1234",
				GasTimestamp = DateTime.UtcNow.AddSeconds(1)
			};

			var calc = createGasFlowCalculator();
			calc.ComputeFlow(input1);
			var result = calc.ComputeFlow(input2);

			Assert.AreEqual(0M, result);
		}

		private static GasFlowCalculator createGasFlowCalculator()
		{
			var log = new Mock<ILog>();
			return new GasFlowCalculator(log.Object);
		}
	}
}
