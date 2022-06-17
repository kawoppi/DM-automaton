using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton.models
{
	public class DmDefinitions
	{
		private ISet<Datum> m_datums;
		private ISet<Proc> m_procs;

		public DmDefinitions()
		{
			m_datums = new HashSet<Datum>();
			m_procs = new HashSet<Proc>();
		}

		public void AddDatum(Datum datum)
		{
			m_datums.Add(datum);
		}

		public void AddProc(Proc proc)
		{
			m_procs.Add(proc);
		}

		public void Print()
		{
			//TODO improve
			Console.WriteLine("datums:");
			foreach (Datum datum in m_datums)
			{
				Console.WriteLine("\t" + datum.ToString());
			}
			Console.WriteLine("procs:");
			foreach (Proc proc in m_procs)
			{
				Console.WriteLine("\t" + proc.ToString());
			}
		}

		private void Sort()
		{
			//TODO
			//go through each datum
			//	go trhough each proc
			//		if proc matches datum path, add proc to datum, remove proc from list
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
