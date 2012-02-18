using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Control.Module.Load;

namespace Control.Module
{
	public abstract class Device : iModule, iConnect, iSettings1, iName
	{
		//
		// Access control for each instance of a module
		//
		private object inputLock = new object();
		private object commandLock = new object();
		private List<String> friendlyName = new List<string>();
		


		#region Module core interaction (Status and Command signaling)


		//
		// Provide listeners with data they may want when processing a function.
		//
		public event functionResponseHandler FunctionResponse;

		protected bool dataIsRequired()
		{
			return FunctionResponse != null;
		}

		protected void provideData(String identifier, object data)
		{
			functionResponseHandler handler = FunctionResponse;
			if (handler != null)
			{
				handler(this, identifier, data);
			}
		}
		
		
		
		//
		// Provide locking so only one function can be requested at a time.
		//
		protected abstract void doFunction(String functionName);
		public void requestFunction(object functionName)	//public delegate void WaitCallback (object state);
		{
			lock (commandLock)
			{
				doFunction(functionName.ToString());
			}
		}

		//
		// Inform listeners about status changes
		//
		public abstract object getStatus(String statusName);			// No thread safety here!
		public event statusChangeHandler StatusChange;

		//
		// Delegate access for modules:
		//
		protected bool statusIsRequired()
		{
			return StatusChange != null;
		}

		protected void doStatusChange(String statusName)
		{
			//
			// Thread safe, exception avoiding, event management
			//
			statusChangeHandler handler = StatusChange;
			if (handler != null)
			{
				handler(this, statusName);
			}
		}


		#endregion


		#region Module to device communications (Raw device data, in and out)

		//
		// Input with locking to provide serial processing
		//
		protected abstract void inputHandler(byte[] input);
		public void sendInput(object input)
		{
			if (input.GetType() == typeof(byte[]))
			{
				lock (inputLock)
				{
					inputHandler((byte[])input);
				}
			}
		}
		
		public event outputHandler Output;

		protected bool outputIsDefined()
		{
			return Output != null;
		}

		protected void doOutput(byte[] output)
		{
			outputHandler handler = Output; //Threadsafe delegates
			if (handler != null)
			{
				handler(this, output);
			}
		}

		#endregion


		#region Communication Settings (connection settings: IP Port, Serial Speed, usb ids)


		public abstract SerialSettings getSerialSettings();
		public abstract NetSettings getEthernetSettings();
		public abstract USBDeviceID getUSBSettings();


		#endregion


		#region Module Information and Initialisation (Connection status, module version ect)

		/// <summary>
		/// Informs the module when the system is ready for it to start communicating.
		/// Allows the module to perform any additional setup or status requests.
		/// </summary>
		public abstract void connectionReady(object status);

		/// <summary>
		/// Informs the module if the connection is lost.
		/// </summary>
		public abstract void connectionLost(object status);



		//
		// Implemetation of Module interface:
		//
		public abstract void relatedFiles(List<String> files);


		public abstract string moduleName();
		public abstract string moduleDescription();
		public abstract string moduleVersion();

		public void setFriendlyName(String name)
		{
			friendlyName.Add(name);
		}
		public List<String> getFriendlyName()
		{
			return friendlyName;
		}

		#endregion
    }
}
