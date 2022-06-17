using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton.Automata
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
			List<string> output = new List<string>();
			int segmentIndex = -1; //first index of the segment currently being read
			int parameterIndex = -1; //first index of the parameters currently being read
			int startingIndex = -1; //first index of the first separated item
			int endingIndex = -1; //last index of the last separated item
			for (int i = 0; i < input.Length; i++)
			{
				
				switch (input[i])
				{
					case '/':
						if (parameterIndex == -1) //only if not already looking for parameters
							if (segmentIndex >= 0) //if already reading a segment
							{
								output.Add(input.Substring(segmentIndex, i - segmentIndex)); //completed a segment
								if (startingIndex == -1) startingIndex = segmentIndex; //only set the very first time an output is split
								endingIndex = i; //set at the end of every split
							}
						segmentIndex = i;

						break;
					case '(':
						if (segmentIndex >= 0) //if already reading a segment
						{
							output.Add(input.Substring(segmentIndex, i - segmentIndex)); //completed a segment
							if (startingIndex == -1) startingIndex = segmentIndex; //only set the very first time an output is split
							endingIndex = i; //set at the end of every split
							segmentIndex = -1;
						}
						if (parameterIndex == -1) parameterIndex = i; //only if not already looking for parameters

						break;
					case ')':
						if (parameterIndex >= 0)
						{
							output.Add(input.Substring(parameterIndex, i - parameterIndex + 1));
							if (startingIndex == -1) startingIndex = parameterIndex; //only set the very first time an output is split
							endingIndex = i + 1; //set at the end of every split
							parameterIndex = -1;
						}
						break;
				}
			}

			//add whatever hasn't been separated yet to the start and end
			string start = input.Substring(0, startingIndex);
			string end = input.Substring(endingIndex);
			if (start.Count() > 0) output.Insert(0, start);
			if (end.Count() > 0) output.Add(end);

			return output.ToArray();
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
