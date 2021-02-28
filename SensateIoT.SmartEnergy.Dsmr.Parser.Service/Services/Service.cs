using System.ServiceProcess;
using SensateIoT.SmartEnergy.Dsmr.Parser.Service.Abstract;

namespace SensateIoT.SmartEnergy.Dsmr.Parser.Service.Services
{
	public class Service : ServiceBase
	{
		private readonly IServer m_server;

		public Service(IServer server)
		{
			this.m_server = server;
		}

		protected override void OnStart(string[] args)
		{
			this.m_server.Start();
		}

		protected override void OnStop()
		{
			this.m_server.Stop();
		}
    }
}
