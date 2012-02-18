using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Control.Module.Load
{
	public interface iName
	{
		//
		// For internal use
		//
		void setFriendlyName(String name);
		List<String> getFriendlyName();
	}
}
