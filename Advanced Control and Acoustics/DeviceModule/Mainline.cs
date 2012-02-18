using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Control.Module.Load;

namespace Control.Module
{
	public abstract class Mainline : iModule, iName
	{
		//
		// Access control for each instance of a module
		//
		private object commandLock = new object();
		private List<String> friendlyName = new List<String>();




		public abstract object getStatus(String statusName);			// No thread safety here!
		public event statusChangeHandler StatusChange;
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

	}
}
