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
			KeywordSet baseType = new KeywordSet(new string[] { "/datum", "/atom", "/turf", "/area", "/mob", "/obj", "/client", "/list", "/world" }, OnPathSegmentRead, "basetype");
			StartsWith subtype = new StartsWith("/", OnPathSegmentRead, "subtype");
			KeywordSet procModifier = new KeywordSet(new string[] { "/proc", "/verb" }, OnModifierSegmentRead, "proc modifier");
			KeywordSet typeModifier = new KeywordSet(new string[] { "/gobal", "/const", "/tmp" }, OnModifierSegmentRead, "type modifier");
			AnyBetween parameters = new AnyBetween("(", ")", OnParametersRead, "parameters");
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

		public DmDefinitions ReadFile(string path)
		{
			Log.DMRead($"reading from file: {path}");
			DmDefinitions definitions = new DmDefinitions();
			string[] lines = System.IO.File.ReadAllLines(path);
			foreach (string line in lines)
			{
				Log.DMRead($"reading line: {line}");

				Datum datum = ReadLineForDatum(line);
				if (datum != null)
				{
					definitions.AddDatum(datum);
					Log.DMRead($"\tadded datum {datum.GetShortName()}");
					continue;
				}

				Proc proc = ReadLineForProc(line);
				if (proc != null)
				{
					definitions.AddProc(proc);
					Log.DMRead($"\tadded proc {proc.GetShortName()}");
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
