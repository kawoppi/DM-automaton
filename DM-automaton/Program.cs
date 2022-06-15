using System;

namespace DM_automaton
{
	class Program
	{
		static void Main(string[] args)
		{
			KeywordSet baseType = new KeywordSet();
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

			Automaton automaton = new Automaton(alphabet, null);
			automaton.AddTransition("A", anySegment, "B");
			/*Console.WriteLine(automaton);
			Console.WriteLine(automaton.IsDFA());*/

			TestDFA();
		}

		static void TestDFA()
		{
			KeywordSet a = new KeywordSet("a");
			KeywordSet b = new KeywordSet("b");

			ISet<Symbol> alphabet = new HashSet<Symbol>();
			alphabet.Add(a);
			alphabet.Add(b);

			//accept if contains 'a'
			Automaton automaton = new Automaton(alphabet, new SpaceSplitter());
			automaton.AddTransition("A", a, "B");
			//automaton.AddTransition("A", b, "A");
			//automaton.AddTransition("B", a, "B");
			//automaton.AddTransition("B", b, "B");
			automaton.DefineAsStartState(new StateSet("A"));
			automaton.DefineAsFinalState(new StateSet("B"));

			automaton = automaton.CreateDFA();

			TestWithString(automaton, "a b a", true);
			TestWithString(automaton, "b b b", false);

			TestWithString(automaton, "a", false);


		}

		static void TestWithString(Automaton automaton, string input, bool expectedResult)
		{
			Console.WriteLine("testing automaton:");
			Console.WriteLine(automaton);
			Console.WriteLine("with input: " + input);
			bool accepted = automaton.AcceptDFAOnly(input);
			Console.WriteLine("automaton result: " + accepted);
			Console.WriteLine("expected result: " + expectedResult);
			Console.WriteLine();
		}
	}
}

/* TODO:
 * improve NFA -> DFA code structure
 * differentiate between state and stateset
 * differentiate between NFA and DFA
 * cleanup and document DFA conversion code
 * 
 * finish grammar definition
 * read DM file line by line
 * implement path splitter
 * 
 * create DM definition datastructures
 * have automaton create DM definions
 * organize a collection of DM definitions into a tree structure
 * display this tree structure
 */