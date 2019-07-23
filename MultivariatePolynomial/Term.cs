using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public class Term : ICloneable<Term>, IEquatable<Term>, IEqualityComparer<Term>
	{
		public BigInteger CoEfficient { get; }
		public Indeterminate[] Variables { get; private set; }

		public Term(BigInteger coefficient, Indeterminate[] variables)
		{
			CoEfficient = coefficient;
			Variables = variables;
		}

		internal static Term Parse(string termString)
		{
			if (string.IsNullOrWhiteSpace(termString)) { throw new ArgumentException(); }

			string input = termString.Replace(" ", "");
			if (string.IsNullOrWhiteSpace(input)) { throw new ArgumentException(); }

			string[] parts = input.Split(new char[] { '*' });

			BigInteger coefficient = BigInteger.One;
			if (parts[0].All(c => c == '-' || char.IsDigit(c)))
			{
				coefficient = BigInteger.Parse(parts[0]);
				parts = parts.Skip(1).ToArray();
			}

			Indeterminate[] variables = parts.Select(str => Indeterminate.Parse(str)).ToArray();

			return new Term(coefficient, variables);
		}


		#region Overrides and Interface implementations

		public Term Clone()
		{
			return new Term(new BigInteger(CoEfficient.ToByteArray()), Variables.ToArray());
		}

		public bool Equals(Term other)
		{
			return this.Equals(this, other);
		}

		public bool Equals(Term x, Term y)
		{
			if (x == null) { return (y == null) ? true : false; }

			if (x.CoEfficient != y.CoEfficient)
			{
				return false;
			}

			int index = 0;

			foreach (Indeterminate variable in x.Variables)
			{
				if (!variable.Equals(y.Variables[index]))
				{
					return false;
				}
				index++;
			}

			return true;
		}

		public int GetHashCode(Term obj)
		{
			return obj.ToString().GetHashCode();
		}

		public override string ToString()
		{
			List<string> parts = Variables.Select(v => v.ToString()).ToList();
			if (BigInteger.Abs(CoEfficient) != 1)
			{
				parts.Insert(0, CoEfficient.ToString());
			}
			else
			{
				if (CoEfficient.Sign == -1)
				{
					parts[0] = parts[0].Insert(0, "-");
				}
			}
			return string.Join("*", parts);
		}

		#endregion

	}
}
