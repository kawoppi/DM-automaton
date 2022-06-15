using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton
{
	public class StateSet : IComparable<StateSet>
	{
		private ISet<string> states;

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
			return HashCode.Combine(states);
		}

		public int CompareTo(StateSet? other)
		{
			if (other == null)
			{
				return 1;
			}

			int result = 0;
			foreach (string state in this.states)
			{
				foreach (string otherState in other.states)
				{
					result = state.CompareTo(otherState);
					if (result != 0)
					{
						return result;
					}
				}
			}
			return result;
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
