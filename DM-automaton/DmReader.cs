using DM_automaton.Automata;
using DM_automaton.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton
{
	public class DmReader
	{
		private Automaton m_datumAcceptor;
		private Automaton m_procAcceptor;

		private DmPath m_currentPath;
		private string m_currentParameters;

		public DmReader()
		{
			//create symbols with callbacks to fill datastructure
			KeywordSet baseType = new KeywordSet(OnPathSegmentRead, "/datum", "/atom", "/turf", "/area", "/mob", "/obj", "/client", "/list", "/world");
			StartsWith subtype = new StartsWith("/", OnPathSegmentRead);
			KeywordSet procModifier = new KeywordSet(OnModifierSegmentRead, "/proc", "/verb");
			KeywordSet typeModifier = new KeywordSet(OnModifierSegmentRead, "/gobal", "/const", "/tmp");
			AnyBetween parameters = new AnyBetween("(", ")", OnParametersRead);
			subtype.SetExceptions(baseType, procModifier, typeModifier); //prevents input from matching two symbols

			ISet<Symbol> alphabet = new HashSet<Symbol>(new Symbol[] { baseType, subtype, procModifier, typeModifier, parameters });
			IStringSplitter splitter = new PathSplitter();
			m_datumAcceptor = new Automaton(alphabet, splitter);
			m_procAcceptor = new Automaton(alphabet, splitter);

			//datum acceptor
			m_datumAcceptor.AddTransition("A", baseType, "B");
			m_datumAcceptor.AddTransition("B", subtype, "B");
			m_datumAcceptor.AddTransition("B", typeModifier, "C");
			m_datumAcceptor.AddTransition("B", subtype, "D");
			m_datumAcceptor.AddTransition("C", subtype, "D");
			m_datumAcceptor.DefineAsStartState("A");
			m_datumAcceptor.DefineAsFinalState("D");
			m_datumAcceptor = m_datumAcceptor.CreateDFA();

			//proc acceptor
			m_procAcceptor.AddTransition("A", baseType, "B");
			m_procAcceptor.AddTransition("B", subtype, "B");
			m_procAcceptor.AddTransition("B", procModifier, "C");
			m_procAcceptor.AddTransition("B", subtype, "D");
			m_procAcceptor.AddTransition("C", subtype, "D");
			m_procAcceptor.AddTransition("D", parameters, "E");
			m_procAcceptor.DefineAsStartState("A");
			m_procAcceptor.DefineAsFinalState("E");
			m_procAcceptor = m_procAcceptor.CreateDFA();
			//TODO make this work for global procs
		}

		public void Test() //
		{
			ReadFile("input.dm").Print();
		}

		public DmDefinitions ReadFile(string path)
		{
			//TODO log which file it is
			DmDefinitions definitions = new DmDefinitions();
			string[] lines = System.IO.File.ReadAllLines(path);
			foreach (string line in lines)
			{
				Datum datum = ReadLineForDatum(line);
				if (datum != null)
				{
					definitions.AddDatum(datum);
					continue;
				}

				Proc proc = ReadLineForProc(line);
				if (proc != null)
				{
					definitions.AddProc(proc);
					continue;
				}
			}
			return definitions;
		}

		public Datum? ReadLineForDatum(string line)
		{
			m_currentPath = new DmPath();
			m_currentParameters = null;
			if (m_datumAcceptor.AcceptDFAOnly(line))
			{
				return new Datum(m_currentPath);
			}
			return null;
		}

		public Proc? ReadLineForProc(string line)
		{
			m_currentPath = new DmPath();
			m_currentParameters = null;
			if (m_procAcceptor.AcceptDFAOnly(line))
			{
				return new Proc(m_currentPath, m_currentParameters);
			}
			return null;
		}

		private void OnPathSegmentRead(string segment, bool isValid)
		{
			if (isValid)
			{
				m_currentPath.AddTypeSegment(segment);
			}
		}

		private void OnModifierSegmentRead(string modifier, bool isValid)
		{
			if (isValid)
			{
				m_currentPath.SetModifier(modifier);
			}
		}

		private void OnParametersRead(string parameters, bool isValid)
		{
			if (isValid)
			{
				m_currentParameters = parameters;
			}
		}
	}
}
