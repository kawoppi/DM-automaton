using System;

namespace DM_automaton
{
	class Program
	{
		static void Main(string[] args)
		{
			//TestDFA();
			TestDmReader("/datum/animal/hostile/retaliate/frog(/var/color)");
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
			automaton.DefineAsStartState("A");
			automaton.DefineAsFinalState("B");

			automaton = automaton.CreateDFA();

			TestWithString(automaton, "a b a", true);
			TestWithString(automaton, "b b b", false);

			TestWithString(automaton, "a", false);
		}

		static void TestDmReader(string input)
		{
			TestSplitter(new PathSplitter(), input);
			DmReader dmReader = new DmReader();
		}

		public static void TestWithString(Automaton automaton, string input, bool expectedResult)
		{
			Console.WriteLine("testing automaton:");
			Console.WriteLine(automaton);
			Console.WriteLine("with input: " + input);
			bool accepted = automaton.AcceptDFAOnly(input);
			Console.WriteLine("automaton result: " + accepted);
			Console.WriteLine("expected result: " + expectedResult);
			Console.WriteLine();
		}

		static void TestSplitter(IStringSplitter splitter, string input)
		{
			Console.WriteLine("testing splitter with input \"" + input + "\"");
			string[] outputs = splitter.Split(input);
			foreach (string output in outputs)
			{
				Console.Write(output + " ");
			}
			Console.WriteLine();
		}
	}
}

/* TODO:
 * must have:
 *		create DM definition datastructures
 *		have automaton fill DM datastructures
 *		organize a collection of DM definition into a tree structure
 *		print this tree structure
 *		read DM file line by line
 *	
 * 
 * cleanup:
 *		improve NFA -> DFA code structure
 *		differentiate between state(string) and stateset using generic type
 *		cleanup and document DFA conversion code
 *		remove unreachable states from DFA
 *		improve path splitter
 *		change field naming to use m_
 *	
 * improvements:
 *		add epsilon support
 */