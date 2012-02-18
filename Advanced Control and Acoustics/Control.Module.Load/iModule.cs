using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Control.Module.Load
{
	public delegate void statusChangeHandler(iName sender, String statusName);
	public delegate void functionResponseHandler(iName sender, String objectName, object data);
	
	public interface iModule
	{
		event statusChangeHandler StatusChange;
		event functionResponseHandler FunctionResponse;

		void requestFunction(object functionName);	//public delegate void WaitCallback (object state); for threading
		object getStatus(String statusName);		// Should not call request function as could block, however could send output


		/// <summary>
		/// A list of files related to the module, possibly settings or command files.
		/// </summary>
		void relatedFiles(List<String> files);

		string moduleName();
		string moduleDescription();
		string moduleVersion();
	}
}
