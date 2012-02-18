using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Control.Utility.Hex
{
	public class HexEncoding
	{
		/// <summary> Convert a string of hex digits (ex: E4 CA B2) to a byte array. </summary>
		/// <param name="s"> The string containing the hex digits in any style ie: "0xAB1234" or "ABH 12H" ect. </param>
		/// <returns> Returns an array of bytes. </returns>
		public static byte[] GetBytes(string s)
		{
			s = Regex.Replace(s, "(0x|[^0-9A-Fa-f])*", ""); //Replaces 0x and all other non hex characters with nothing

			byte[] buffer = new byte[s.Length / 2];
			for (int i = 0; i < s.Length; i += 2)
				buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
			return buffer;
		}

		/// <summary> Converts an array of bytes into a formatted string of hex digits (ex: E4CAB2)</summary>
		/// <param name="data"> The array of bytes to be translated into a string of hex digits. </param>
		/// <returns> Returns a well formatted string of hex digits without spacing. </returns>
		public static string GetString(byte[] data)
		{
			StringBuilder sb = new StringBuilder(data.Length * 3);
			foreach (byte b in data)
				sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
			return sb.ToString().ToUpper();
		}

		/// <summary>
		/// Converts 1 or 2 character string into equivalant byte value
		/// </summary>
		/// <param name="hex">1 or 2 character string</param>
		/// <returns>The byte or 0 if invalid string</returns>
		public static byte HexToByte(string hex)
		{
			if (hex.Length > 2 || hex.Length <= 0)
				return (byte)0;
			byte newByte = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
			return newByte;
		}
	}
}
