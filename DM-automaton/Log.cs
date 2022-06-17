using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton
{
	public static class Log
	{
		private const bool logAccept = true;
		private const bool logDFACreation = true;
		private const bool logDMRead = true;

		public static void Accept(string message)
		{
			if (logAccept)
			{
				Console.WriteLine("accept: \t" + message);
			}
		}

		public static void DFACreation(string message)
		{
			if (logDFACreation)
			{
				Console.WriteLine("DFA Creation: \t" + message);
			}
		}

		public static void DMRead(string message)
		{
			if (logDMRead)
			{
				Console.WriteLine("DM reading: \t" + message);
			}
		}
	}
}
