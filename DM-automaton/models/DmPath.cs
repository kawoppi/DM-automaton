using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton.models
{
	public class DmPath
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

		public void AddModifier(string modifier)
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
