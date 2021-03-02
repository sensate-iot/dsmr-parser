using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Tests.TelegramParser
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void CanParseV2()
        {
        }

        [TestMethod]
        public void CanParseV4()
        {
        }

        [TestMethod]
        public void CanParseV5()
        {
	        var current = Environment.CurrentDirectory;
	        var v5File = $"{current}{Path.DirectorySeparatorChar}Resources{Path.DirectorySeparatorChar}v5.txt";
	        var input = File.ReadAllText(v5File);
		        
			var parser = new Common.Services.Parser();
			var result = parser.Parse(input).GetAwaiter().GetResult();

			Assert.AreEqual(2745.056M, result.GasConsumption);
			Assert.AreEqual(0.306M, result.InstantaneousPowerProduction);
			Assert.AreEqual(2055.686M, result.EnergyConsumptionTariff2);
        }
    }
}
