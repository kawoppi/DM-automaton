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

	/// <summary>
	/// Will split a path string into segments each starting with /.
	/// Text surrounded by parentheses will also be split.
	/// </summary>
	public class PathSplitter : IStringSplitter
	{
		public override string[] Split(string input)
		{
			//remove parameters first so they don't get split in segments
			int startingIndex = input.IndexOf('(');
			int endingIndex = input.LastIndexOf(')');
			string parameters = input.Substring(startingIndex, endingIndex - startingIndex + 1);
			input = input.Substring(0, startingIndex); //remove the parameters as they are already processed

			//now separate the path segments
			List<string> segments = new List<string>(input.Split('/', StringSplitOptions.RemoveEmptyEntries));
			for (int i = 0; i < segments.Count; i++)
			{
				segments[i] = "/" + segments[i];
			}
			segments.Add(parameters); //add the parameters back at the end
			return segments.ToArray();
			//TODO put parameters back in the right order and support multiple of them
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
