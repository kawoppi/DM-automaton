using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton
{
	public class Automaton<TState>
		where TState : IComparable<TState>
	{
		private ISet<Transition<TState>> transitions;
		private SortedSet<TState> states;
		private SortedSet<TState> startStates;
		private SortedSet<TState> finalStates;
		private SortedSet<Symbol> symbols;
		private IStringSplitter inputSplitter;

		public Automaton(SortedSet<Symbol> symbols, IStringSplitter inputSplitter)
		{
			this.transitions = new SortedSet<Transition<TState>>();
			this.states = new SortedSet<TState>();
			this.startStates = new SortedSet<TState>();
			this.finalStates = new SortedSet<TState>();
			this.symbols = symbols;
			this.inputSplitter = inputSplitter;
		}
	}
}
