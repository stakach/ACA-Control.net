using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Control.Module.Load
{
	public delegate void outputHandler(iName sender, byte[] output);
	
	
	//
	// Say a new connection type comes out we can extend the system by creating a new interface
	//	that implements the same functions as iConnect, then the inheriting type will inherit
	//	both the new interface and iconnect. So no changes will need to happen to the rest
	//	of the system (communications will be the same after setup)
	//
	public interface iConnect
	{
		void sendInput(object input);	// Meets thread pool requirements too
		event outputHandler Output;

		void connectionReady(object status);
		void connectionLost(object status);

		//
		// Create a function to allow device to control the connection?
		//	Ie disconnect and re-connect
		//
	}
}
