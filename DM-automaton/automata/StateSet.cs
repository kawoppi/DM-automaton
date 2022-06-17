using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton.Automata
{
	public class StateSet : IComparable<StateSet>
	{
		public SortedSet<string> States { get { return this.states; } }
		private SortedSet<string> states;

		public StateSet(SortedSet<string> states)
		{
			this.states = states;
		}

		public StateSet(params string[] states)
		{
			this.states = new SortedSet<string>();
			foreach (string state in states)
			{
				this.states.Add(state);
			}
		}

		public override bool Equals(object? obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is StateSet)
			{
				StateSet other = (StateSet)obj;
				return this.states.SetEquals(other.states);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(states.ToArray());
		}

		public int CompareTo(StateSet? other)
		{
			if (other == null)
			{
				return 1;
			}
			if (this.states.Count != other.states.Count)
			{
				return this.states.Count - other.states.Count;
			}

			for(int i = 0; i < this.states.Count; i++)
			{
				int result = this.states.ElementAt(i).CompareTo(other.states.ElementAt(i));
				if (result != 0)
				{
					return result;
				}
			}
			return 0;
		}

		public override string ToString()
		{
			string output = "(";
			foreach (string state in this.states)
			{
				output += state;
			}
			output += ")";
			return output;
		}
	}
}
