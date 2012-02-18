using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime;

using Control.Module.Load;


//
// How this should run:
//	* Settings Downloaded into Memory (Required Modules Check, XML Written)
//	* Module list is sent to Module manager that loads modules
//	* Connection list is sent to Connection manager which instansiates modules (Using module manager)
//		and joins setting names to instances (using settings module) and maintains connection information
//	* Mainline manager loads mainline modules (based on Settings)
//		mainline modules provided with interfaces to module settings and connection manager
//		mainline modules can connect to interface manager if they want device feedback
//	* Interface manager, when an interface device/mainline module connects,
//		gets device instance based on settings names (from settings manager)
//		provides event feedback and triggering methods (as required) to connected interfaces
//

namespace Control.TestConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("---- Control Console Started...");

			String file = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
			file = file.Substring(8);
			ModuleLoader moduleLoader = new ModuleLoader(file);
			SettingsProvider settingsProvider = new SettingsProvider();
						
			Console.WriteLine("\tInterrogating XML...");
			
			settingsProvider.loadSettings();

			String folder = Path.Combine(Path.GetDirectoryName(file), "devices");
			Console.WriteLine("---- Loaded Modules");
			
			foreach (SettingsProvider.DeviceSetting device in settingsProvider.CS.devices)
			{
				if(device.instance != null)
				{
					iModule dev = (iModule)device.instance;
					LogNotice("\tLoaded: " + dev.moduleName() + " : " + dev.moduleVersion());
				}
			}
			
			Console.WriteLine("---- Setting up connections...");
			// ToDO


			//
			// Pause the application from exiting
			//
			Console.WriteLine();
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}


		public static void LogError(Object err)
		{
			Console.WriteLine("Error: " + err.ToString());
		}

		public static void LogNotice(Object notice)
		{
			Console.WriteLine(notice.ToString());
		}
	}
}
