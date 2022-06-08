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



		public bool IsDFA()
		{
			foreach(TState state in this.states)
			{
				foreach (Symbol symbol in this.symbols)
				{
					if (this.GetToStates(state, symbol).Count != 1)
					{
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Return the set of states can be reached from a given state when a given symbol is received.
		/// </summary>
		public ISet<TState> GetToStates(TState from, string symbol)
		{
			SortedSet<TState> states = new SortedSet<TState>();
			foreach (Transition<TState> transition in this.transitions)
			{
				if (transition.FromState.Equals(from))
				{
					if (transition.Symbol.Validate(symbol))
					{
						states.Add(transition.ToState);
					}
				}
			}
			return states;
		}

		/// <summary>
		/// Return the set of states can be reached from a given state when a given symbol is received.
		/// </summary>
		public ISet<TState> GetToStates(TState from, Symbol symbol)
		{
			SortedSet<TState> states = new SortedSet<TState>();
			foreach (Transition<TState> transition in this.transitions)
			{
				if (transition.FromState.Equals(from))
				{
					if (transition.Symbol.Equals(symbol))
					{
						states.Add(transition.ToState);
					}
				}
			}
			return states;
		}

		public bool AcceptDFAOnly(string sequence)
		{
			TState currentState = this.startStates.First();
			Console.WriteLine($"Accept sequence {sequence}, start at state {currentState}");

			foreach (string symbol in this.inputSplitter.Split(sequence))
			{
				ISet<TState> toStates = this.GetToStates(currentState, symbol);
				if (toStates.Count != 1)
				{
					Debug.Fail("multiple to states found in DFA");
					return false;
				}
				Console.Write($"Went from state {currentState} ");
				currentState = toStates.First<TState>();
				Console.WriteLine($"to state {currentState} using symbol {symbol}");
			}
			return this.finalStates.Contains(currentState);
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
