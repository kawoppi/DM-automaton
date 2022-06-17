using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton.models
{
	public class Proc
	{
		public DmPath Path { get { return m_path; } }
		private DmPath m_path;

		public string Parameters { get { return m_parameters; } }
		private string m_parameters;

		public Proc(DmPath path, string parameters)
		{
			m_path = path;
			m_parameters = parameters;
		}
	}
}
