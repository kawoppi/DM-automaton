using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton
{
	public class Automaton
	{
		private ISet<Transition<StateSet>> transitions;
		private SortedSet<StateSet> states;
		private SortedSet<StateSet> startStates;
		private SortedSet<StateSet> finalStates;
		private ISet<Symbol> symbols;
		private IStringSplitter inputSplitter;

		public Automaton(ISet<Symbol> symbols, IStringSplitter inputSplitter)
		{
			this.transitions = new HashSet<Transition<StateSet>>();
			this.states = new SortedSet<StateSet>();
			this.startStates = new SortedSet<StateSet>();
			this.finalStates = new SortedSet<StateSet>();
			this.symbols = symbols;
			this.inputSplitter = inputSplitter;
		}

		public void AddTransition(Transition<StateSet> transition)
		{
			this.states.Add(transition.FromState);
			this.states.Add(transition.ToState);
			this.transitions.Add(transition);
		}

		public void AddTransition(string fromState, Symbol symbol, string toState)
		{
			this.AddTransition(new Transition<StateSet>(new StateSet(fromState), symbol, new StateSet(toState)));
		}

		public void DefineAsStartState(StateSet state)
		{
			if (!this.states.Contains(state))
			{
				Debug.Fail("tried to define non-existant state as start state");
				return;
			}
			this.startStates.Add(state);
		}

		public void DefineAsFinalState(StateSet state)
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
			foreach(StateSet state in this.states)
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
		public ISet<StateSet> GetToStates(StateSet from, string symbol)
		{
			SortedSet<StateSet> states = new SortedSet<StateSet>();
			foreach (Transition<StateSet> transition in this.transitions)
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
		public ISet<StateSet> GetToStates(StateSet from, Symbol symbol)
		{
			SortedSet<StateSet> states = new SortedSet<StateSet>();
			foreach (Transition<StateSet> transition in this.transitions)
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
			StateSet currentState = this.startStates.First();
			Console.WriteLine($"Accept sequence {sequence}, start at state {currentState}");

			foreach (string symbol in this.inputSplitter.Split(sequence))
			{
				ISet<StateSet> toStates = this.GetToStates(currentState, symbol);
				if (toStates.Count != 1)
				{
					Debug.Fail("multiple to states found in DFA");
					return false;
				}
				Console.Write($"Went from state {currentState} ");
				currentState = toStates.First<StateSet>();
				Console.WriteLine($"to state {currentState} using symbol {symbol}");
			}
			return this.finalStates.Contains(currentState);
		}

		public Automaton CreateDFA()
		{
			if(this.IsDFA())
			{
				return this;
			}
			Automaton dfa = new Automaton(this.symbols, this.inputSplitter);
			//foutconditie aanmaken
			//maak states voor alle combinaties bestaande states
			//kijk per state, per input naar welke state het moet gaan
			//onbereikbare toestanden verwijderen
			return dfa;
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

		private static string StatesToString(ISet<StateSet> states)
		{
			string output = "{";
			foreach (StateSet state in states)
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
