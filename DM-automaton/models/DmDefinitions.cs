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
			m_datums = new HashSet<Datum>();
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
			Sort();//
			Console.WriteLine("datums:");
			foreach (Datum datum in m_datums) //TODO sort datums in order or parents
			{
				Console.WriteLine("\t" + datum.ToString());
				foreach (Proc proc in datum.Procs)
				{
					Console.WriteLine("\t\t" + proc.ToString()); //TODO print short version
				}
			}
			Console.WriteLine("unsorted procs:");
			foreach (Proc proc in m_unsortedProcs)
			{
				Console.WriteLine("\t" + proc.ToString());
			}
		}

		/// <summary>
		/// Puts Procs under the appropriate Datum if it exists.
		/// </summary>
		private void Sort()
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

		/// <summary>
		/// Creates all implied parent datums from existing procs and datums if they do not exist already.
		/// </summary>
		private void GenerateParents()
		{
			//TODO
		}
	}
}
