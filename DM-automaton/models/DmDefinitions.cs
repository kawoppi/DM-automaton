using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton.models
{
	public class DmDefinitions
	{
		private ISet<Datum> m_datums; //only contains highest parents after sorting
		private ISet<Proc> m_unsortedProcs; //only contains procs not contained in datums after sorting

		public DmDefinitions()
		{
			m_datums = new SortedSet<Datum>();
			m_unsortedProcs = new HashSet<Proc>();
		}

		public void AddDatum(Datum datum)
		{
			m_datums.Add(datum);
		}

		public void AddProc(Proc proc)
		{
			m_unsortedProcs.Add(proc);
		}

		public void Print()
		{
			if (m_datums.Count > 0) Console.WriteLine("datums:");
			foreach (Datum datum in m_datums)
			{
				Console.WriteLine("\t" + datum.GetFullName());
				foreach (Proc proc in datum.Procs)
				{
					Console.WriteLine("\t\t" + proc.GetShortName());
				}
			}
			if (m_unsortedProcs.Count > 0) Console.WriteLine("\nunsorted procs:");
			foreach (Proc proc in m_unsortedProcs)
			{
				Console.WriteLine("\t" + proc.GetFullName());
			}
			Console.WriteLine("");
		}

		/// <summary>
		/// Tries to sort each Proc into the appropriate Datum.
		/// </summary>
		public void SortProcs()
		{
			foreach (Datum datum in m_datums)
			{
				ISet<Proc> toRemove = new HashSet<Proc>();
				foreach (Proc proc in m_unsortedProcs)
				{
					if (proc.Path.GetParentType().Equals(datum.Path))
					{
						datum.AddProc(proc);
						toRemove.Add(proc);
					}
				}
				foreach (Proc proc in toRemove)
				{
					m_unsortedProcs.Remove(proc);
				}
			}
		}

	}
}
