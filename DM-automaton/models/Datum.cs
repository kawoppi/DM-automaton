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
	}
}
