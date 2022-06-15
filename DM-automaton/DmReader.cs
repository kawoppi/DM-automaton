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

		public DmReader()
		{
			CreateAutomata();
		}

		private void CreateAutomata()
		{
			KeywordSet baseType = new KeywordSet("/datum", "/atom", "/turf", "/area", "/mob", "/obj", "/client", "/list", "/world");
			StartsWith subtype = new StartsWith("/"); //TODO prevent this from also matching others
			KeywordSet procModifier = new KeywordSet("/proc", "/verb");
			KeywordSet typeModifier = new KeywordSet("/gobal", "/const", "/tmp");
			AnyBetween parameters = new AnyBetween("(", ")");
			subtype.SetExceptions(baseType, procModifier, typeModifier);

			ISet<Symbol> alphabet = new HashSet<Symbol>();
			alphabet.Add(baseType);
			alphabet.Add(subtype);
			alphabet.Add(procModifier);
			alphabet.Add(typeModifier);
			alphabet.Add(parameters);

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
			Console.WriteLine(m_datumAcceptor);

			//TODO proc acceptor

			//temp test//
			m_datumAcceptor = m_datumAcceptor.CreateDFA();
			Console.WriteLine(m_datumAcceptor);
			Program.TestWithString(m_datumAcceptor, "/datum/animal/hostile/retaliate/frog", true);
		}
	}
}
