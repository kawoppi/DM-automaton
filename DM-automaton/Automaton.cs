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

		private void AddTransition(Transition<StateSet> transition)
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
			foreach (StateSet state in this.states)
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
				currentState = toStates.First();
				Console.WriteLine($"to state {currentState} using symbol {symbol}");
			}
			return this.finalStates.Contains(currentState);
		}

		/// <summary>
		/// Creates a DFA from an Automaton. Does not support the use of epsilon.
		/// </summary>
		public Automaton CreateDFA()
		{
			if (this.IsDFA())
			{
				Console.WriteLine("automaton is already a DFA!");
				return this;
			}
			Automaton dfa = new Automaton(this.symbols, this.inputSplitter);

			//create the empty state from which you cannot escape
			StateSet emptySet = new StateSet();
			foreach (Symbol symbol in dfa.symbols)
			{
				dfa.AddTransition(new Transition<StateSet>(emptySet, symbol, emptySet));
			}

			//create a new state for each combination of states
			SortedSet<StateSet> newStates = new SortedSet<StateSet>();
			foreach (StateSet state1 in this.states)
			{
				foreach (StateSet state2 in this.states)
				{
					string[] combinedStates = Enumerable.ToArray(state1.States.Union(state2.States));
					newStates.Add(new StateSet(combinedStates));
				}
			}
			Console.WriteLine("new states: " + StatesToString(newStates));//

			//give each new state a transition for each symbol
			foreach (StateSet state in newStates)
			{
				//get tostates of each substate, turn them into a single state to transition to
				//if there are none, the tostate will be the empty set
				foreach (Symbol symbol in this.symbols)
				{
					StateSet toState = GetToStateFromSubstates(state, symbol);
					Console.WriteLine(state + " --(" + symbol + ")-> " + toState);
					dfa.AddTransition(new Transition<StateSet>(state, symbol, toState));
				}
			}

			//any new state containing an original final state should also become an final state
			foreach (StateSet state in dfa.states)
			{
				foreach (StateSet finalState in this.finalStates)
				{
					if (state.States.Contains(finalState.States.First()))
					{
						dfa.finalStates.Add(state);
					}
				}
			}
			foreach (StateSet state in this.startStates)
			{
				dfa.DefineAsStartState(state);
			}

			Console.WriteLine(dfa);
			return dfa;
		}

		/// <summary>
		/// Return the set of states can be reached from a given state when a given symbol is received.
		/// </summary>
		private ISet<StateSet> GetToStates(StateSet from, string symbol)
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
		private ISet<StateSet> GetToStates(StateSet from, Symbol symbol)
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

		private StateSet GetToStateFromSubstates(StateSet from, Symbol symbol)
		{
			SortedSet<string> toStates = new SortedSet<string>(); //for {AB}
			foreach (string subState in from.States) //check each substate in the set
			{
				ISet<StateSet> subToStates = this.GetToStates(new StateSet(subState), symbol); //for {A}
				foreach (StateSet subToState in subToStates)
				{
					foreach (string state in subToState.States)
					{
						toStates.Add(state);
					}
				}
			}
			return new StateSet(toStates);
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
