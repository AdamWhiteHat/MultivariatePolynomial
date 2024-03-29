﻿using System;
using System.Linq;
using System.Numerics;
using System.Globalization;
using System.Collections.Generic;

namespace ExtendedArithmetic
{
	public class Indeterminate : ICloneable<Indeterminate>, IEquatable<Indeterminate>, IEqualityComparer<Indeterminate>
	{
		public char Symbol { get; }
		public int Exponent { get; }

		internal static Indeterminate[] Empty = new Indeterminate[0];
		internal static Indeterminate[] Zero = new Indeterminate[] { new Indeterminate('X', 0) };

		private UnicodeCategory[] AllowedSymbolCategories = new UnicodeCategory[]
		{
			UnicodeCategory.LowercaseLetter,
			UnicodeCategory.UppercaseLetter,
			UnicodeCategory.ModifierLetter,
			UnicodeCategory.MathSymbol
		};

		#region Constructor & Parse

		public Indeterminate(char symbol, int exponent)
		{
			var symbolCategory = CharUnicodeInfo.GetUnicodeCategory(symbol);
			if (!AllowedSymbolCategories.Contains(symbolCategory))
			{
				throw new ArgumentException($"Parameter {nameof(symbol)} must be a letter character.");
			}
			Symbol = symbol;
			Exponent = exponent;
		}

		internal static Indeterminate Parse(string input)
		{
			int exponent = 1;

			string[] parts = input.Split(new char[] { '^' });

			if (parts[0].Length != 1) { throw new FormatException(); }
			char symbol = parts[0][0];
			if (!char.IsLetter(symbol)) { throw new FormatException(); }

			if (parts.Length == 2)
			{
				if (!parts[1].All(c => char.IsDigit(c))) { throw new FormatException(); }
				exponent = int.Parse(parts[1]);
			}

			return new Indeterminate(symbol, exponent);
		}

		#endregion

		#region Overrides and Interface implementations

		public Indeterminate Clone()
		{
			return new Indeterminate(this.Symbol, this.Exponent);
		}

		public bool Equals(Indeterminate other)
		{
			return this.Equals(this, other);
		}

		public bool Equals(Indeterminate left, Indeterminate right)
		{
			if (left == null) { return (right == null) ? true : false; }
			if (left.Exponent == 0 && right.Exponent == 0) { return true; }
			if (left.Symbol != right.Symbol) { return false; }
			if (left.Exponent != right.Exponent) { return false; }
			return true;
		}

		internal static bool AreCompatable(Indeterminate left, Indeterminate right)
		{
			if (left.Exponent == 0 || right.Exponent == 0)
			{
				return left.Exponent == right.Exponent;
			}
			return left.Symbol == right.Symbol;
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as Indeterminate);
		}

		public int GetHashCode(Indeterminate obj)
		{
			return obj.GetHashCode();
		}

		public override int GetHashCode()
		{
			if (Exponent == 0)
			{
				return new Tuple<char, int>('X', 0).GetHashCode();
			}
			return new Tuple<char, int>(Symbol, Exponent).GetHashCode();
		}

		public override string ToString()
		{
			if (Exponent == 0)
			{
				return string.Empty;
			}
			else if (Exponent == 1)
			{
				return Symbol.ToString();
			}
			else
			{
				return $"{Symbol}^{Exponent}";
			}
		}

		#endregion
	}
}
