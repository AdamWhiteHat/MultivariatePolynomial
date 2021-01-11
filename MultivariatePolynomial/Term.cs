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
		public int Degree { get { return Variables.Any() ? Variables.Select(v => v.Exponent).Sum() : 0; } }

		public static Term Empty = new Term(BigInteger.Zero, new Indeterminate[0]);
		internal static Term Zero = new Term(BigInteger.Zero, new Indeterminate[] { new Indeterminate('X', 0) });

		#region Constructor & Parse

		public Term(BigInteger coefficient, Indeterminate[] variables)
		{
			CoEfficient = coefficient.Clone();
			Variables = CloneHelper<Indeterminate>.CloneCollection(variables).ToArray();
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
			else if (parts[0].StartsWith("-") && parts[0].Any(c => char.IsLetter(c)))
			{
				coefficient = BigInteger.MinusOne;
				parts[0] = parts[0].Replace("-", "");
			}

			Indeterminate[] variables = parts.Select(str => Indeterminate.Parse(str)).ToArray();

			return new Term(coefficient, variables);
		}

		#endregion

		#region Internal Helper Methods

		internal static bool ShareCommonFactor(Term left, Term right)
		{
			if (left == null || right == null) { throw new ArgumentNullException(); }
			if (!left.Variables.Any(lv => right.Variables.Any(rv => rv.Equals(lv)))) { return false; }
			if (right.CoEfficient != 1 && (left.CoEfficient % right.CoEfficient != 0)) { return false; }
			return true;
		}

		internal static bool AreIdentical(Term left, Term right)
		{
			if (left == null || right == null) { throw new ArgumentNullException(); }
			if (left.Variables.Length != right.Variables.Length) { return false; }

			int index = 0;
			foreach (Indeterminate variable in left.Variables)
			{
				if (!variable.Equals(right.Variables[index])) { return false; }
				index++;
			}
			return true;
		}

		internal bool HasVariables()
		{
			return Variables.Any();
		}

		internal int VariableCount()
		{
			if (!HasVariables())
			{
				return 0;
			}
			else
			{
				return Variables.Length;
			}
		}

		#endregion

		#region Arithmetic

		public static Term Add(Term left, Term right)
		{
			if (!AreIdentical(left, right))
			{
				//throw new ArgumentException("Terms are incompatable for adding; Their indeterminates must match.");
				return Empty;
			}
			return new Term(BigInteger.Add(left.CoEfficient, right.CoEfficient), left.Variables);
		}

		public static Term Subtract(Term left, Term right)
		{
			return Add(left, Negate(right));
		}

		public static Term Negate(Term term)
		{
			return new Term(BigInteger.Negate(term.CoEfficient), term.Variables);
		}

		public static Term Multiply(Term left, Term right)
		{
			BigInteger resultCoefficient = BigInteger.Multiply(left.CoEfficient, right.CoEfficient);
			List<Indeterminate> resultVariables = new List<Indeterminate>();

			List<Indeterminate> rightVariables = right.Variables.ToList();

			foreach (var leftVar in left.Variables)
			{
				var matches = rightVariables.Where(indt => indt.Symbol == leftVar.Symbol).ToList();
				if (matches.Any())
				{
					foreach (var rightMatch in matches)
					{
						rightVariables.Remove(rightMatch);
						resultVariables.Add(
							new Indeterminate(leftVar.Symbol, (leftVar.Exponent + rightMatch.Exponent))
						);
					}
				}
				else
				{
					resultVariables.Add(leftVar.Clone());
				}
			}

			if (rightVariables.Any())
			{
				foreach (var rightVar in rightVariables)
				{
					resultVariables.Add(rightVar.Clone());
				}
			}

			resultVariables = resultVariables.OrderBy(indt => indt.Symbol).ThenBy(indt => indt.Exponent).ToList();

			return new Term(resultCoefficient, resultVariables.ToArray());
		}

		public static Term Divide(Term left, Term right)
		{
			if (!Term.ShareCommonFactor(left, right)) { return Empty; }

			BigInteger newCoefficient = BigInteger.Divide(left.CoEfficient, right.CoEfficient);

			List<Indeterminate> newVariables = new List<Indeterminate>();
			int max = left.Variables.Length;
			int index = 0;
			while (index < max)
			{
				if (index > right.Variables.Length - 1)
				{
					newVariables.Add(new Indeterminate(left.Variables[index].Symbol, left.Variables[index].Exponent));
				}
				else
				{
					if (left.Variables[index].Symbol == right.Variables[index].Symbol)
					{
						int newExponent = left.Variables[index].Exponent - right.Variables[index].Exponent;
						if (newExponent > 0)
						{
							newVariables.Add(new Indeterminate(left.Variables[index].Symbol, newExponent));
						}
					}
				}
				index++;
			}
			return new Term(newCoefficient, newVariables.ToArray());
		}

		#endregion

		#region Overrides and Interface implementations

		public Term Clone()
		{
			return new Term(CoEfficient.Clone(), CloneHelper<Indeterminate>.CloneCollection(Variables).ToArray());
		}
		public bool Equals(Term other)
		{
			return this.Equals(this, other);
		}

		public bool Equals(Term x, Term y)
		{
			if (x == null) { return (y == null) ? true : false; }
			if (x.CoEfficient != y.CoEfficient) { return false; }
			if (!x.Variables.Any()) { return (!y.Variables.Any()) ? true : false; }
			if (x.Variables.Length != y.Variables.Length) { return false; }

			int index = 0;
			foreach (Indeterminate variable in x.Variables)
			{
				if (!variable.Equals(y.Variables[index++])) { return false; }
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as Term);
		}

		public int GetHashCode(Term obj)
		{
			return obj.GetHashCode();
		}

		public override int GetHashCode()
		{
			int hashCode = CoEfficient.GetHashCode();
			if (Variables.Any())
			{
				foreach (var variable in Variables)
				{
					hashCode = CombineHashCodes(hashCode, variable.GetHashCode());
				}
			}
			return hashCode;
		}

		internal static int CombineHashCodes(int h1, int h2)
		{
			return (((h1 << 5) + h1) ^ h2);
		}

		public override string ToString()
		{
			if (CoEfficient == 0)
			{
				return "0";
			}

			string signString = string.Empty;
			string coefficientString = string.Empty;
			string variableString = string.Empty;
			string multiplyString = string.Empty;
					
			if (Variables.Any())
			{
				variableString = string.Join("*", Variables.Select(v => v.ToString()));
			}
			else if (BigInteger.Abs(CoEfficient) == 1)
			{
				coefficientString = BigInteger.Abs(CoEfficient).ToString();
			}

			if (BigInteger.Abs(CoEfficient) != 1)
			{
				if (Variables.Any())
				{
					multiplyString = "*";
				}
				coefficientString = BigInteger.Abs(CoEfficient).ToString();
			}

			return $"{coefficientString}{multiplyString}{variableString}";
		}

		#endregion

	}
}
