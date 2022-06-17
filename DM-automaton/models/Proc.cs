﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton.models
{
	public class Proc : IComparable<Proc>
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

		public override bool Equals(object? obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is Proc)
			{
				Proc other = (Proc)obj;
				return m_path.Equals(other.m_path) && m_parameters.Equals(other.m_parameters);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(m_path.ToString(), m_parameters);
		}

		public int CompareTo(Proc? other)
		{
			if (other == null) return 1;
			//TODO sort on parameters  too
			return this.m_path.CompareTo(other.m_path);
		}

		public override string ToString()
		{
			return m_path.ToString() + m_parameters;
		}
	}
}
