using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		private ISet<Symbol> symbols;
		private IStringSplitter inputSplitter;

		public Automaton(ISet<Symbol> symbols, IStringSplitter inputSplitter)
		{
			this.transitions = new SortedSet<Transition<TState>>();
			this.states = new SortedSet<TState>();
			this.startStates = new SortedSet<TState>();
			this.finalStates = new SortedSet<TState>();
			this.symbols = symbols;
			this.inputSplitter = inputSplitter;
		}

		public void AddTransition(Transition<TState> transition)
		{
			this.states.Add(transition.FromState);
			this.states.Add(transition.ToState);
			this.transitions.Add(transition);
		}

		public void DefineAsStartState(TState state)
		{
			if (!this.states.Contains(state))
			{
				Debug.Fail("tried to define non-existant state as start state");
				return;
			}
			this.startStates.Add(state);
		}

		public void DefineAsFinalState(TState state)
		{
			if (!this.states.Contains(state))
			{
				Debug.Fail("tried to define non-existant state as final state");
				return;
			}
			this.finalStates.Add(state);
		}

		public override string ToString()
		{
			string output = "(";
			output += "states: " + StatesToString(this.states) + ", \n";
			output += "alphabet: " + SymbolsToString(this.symbols) + ", \n";
			output += "start states: " + StatesToString(this.startStates) + ", \n";
			output += "final states: " + StatesToString(this.finalStates) + ")";
			return output;
		}

		private static string StatesToString(ISet<TState> states)
		{
			string output = "{";
			foreach (TState state in states)
			{
				output += state + " ";
			}
			output += "}";
			return output;
		}

		private static string SymbolsToString(ISet<Symbol> symbols)
		{
			string output = "{";
			foreach (Symbol symbol in symbols)
			{
				output += "(" + symbol + ") ";
			}
			output += "}";
			return output;
		}
	}
}
