using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton
{
	public class Automaton<TState, TSymbol>
		where TState : IComparable<TState>
	{
		private ISet<Transition<TState, TSymbol>> transitions;
		private SortedSet<TState> states;
		private SortedSet<TState> startStates;
		private SortedSet<TState> finalStates;
		private SortedSet<TSymbol> symbols;

		public Automaton(SortedSet<TSymbol> symbols)
		{
			this.transitions = new SortedSet<Transition<TState, TSymbol>>();
			this.states = new SortedSet<TState>();
			this.startStates = new SortedSet<TState>();
			this.finalStates = new SortedSet<TState>();
			this.symbols = symbols;
		}
	}
}
