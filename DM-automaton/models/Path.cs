using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton.models
{
	public class Path
	{
		private List<string> m_typeSegments;
		private string? m_modifier;

		public Path()
		{
			m_typeSegments = new List<string>();
		}

		public Path(List<string> typeSegments)
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

		public Path GetParentType()
		{
			List<string> parentTypeSegments = new List<string>(m_typeSegments);
			parentTypeSegments.RemoveAt(parentTypeSegments.Count - 1);
			return new Path(parentTypeSegments);
		}

		public string GetModifier()
		{
			return m_modifier;
		}
	}
}
