using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public class Indeterminate : ICloneable<Indeterminate>, IEquatable<Indeterminate>, IEqualityComparer<Indeterminate>
	{
		public char Symbol { get; }
		public int Exponent { get; }

		private BigInteger? IndeterminateValue { get; set; }

		#region Constructor & Parse

		public Indeterminate(char symbol, int exponent)
		{
			Symbol = symbol;
			Exponent = exponent;
			IndeterminateValue = null;
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

		public bool Equals(Indeterminate x, Indeterminate y)
		{
			if (x == null) { return (y == null) ? true : false; }
			if (x.Symbol != y.Symbol) { return false; }
			if (x.Exponent != y.Exponent) { return false; }
			return true;
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
			return new Tuple<char, int>(Symbol, Exponent).GetHashCode();
		}

		public override string ToString()
		{
			return (Exponent == 1) ? Symbol.ToString() : $"{Symbol}^{Exponent}";
		}

		#endregion
	}
}
