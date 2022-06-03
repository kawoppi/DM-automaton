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
}
