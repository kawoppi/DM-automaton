using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_automaton.Automata
{
	/// <summary>
	/// Represents a symbol to be used in the alphabet of an automaton.
	/// </summary>
	abstract public class Symbol
	{
		public delegate void OnValidate(string validatedInput, bool isValid);
		private OnValidate? onValidateCallback;
		private Symbol[] exceptions;

		public Symbol(OnValidate? onValidateCallback = null)
		{
			this.onValidateCallback = onValidateCallback;
			this.exceptions = new Symbol[0];
		}

		public void SetExceptions(params Symbol[] exceptions)
		{
			this.exceptions = exceptions;
		}

		/// <summary>
		/// Checks if the input conforms to the specification of the symbol and calls the OnValidate callback.
		/// Returns false if the input conforms to any of the set exception symbols.
		/// </summary>
		public bool Validate(string input)
		{
			foreach (Symbol exception in this.exceptions)
			{
				if (exception.Validate(input))
				{
					return false;
				}
			}
			bool isValid = this.ValidateInput(input);
			if (this.onValidateCallback != null)
			{
				this.onValidateCallback(input, isValid);
			}
			return isValid;
		}

		abstract protected bool ValidateInput(string input);
	}



	/// <summary>
	/// Can check a string to see if it matches any of the keywords it has.
	/// </summary>
	public class KeywordSet : Symbol
	{
		private ISet<string> keywords;

		public KeywordSet(OnValidate? onValidateCallback, params string[] keywords) : base(onValidateCallback)
		{
			this.keywords = new HashSet<string>(keywords);
		}

		public KeywordSet(params string[] keywords) : this(null, keywords) { }

		protected override bool ValidateInput(string input)
		{
			return this.keywords.Contains(input);
		}

		public override string ToString()
		{
			string output = "matches: ";
			foreach (string keyword in this.keywords)
			{
				output += "\"" + keyword + "\" ";
			}
			return output;
		}
	}



	/// <summary>
	/// Can check a string to see if it starts with a specified string.
	/// </summary>
	public class StartsWith : Symbol
	{
		private string start;

		public StartsWith(string start, OnValidate? onValidateCallback = null) : base(onValidateCallback)
		{
			this.start = start;
		}

		protected override bool ValidateInput(string input)
		{
			return input.StartsWith(this.start);
		}

		public override string ToString()
		{
			return "starts with: \"" + this.start + "\"";
		}
	}



	/// <summary>
	/// Can check a string to see if it starts and ends with the specified strings.
	/// </summary>
	public class AnyBetween : Symbol
	{
		private string start;
		private string end;

		public AnyBetween(string start, string end, OnValidate? onValidateCallback = null) : base(onValidateCallback)
		{
			this.start = start;
			this.end = end;
		}

		protected override bool ValidateInput(string input)
		{
			return input.StartsWith(this.start) && input.EndsWith(this.end);
		}

		public override string ToString()
		{
			return "starts with: \"" + this.start + "\" ends with: \"" + this.end + "\"";
		}
	}
}