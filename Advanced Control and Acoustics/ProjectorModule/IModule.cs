using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectorModule
{
	public abstract class IModule
	{
		//
		// Module core interaction
		//
		public enum StatusType { BOOL, INT, DECIMAL, STRING, UNDEFINED };
		public enum Feedback { SUCCESS, WAITING, BUSY_FAIL, UNKNOWN_FAIL, UNDEFINED };

		public struct Status
		{
			public String sStat;
			public int iStat;
			public Decimal dStat;

			public StatusType statusType;
		};

		public abstract Feedback doFunction(String functionName);
		public abstract Status getStatus(String statusName);

		public delegate void statusChangeHandler(String statusName);
		public event statusChangeHandler StatusChange;

		public bool statusIsRequired()
		{
			return StatusChange != null;
		}

		public void doStatusChange(String statusName)
		{
			StatusChange(statusName);
		}


		//
		// Module communications (XML and DB connection settings [IP Port, Serial Speed])
		//
		public abstract void inputHandler(byte[] input);

		public delegate void outputHandler(byte[] output);
		public event outputHandler Output;

		public bool outputIsDefined()
		{
			return Output != null;
		}

		public void doOutput(byte[] output)
		{
			//
			// overload this - to let people define their encoding filter
			//
			Output(output);
		}
	}
}
