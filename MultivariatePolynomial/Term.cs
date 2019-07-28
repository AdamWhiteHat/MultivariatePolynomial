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

		#region Constructor & Parse

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

		#endregion

		internal static bool AreCompatable(Term left, Term right)
		{
			if (left == null || right == null) { throw new ArgumentNullException(); }
			if (left.Variables.Length != right.Variables.Length) { return false; }

			int index = 0;
			foreach (Indeterminate variable in left.Variables)
			{
				if (!variable.Equals(right.Variables[index]))
				{
					return false;
				}
				index++;
			}
			return true;
		}

		#region Evaluate

		public void SetIndeterminateValues(List<Tuple<char, BigInteger>> indeterminateValues)
		{
			foreach (Tuple<char, BigInteger> indeterminateValue in indeterminateValues)
			{
				var matches = Variables.Where(indt => indt.Symbol == indeterminateValue.Item1);
				if (matches.Any())
				{
					Indeterminate match = matches.Single();
					match.SetValue(indeterminateValue.Item2);
				}
			}
		}

		public BigInteger Evaluate()
		{
			if (Variables.Any())
			{
				return BigInteger.Multiply(CoEfficient, Variables.Select(indt => indt.Evaluate()).Aggregate(BigInteger.Multiply) );
			}
			else
			{
				return CoEfficient;
			}
		}

		#endregion

		#region Arithmetic

		public static Term Add(Term left, Term right)
		{
			if (!AreCompatable(left, right)) { throw new ArgumentException("Terms are incompatable for adding; Their indeterminates must match."); }
			return new Term(BigInteger.Add(left.CoEfficient, right.CoEfficient), CloneHelper<Indeterminate>.CloneCollection(left.Variables).ToArray());
		}

		public static Term Subtract(Term left, Term right)
		{
			return Add(left, Negate(right));
		}

		public static Term Negate(Term term)
		{
			return new Term(BigInteger.Negate(term.CoEfficient), CloneHelper<Indeterminate>.CloneCollection(term.Variables).ToArray());
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

		#endregion

		#region Overrides and Interface implementations

		public Term Clone()
		{
			return new Term(new BigInteger(CoEfficient.ToByteArray()), CloneHelper<Indeterminate>.CloneCollection(Variables).ToArray());
		}

		public bool Equals(Term other)
		{
			return this.Equals(this, other);
		}

		public bool Equals(Term x, Term y)
		{
			if (x == null) { return (y == null) ? true : false; }
			if (x.CoEfficient != y.CoEfficient) { return false; }
			if (x.Variables.Length != y.Variables.Length) { return false; }

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
					if (parts.Any())
					{
						parts[0] = parts[0].Insert(0, "-");
					}
					else
					{
						parts.Add(CoEfficient.ToString());
					}
				}
			}
			return string.Join("*", parts);
		}

		#endregion

	}
}
