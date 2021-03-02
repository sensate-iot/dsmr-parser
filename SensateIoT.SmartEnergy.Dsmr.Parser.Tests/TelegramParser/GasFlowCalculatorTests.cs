using System;
using log4net;
using Moq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Abstract;
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

			var now = DateTime.UtcNow;
			var log = new Mock<ILog>();
			var clock = new Mock<ISystemClock>();
			clock.Setup(x => x.GetNowUtc()).Returns(now);
			var calc = new GasFlowCalculator(clock.Object, log.Object);

			calc.ComputeFlow(input1);
			now = now.Add(TimeSpan.FromSeconds(1));
			clock.Setup(x => x.GetNowUtc()).Returns(now);
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
			var clock = new Mock<ISystemClock>();

			return new GasFlowCalculator(clock.Object, log.Object);
		}
	}
}
