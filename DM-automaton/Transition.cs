using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton
{
	public class Transition<TState> : IComparable<Transition<TState>>
		where TState : IComparable<TState>
	{
		private TState fromState;
		private Symbol symbol;
		private TState toState;

		public Transition(TState from, Symbol symbol, TState to)
		{
			this.fromState = from;
			this.symbol = symbol;
			this.toState = to;
		}

		public int CompareTo(Transition<TState>? other)
		{
			throw new NotImplementedException();
		}
	}
}
