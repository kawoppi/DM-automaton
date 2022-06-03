using System;

namespace DM_automaton
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("hi");
			KeywordSet baseType = new KeywordSet();
			KeywordSet procModifier = new KeywordSet("/proc", "/verb");
			KeywordSet typeModifier = new KeywordSet("/gobal", "/const", "/tmp");
		}
	}
}

/* TODO:
 * design alphabet structure
 * port automaton code
 * convert NDFA to DFA automatically
 * (create automaton unit tests)
 * 
 * finish grammar definition
 * read DM file line by line
 * 
 * create DM definition datastructures
 * have automaton create DM definions
 * organize a collection of DM definitions into a tree structure
 * display this tree structure
 */