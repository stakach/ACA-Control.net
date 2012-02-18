using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using Control.Module.Load;

namespace Control.TestConsole
{
	public class SettingsProvider
	{
		public SettingsProvider()
		{
			//
			// Connect to database here
			// fall back to XML
			//	if DB connection found, load settings and re-write xml
			//	Check XML and DB settings every 30min
			//	Interface provided to trigger re-load
			//	If settings have changed, apply the changes to the live system
			//	on settings change if using DB re-write the XML
			//

			// If DB unavaliable then rely on XML
			// Download modules that are not avaliable locally from the server
			// Delete un-required local modules after properly unloading when change applied

			// Method of loading new modules + unloading
			//	* download new module
			//	* Load new module
			//	* Kill connection of old module
			//	* Start connection of new module
			//	* Unload old module
			//	* delete old module
		}


		#region Type Definitions


		[XmlType("ServerInfo")]
		public struct ServerSettings
		{
			[XmlElement("Hostname")]
			public String hostname;
			[XmlElement("DatabaseType")]
			public int databaseType;
			[XmlElement("ConnectionString")]
			public String connectionString;
			[XmlElement("FtpUrl")]
			public String ftpUrl;
			[XmlElement("FtpUser")]
			public String ftpUser;
			[XmlElement("FtpPassword")]
			public String ftpPassword;
			[XmlElement("UseSSL")]
			public bool useSSL;
		};

		[XmlType("SystemInfo")]
		public struct SystemInfo
		{
			[XmlElement("Name")]
			public String name;	//For database match
			[XmlElement("Location")]
			public String location;
			[XmlElement("Description")]
			public String description;
			[XmlElement("Version")] //Automated by server
			public int version;
		};

		[XmlRoot("Device")]
		public struct DeviceSetting
		{
			[XmlElement("Module")]
			public String module;
			[XmlElement("Name")]
			public String name;	// For referencing device (Module can change)
			[XmlElement("Priority")]
			public int priority;
			[XmlElement("ConnectionType")]	// Settings as a class? that can be seialised
			public int connectionType;
			[XmlElement("netSettings")]
			public NetSettings netSettings;
			[XmlElement("serialSettings")]
			public SerialSettings serialSettings;
			[XmlElement("usbSettings")]
			public USBDeviceID usbSettings;
			[XmlArray("RequiredFiles")]
			public List<String> requiredFiles;
			
			[XmlIgnore]
			public iName instance;		// This allows for extendibility into the future where there might be new device interfaces
			[XmlIgnore]
			public Type type;
		};

		[XmlType("Mainline")]
		public struct Mainline
		{
			[XmlElement("Module")]
			public String module;
			[XmlElement("Name")]
			public String name;
			[XmlArray("RequiredFiles")]
			public List<String> requiredFiles;

			[XmlIgnore]
			public iModule instance;
			[XmlIgnore]
			public Type type;
		};

		//
		// Macro's defined as:
		//	DevName.FunctionName
		//	Mainline.FunctionName
		//	SystemName.Dev/Main.FunctionName
		//
		[XmlType("Macro")]
		public struct MacroDef
		{
			[XmlElement("Name")]
			public String name;

			[XmlElement("Description")]
			public String description;
			//
			// Macros should be able to define waits (either milliseconds, seconds, device ready (function unlocked) or feedback)
			//
			[XmlArray("Functions")]
			public String[] functions;
		}

		[XmlRoot("System")]
		public class ControlSystem
		{
			[XmlElement("SystemInfo")]
			public SystemInfo systemInfo;

			[XmlArray("Devices")]
			public DeviceSetting[] devices;

			[XmlArray("Mainlines")]
			public Mainline[] mainlines;

			[XmlArray("Macros")]
			public MacroDef[] macros;
		};


		#endregion


		bool serverInUse = false;   // Are we connected to a DB with a module repository?
		public ServerSettings SS;
		public ControlSystem CS;
		
		
		//
		// TODO move xml settings into a DLL (Loading unloading, creating device definitions, server def ect)
		//


		public void saveXML()
		{
			XMLSerialization.SerializeObject(CS, "system.xml");
			XMLSerialization.SerializeObject(SS, "settings.xml");
		}

		public void loadXML()
		{
			CS = XMLSerialization.DeserializeObject<ControlSystem>("system.xml");
			
			//
			// TODO:: if settings already loaded, compare them and copy over instance + connection data of non-changes
			//	Keep old settings in memory untill change is propagated, then overwrite with new settings
			//
			
			propagateSettingsChange();
		}
		
		private void propagateSettingsChange()
		{
			//
			// Thread safe event management
			//
			settingsChanged handler = newSettings;
			if (handler != null)
				handler(CS, SS);
		}

		public void loadSettings()
		{
			if (File.Exists("settings.xml"))
			{
				SS = XMLSerialization.DeserializeObject<ServerSettings>("settings.xml");
				serverInUse = true;
				
				//
				// TODO: Download settings here (Call appropriate function)
				//
				
				//
				// TODO: Load xml if server not avaliable and continue to check for a connection (every 10min ect)
				//
				loadXML();
			}
			else
			{
				serverInUse = false;
				loadXML();
			}				
		}

		public int checkXmlVersion()
		{
			//
			// Used to see if there are any changes
			//
			XmlTextReader check = new XmlTextReader("system.xml");
			String version = check.GetAttribute("Version");
			check.Close();
			return int.Parse(version);
		}

		public int checkDbVersion()
		{
			return 0;
		}


		public delegate void settingsChanged(ControlSystem controlSystem, ServerSettings serverSettings);
		public static event settingsChanged newSettings;
	}
}
