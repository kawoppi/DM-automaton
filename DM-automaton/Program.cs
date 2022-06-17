using DM_automaton.Automata;
using DM_automaton.models;
using System;

namespace DM_automaton
{
	class Program
	{
		static void Main(string[] args)
		{
			DmReader reader = new DmReader();
			DmDefinitions definitions = reader.ReadFile("input.dm");
			Console.WriteLine("***results before sorting procs***");
			definitions.Print();
			Console.WriteLine("***results after sorting procs***");
			definitions.SortProcs();
			definitions.Print();
		}

	}
}

/* TODO:
 * must have:
 *		fix false negatives (trim input)
 *		test with real code
 * 
 * cleanup:
 *		improve NFA -> DFA code structure
 *		differentiate between state(string) and stateset using generic type
 *		cleanup and document DFA conversion code
 *		remove unreachable states from DFA
 *		improve path splitter
 *		change field naming to use m_
 *	
 * improvements:
 *		add epsilon support
 */