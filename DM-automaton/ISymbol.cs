using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton
{
	abstract public class ISymbol
	{
		abstract public bool Validate(string input);

		//TODO add callback for filling in datastructure when validated
	}

	/// <summary>
	/// Can check a string to see if it matches any of the keywords it has.
	/// </summary>
	public class KeywordSet : ISymbol
	{
		private ISet<string> keywords;

		public KeywordSet(params string[] keywords)
		{
			this.keywords = new HashSet<string>(keywords);
		}

		public override bool Validate(string input)
		{
			return this.keywords.Contains(input);
		}
	}

	/// <summary>
	/// Can check a string to see if it starts with a specified string.
	/// </summary>
	public class StartsWith : ISymbol
	{
		private string start;

		public StartsWith(string start)
		{
			this.start = start;
		}

		public override bool Validate(string input)
		{
			return input.StartsWith(this.start);
		}
	}

	/// <summary>
	/// Can check a string to see if it starts and ends with the specified strings.
	/// </summary>
	public class AnyBetween : ISymbol
	{
		private string start;
		private string end;

		public AnyBetween(string start, string end)
		{
			this.start = start;
			this.end = end;
		}

		public override bool Validate(string input)
		{
			return input.StartsWith(this.start) && input.EndsWith(this.end);
		}
	}
}
