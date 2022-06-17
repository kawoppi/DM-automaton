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

		public DmReader()
		{
			//create symbols with callbacks to fill datastructure
			KeywordSet baseType = new KeywordSet(OnSegmentRead, "/datum", "/atom", "/turf", "/area", "/mob", "/obj", "/client", "/list", "/world");
			StartsWith subtype = new StartsWith("/", OnSegmentRead);
			KeywordSet procModifier = new KeywordSet(OnSegmentRead, "/proc", "/verb");
			KeywordSet typeModifier = new KeywordSet(OnSegmentRead, "/gobal", "/const", "/tmp");
			AnyBetween parameters = new AnyBetween("(", ")", OnSegmentRead);
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

		public void ReadFile(string path)
		{

		}

		public void ReadLine(string line)
		{
			m_currentPath = new DmPath();
		}

		private void OnSegmentRead(string segment, bool isValid)
		{
			if (isValid)
			{
				m_currentPath.AddTypeSegment(segment);
				Console.WriteLine(m_currentPath);
			}
		}
	}
}
