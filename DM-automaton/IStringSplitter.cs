using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton
{
	abstract public class IStringSplitter
	{
		abstract public string[] Split(string input);
	}

	public class PathSplitter : IStringSplitter
	{
		public override string[] Split(string input)
		{
			throw new NotImplementedException(); //TODO implement
		}
	}

	public class SpaceSplitter : IStringSplitter
	{
		public override string[] Split(string input)
		{
			return input.Split(' ');
		}
	}
}
