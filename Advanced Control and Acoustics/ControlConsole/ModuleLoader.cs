using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

using Control.Module.Load;


namespace Control.TestConsole
{
	class ModuleLoader
	{
		List<Type> typeList;		// Loaded types
		List<String> moduleList;	// Module types
		String baseFolder;
		
		//
		// Module list change event.
		//
		public ModuleLoader(String exeFolder)
		{
			baseFolder = exeFolder;
			typeList = new List<Type>();
			moduleList = new List<string>();
			SettingsProvider.newSettings += new SettingsProvider.settingsChanged(moduleListChanged);
		}

		public void moduleListChanged(SettingsProvider.ControlSystem CC, SettingsProvider.ServerSettings SS)
		{
			for(int i = 0; i < CC.devices.Length; i++)
			{
				//
				// Device is already setup (a change in file list should call disconnect on the module)
				//
				if(CC.devices[i].instance != null)
					continue;

				try
				{
					loadModule(ref CC.devices[i].module, CC.devices[i].requiredFiles, SS, out CC.devices[i].instance, out CC.devices[i].type);
				}
				catch (Exception ex)
				{
					Program.LogError(ex);
				}
			}

			for (int i = 0; i < CC.mainlines.Length; i++)
			{
				//
				// Device is already setup (a change in file list should call disconnect on the module)
				//
				if (CC.mainlines[i].instance != null)
					continue;

				try
				{
					loadModule(ref CC.mainlines[i].module, CC.mainlines[i].requiredFiles, SS, out CC.mainlines[i].instance, out CC.mainlines[i].type);
				}
				catch (Exception ex)
				{
					Program.LogError(ex);
				}
			}
		}


		protected void loadModule(ref String module, List<String> requiredFiles, SettingsProvider.ServerSettings SS, out iModule instance, out Type type)
		{
			type = null;
			instance = null;
			
		
			//
			// We have to locate the device type, download from server if not avaliable
			//
			if (!checkFile(ref module, SS, ".dll"))
				throw new FileNotFoundException("Module could not be loaded", module);


			//
			// We've already loaded this device type however this instance has not been created
			//
			int index;
			index = moduleList.IndexOf(module);
			if (index != -1)
			{
				type = typeList.ElementAt(index);
				instance = (iModule)Activator.CreateInstance(type);
			}
			
			//
			// Device type not loaded
			//
			else
			{
				String temp = Path.Combine(Path.GetDirectoryName(baseFolder), module);	//Full path here (not relative as in settings files)
				Assembly assembly = Assembly.LoadFrom(temp);

				foreach (Type mtype in assembly.GetTypes())
				{
					if (!mtype.IsClass || mtype.IsNotPublic)
						continue;
					
					Type[] interfaces = mtype.GetInterfaces();
					if (((IList<Type>)interfaces).Contains(typeof(iModule)))
					{
						instance = (iModule)Activator.CreateInstance(mtype);
						type = mtype;
						typeList.Add(mtype);
						moduleList.Add(module);
						break;
					}
				}
			}

			if (instance != null)
			{
				//
				// Check if there are any related files, if they exist and if not download them from the server
				//
				for (int j = 0; j < requiredFiles.Count; j++)
				{
					String file = requiredFiles.ElementAt(j);
					if (checkFile(ref file, SS))
						requiredFiles[j] = file;
					else
					{
						Program.LogError(new FileNotFoundException("Required file not found", requiredFiles.ElementAt(j)));
						requiredFiles.RemoveAt(j);
						j--;
					}
				}

				//
				// Initialise module
				//
				instance.relatedFiles(requiredFiles);
			}
		}


		//
		// Check if file exists, download if it doesn't and return true if avaliable, false if not
		//
		protected bool checkFile(ref String file, SettingsProvider.ServerSettings SS)
		{
			if(!File.Exists(file))
			{
				// TODO:: SS.ftpUrl; SS.ftpUser; SS.ftpPassword (encrypted based on username)
				//	Download from server
				//	Create file structure locally

				return false;
			}

			return true;
		}
		protected bool checkFile(ref String file, SettingsProvider.ServerSettings SS, String extention)
		{
			if(!file.EndsWith(extention))
				file = file + extention;
			return checkFile(ref file, SS);
		}
	}
}


