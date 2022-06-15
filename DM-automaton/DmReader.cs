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
			StartsWith anySegment = new StartsWith("/");
			KeywordSet procModifier = new KeywordSet("/proc", "/verb");
			KeywordSet typeModifier = new KeywordSet("/gobal", "/const", "/tmp");
			AnyBetween parameters = new AnyBetween("(", ")");

			ISet<Symbol> alphabet = new HashSet<Symbol>();
			alphabet.Add(baseType);
			alphabet.Add(anySegment);
			alphabet.Add(procModifier);
			alphabet.Add(typeModifier);
			alphabet.Add(parameters);
			//TODO seperate alphabets?

			m_datumAcceptor = new Automaton(alphabet, null);
			m_procAcceptor = new Automaton(alphabet, null);
		}
	}
}
