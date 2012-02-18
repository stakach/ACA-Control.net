using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Control.TestConsole
{
	class XMLSerialization
	{
		//
		// Based on: http://developmenttips.blogspot.com/2009/03/serialize-and-deserialize-objects.html
		//	and http://andrewgunn.blogspot.com/2008/06/xml-serialization-in-cnet.html
		//
		public static void SerializeObject(Object obj, String filename)
		{
			//
			// TODO::Check filename exists in both files
			//
			XmlSerializer xs = new XmlSerializer(obj.GetType());
			XmlTextWriter xmlTextWriter = new XmlTextWriter(filename, Encoding.UTF8);
			xs.Serialize(xmlTextWriter, obj);
			xmlTextWriter.Flush();
			xmlTextWriter.Close();
		}

		public static T DeserializeObject<T>(string filename)
		{
			Object obj = null;
			XmlSerializer xs = new XmlSerializer(typeof(T));
			XmlTextReader xtr = new XmlTextReader(filename);
			obj = xs.Deserialize(xtr);
			return (T)obj;
		}
	}
}
