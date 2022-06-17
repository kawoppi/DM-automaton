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
			DmDefinitions definitions = reader.ReadFile("demoinput.dm"); //try "demoinput.dm" or "input.dm"
			Console.WriteLine("***results before sorting procs***");
			definitions.Print();
			Console.WriteLine("***results after sorting procs***");
			definitions.SortProcs();
			definitions.Print();
			//see Log for logging options
		}

	}
}

/* TASKS:
 *		urgent:
 * 
 *		cleanup:
 *			improve NFA -> DFA code structure
 *			differentiate between a single state(string) and StateSet using generic types
 *			change field naming to use m_
 *	
 *		improvements:
 *			ignore comments after procs/datums
 *			make proc acceptor work for global procs
 *			remove unreachable states from DFA
 *			read multiple files and add to the same DmDefinitions object
 *			add epsilon support
 *			show proc overrides explicitly
 */