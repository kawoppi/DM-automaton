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
		private ISet<Transition<string>> transitions;
		private SortedSet<string> states;
		private SortedSet<string> startStates;
		private SortedSet<string> finalStates;
		private ISet<Symbol> symbols;
		private IStringSplitter inputSplitter;

		public Automaton(ISet<Symbol> symbols, IStringSplitter inputSplitter)
		{
			this.transitions = new HashSet<Transition<string>>();
			this.states = new SortedSet<string>();
			this.startStates = new SortedSet<string>();
			this.finalStates = new SortedSet<string>();
			this.symbols = symbols;
			this.inputSplitter = inputSplitter;
		}

		public void AddTransition(Transition<string> transition)
		{
			this.states.Add(transition.FromState);
			this.states.Add(transition.ToState);
			this.transitions.Add(transition);
		}

		public void DefineAsStartState(string state)
		{
			if (!this.states.Contains(state))
			{
				Debug.Fail("tried to define non-existant state as start state");
				return;
			}
			this.startStates.Add(state);
		}

		public void DefineAsFinalState(string state)
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
			foreach(string state in this.states)
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
		public ISet<string> GetToStates(string from, string symbol)
		{
			SortedSet<string> states = new SortedSet<string>();
			foreach (Transition<string> transition in this.transitions)
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
		public ISet<string> GetToStates(string from, Symbol symbol)
		{
			SortedSet<string> states = new SortedSet<string>();
			foreach (Transition<string> transition in this.transitions)
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
			string currentState = this.startStates.First();
			Console.WriteLine($"Accept sequence {sequence}, start at state {currentState}");

			foreach (string symbol in this.inputSplitter.Split(sequence))
			{
				ISet<string> toStates = this.GetToStates(currentState, symbol);
				if (toStates.Count != 1)
				{
					Debug.Fail("multiple to states found in DFA");
					return false;
				}
				Console.Write($"Went from state {currentState} ");
				currentState = toStates.First<string>();
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
			Automaton automaton = new Automaton(this.symbols, this.inputSplitter);
			//foutconditie aanmaken
			//maak states voor alle combinaties bestaande states
			//kijk per state, per input naar welke state het moet gaan
			//onbereikbare toestanden verwijderen
			return automaton;
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

		private static string StatesToString(ISet<string> states)
		{
			string output = "{";
			foreach (string state in states)
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
