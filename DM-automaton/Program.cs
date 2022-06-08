﻿using System;

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

			Automaton<String> automaton = new Automaton<string>(alphabet, null);
			automaton.AddTransition(new Transition<string>("A", anySegment, "B"));
			Console.WriteLine(automaton);
		}
	}
}

/* TODO:
 * implement path splitter
 * port automaton code
 * convert NDFA to DFA automatically
 * (create automaton unit tests)
 * 
 * finish grammar definition
 * read DM file line by line
 * 
 * create DM definition datastructures
 * have automaton create DM definions
 * organize a collection of DM definitions into a tree structure
 * display this tree structure
 */