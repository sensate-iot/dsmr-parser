using System;
using System.Threading;

using log4net;
using Moq;

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

			var calc = createGasFlowCalculator();
			calc.ComputeFlow(input1);
			Thread.Sleep(TimeSpan.FromMilliseconds(990));
			var diff = calc.ComputeFlow(input2);

			Assert.AreEqual(60, Math.Round(diff), "Unable to compute gas flow!");
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
				SerialNumberGasMeter = "1234"
			};
			var input2 = new Telegram {
				GasConsumption = 0.5M,
				SerialNumberGasMeter = "1234"
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
