using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton.models
{
	public class DmPath : IComparable<DmPath>
	{
		private List<string> m_typeSegments;
		private string? m_modifier;

		public DmPath()
		{
			m_typeSegments = new List<string>();
		}

		private DmPath(List<string> typeSegments)
		{
			m_typeSegments = typeSegments;
		}

		public void AddTypeSegment(string typeSegment)
		{
			m_typeSegments.Add(typeSegment);
		}

		public void SetModifier(string modifier)
		{
			m_modifier = modifier;
		}

		public string GetName()
		{
			return m_typeSegments.Last();
		}

		public DmPath GetParentType()
		{
			List<string> parentTypeSegments = new List<string>(m_typeSegments);
			parentTypeSegments.RemoveAt(parentTypeSegments.Count - 1);
			return new DmPath(parentTypeSegments);
		}

		public string? GetModifier()
		{
			return m_modifier;
		}

		public override bool Equals(object? obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is DmPath)
			{
				DmPath other = (DmPath)obj;
				return ToString().Equals(other.ToString());
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(ToString());
		}

		public int CompareTo(DmPath? other)
		{
			if (other == null) return 1;
			//if (this.m_typeSegments.Count != other.m_typeSegments.Count) return this.m_typeSegments.Count - other.m_typeSegments.Count;
			//if (this.m_modifier != null && other.m_modifier == null) return 1;
			//if (this.m_modifier == null && other.m_modifier != null) return -1;
			return this.ToString().CompareTo(other.ToString());
		}

		public override string ToString()
		{
			string output = "";
			for (int i = 0; i < m_typeSegments.Count; i++)
			{
				if (i == m_typeSegments.Count - 1) //modifier must be the first to last segment
				{
					if (m_modifier != null)
					{
						output += m_modifier;
					}
				}
				output += m_typeSegments[i];
			}
			return output;
		}
	}
}
