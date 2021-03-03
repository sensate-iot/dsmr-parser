using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SensateIoT.SmartEnergy.Dsmr.Parser.Common.Models;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Tests.TelegramParser
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
		[DeploymentItem("Resources/v2.txt", "Resources")]
        public void CanParseV2()
        {
	        var input = getTelegram("2");
			var parser = new Common.Logic.Parser();
			var result = parser.Parse(input).GetAwaiter().GetResult();

			Assert.AreEqual(ObisVersion.V20, result.MessageVersion);
			Assert.AreEqual(0.98M, result.InstantaneousPowerUsage);
			Assert.AreEqual(185M, result.EnergyConsumptionTariff1);
			Assert.AreEqual(13M, result.EnergyProductionTariff1);
        }

        [TestMethod]
		[DeploymentItem("Resources/v4.txt", "Resources")]
        public void CanParseV4()
        {
	        var input = getTelegram("4");
	        var parser = new Common.Logic.Parser();
	        var result = parser.Parse(input).GetAwaiter().GetResult();

	        Assert.AreEqual(ObisVersion.V42, result.MessageVersion);
	        Assert.AreEqual(0.494M, result.InstantaneousPowerUsage);
	        Assert.AreEqual(2074.842M, result.EnergyConsumptionTariff1);
	        Assert.AreEqual(10.981M, result.EnergyProductionTariff1);
		}

        [TestMethod]
		[DeploymentItem("Resources/v5.txt", "Resources")]
        public void CanParseV5()
        {
	        var input = getTelegram("5");
			var parser = new Common.Logic.Parser();
			var result = parser.Parse(input).GetAwaiter().GetResult();

			Assert.AreEqual(2745.056M, result.GasConsumption);
			Assert.AreEqual(0.306M, result.InstantaneousPowerProduction);
			Assert.AreEqual(2055.686M, result.EnergyConsumptionTariff2);
			Assert.AreEqual(ObisVersion.V50, result.MessageVersion);
        }

        private static string getTelegram(string version)
        {
			var v5File = $"Resources{Path.DirectorySeparatorChar}v{version}.txt";
	        return File.ReadAllText(v5File);
        }
    }
}
