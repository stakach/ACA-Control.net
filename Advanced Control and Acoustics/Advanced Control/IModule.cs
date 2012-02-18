using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Control.Module.Load
{
	public delegate void outputHandler(byte[] output);
	public delegate void statusChangeHandler(String statusName);

	interface iModule
	{
		//
		// Module core interaction
		//
		public void requestFunction(object functionName);
		public object getStatus(String statusName);
		public event statusChangeHandler StatusChange;


		//
		// Module communications
		//
		public void inputHandler(byte[] input);
		public event outputHandler Output;
	}
}
