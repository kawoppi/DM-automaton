using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton
{
	public class Transition<TState, TSymbol> : IComparable<Transition<TState, TSymbol>>
		where TState : IComparable<TState>
	{
		private TState fromState;
		private TSymbol symbol; //TODO use ISymbol
		private TState toState;

		public Transition(TState from, TSymbol symbol, TState to)
		{
			this.fromState = from;
			this.symbol = symbol;
			this.toState = to;
		}

		public int CompareTo(Transition<TState, TSymbol>? other)
		{
			throw new NotImplementedException();
		}
	}
}
