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

/* TASKS:
 *		urgent:
 *			fix false negatives (trim input)
 *			test with real code
 *			fix path splitter false positive
 *			show overrides
 *			FIX CRASH IF NO TRANSITIONS ARE POSSIBLE
 * 
 *		cleanup:
 *			improve NFA -> DFA code structure
 *			differentiate between a single state(string) and StateSet using generic types
 *			change field naming to use m_
 *	
 *		improvements:
 *			remove unreachable states from DFA
 *			add epsilon support
 */