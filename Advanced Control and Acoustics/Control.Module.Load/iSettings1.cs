using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Control.Module.Load
{
	[XmlType("ParityType")]
	public enum ParityType { NONE, ODD, EVEN, MARK, SPACE };

	[XmlType("StopType")]
	public enum StopType { NONE, ONE, TWO, ONEPOINTFIVE };

	[XmlType("SerialSettings")]
	public struct SerialSettings
	{
		[XmlElement("BaudRate")]
		public int BaudRate;
		[XmlElement("Parity")]
		public ParityType Parity;
		[XmlElement("DataBits")]
		public int DataBits;
		[XmlElement("StopBit")]
		public StopType StopBit;
	};

	[XmlType("USBDeviceID")]
	public struct USBDeviceID
	{
		[XmlElement("vid")]
		public int vid;			// Vendor ID
		[XmlElement("pid")]
		public int pid;			// Product ID
		[XmlElement("release")]
		public int release;		// (optional)Release version
		[XmlElement("devIndex")]
		public int devIndex;		// (optional)device index - if multiple devices attached
		[XmlElement("manufacturer")]
		public String manufacturer;
		[XmlElement("product")]
		public String product;
		[XmlElement("serial")]
		public String serial;
	};

	[XmlType("NetSettings")]
	public struct NetSettings
	{
		[XmlElement("IP")]
		public String ip;
		[XmlElement("Port")]
		public int port;

		//
		// Style of connection
		//	0 == Maintain connection
		//	1 =< Wait that number of seconds for a response
		//	-1 => Wait until platform timeout occurs
		//
		[XmlElement("timeout")]		// (Mainly for IP) Response wait time (0 = maintain conecction)
		public int timeout;
	}
	
	public interface iSettings1
	{
		USBDeviceID getUSBSettings();
		SerialSettings getSerialSettings();
		NetSettings getEthernetSettings();
	}
}
