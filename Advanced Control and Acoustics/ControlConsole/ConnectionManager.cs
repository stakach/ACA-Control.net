using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;


using Control.Module.Load;


namespace Control.TestConsole
{
	public class ConnectionManager
	{
		//
		// list of connections? abstracted?
		//
		String[] PortNames;
		protected List<SerialPort> comports = new List<SerialPort>();


		public ConnectionManager()
		{
			SettingsProvider.newSettings += new SettingsProvider.settingsChanged(moduleListChanged);

			//
			// Must serialise a list of connection devices here
			//	This includes COM ports and avaliable NICs
			//
			getDeviceList();
		}

		//
		// Start connections on new modules loaded
		//
		public void moduleListChanged(SettingsProvider.ControlSystem CC, SettingsProvider.ServerSettings SS)
		{
			//
			// loop through connections here
			//
		}


		protected void getDeviceList()
		{
			PortNames = SerialPort.GetPortNames();
		}


		public void AssociateIP(string IP, int port, iModule dev)
		{
		}

		public void AssociateSerial(SerialSettings settings, int port, iModule dev)
		{
		}

		public void startConnections()
		{
		}


	}
}
