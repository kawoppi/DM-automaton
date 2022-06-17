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
			KeywordSet procModifier = new KeywordSet(OnPathSegmentRead, "/proc", "/verb");
			KeywordSet typeModifier = new KeywordSet(OnPathSegmentRead, "/gobal", "/const", "/tmp");
			AnyBetween parameters = new AnyBetween("(", ")", OnParametersRead);
			subtype.SetExceptions(baseType, procModifier, typeModifier); //prevent input from matching two symbols

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
			m_procAcceptor.AddTransition("D", parameters, "E");
			m_procAcceptor.DefineAsStartState("A");
			m_procAcceptor.DefineAsFinalState("E");
			m_procAcceptor = m_procAcceptor.CreateDFA();
			//TODO make this work for global procs
		}

		public void Test()
		{
			m_currentPath = new DmPath();
			//temp test//
			Console.WriteLine(m_procAcceptor);

			Console.WriteLine(m_procAcceptor);
			Program.TestWithString(m_procAcceptor, "/datum/animal/hostile/retaliate/frog(/var/color)", true);
		}

		public void ReadFile(string path) //TODO return list of procs and datums
		{

		}

		public void ReadLineForDatum(string line) //TODO return datum
		{
			m_currentPath = new DmPath();
			m_currentParameters = null;
			if (m_datumAcceptor.AcceptDFAOnly(line))
			{
				//return new datum
			}
			//return null
		}

		public void ReadLineForProc(string line) //TODO return proc
		{
			m_currentPath = new DmPath();
			m_currentParameters = null;
			if (m_procAcceptor.AcceptDFAOnly(line))
			{
				//return new proc
			}
			//return null
		}

		private void OnPathSegmentRead(string segment, bool isValid)
		{
			if (isValid)
			{
				m_currentPath.AddTypeSegment(segment);
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
