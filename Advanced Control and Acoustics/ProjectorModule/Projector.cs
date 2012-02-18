using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Control.Module;
using Control.Module.Load;
using Control.Utility.Hex;


namespace ProjectorModule
{
	public class Projector : Device, iDevice
	{
		bool projOn = false;


		#region Device Members


		//
		// Do function can wait for the device to respond
		//
		protected override void doFunction(string functionName)
		{
			switch (functionName)
			{
				case "PowerOn":
					doOutput(HexEncoding.GetBytes("02H 01H 00H 00H 00H 03H"));
					break;
				default:
					Console.WriteLine("Invalid selection.");
					throw new NotImplementedException();
			}
		}

		public override object getStatus(string statusName)
		{
			switch (statusName)
			{
				case "isOn":
					return projOn;
				default:
					Console.WriteLine("Invalid selection.");
					throw new NotImplementedException();
			}
		}


		//
		// module communications
		//
		protected override void inputHandler(byte[] input)
		{
			String strInput = HexEncoding.GetString(input);
			Console.WriteLine(strInput);

			doStatusChange("Something Changed signal");
		}

		public override SerialSettings getSerialSettings()
		{
			throw new NotImplementedException();

		}

		public override NetSettings getEthernetSettings()
		{
			throw new NotImplementedException();
		}

		public override USBDeviceID getUSBSettings()
		{
			throw new NotImplementedException();
		}


		public override void connectionReady(object status)
		{
			return;
		}

		public override void connectionLost(object status)
		{
			return;
		}
		
		public override void relatedFiles(List<String> files)
		{
			return;
		}

		public override string moduleName()
		{
			return "NEC Projector";
		}

		public override string moduleDescription()
		{
			return "Controls NEC projectors";
		}

		public override string moduleVersion()
		{
			return "V1.0.0 Beta";
		}

		#endregion
	}
}
