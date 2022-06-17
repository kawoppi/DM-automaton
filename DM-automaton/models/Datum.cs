using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton.models
{
	public class Datum
	{
		public DmPath Path { get { return m_path; } }
		private DmPath m_path;

		public Datum(DmPath path)
		{
			m_path = path;
		}

		public override bool Equals(object? obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is Datum)
			{
				Datum other = (Datum)obj;
				return m_path.Equals(other.m_path);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(m_path.ToString());
		}

		public override string ToString()
		{
			return m_path.ToString();
		}
	}
}
