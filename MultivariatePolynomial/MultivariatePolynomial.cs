using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public class MultivariatePolynomial
	{
		public Term[] Terms { get; private set; }
		public int Degree { get { return Terms.Any() ? Terms.Select(t => t.Degree).Max() : 0; } }

		#region Constructor & Parse

		public MultivariatePolynomial(Term[] terms)
		{
			IEnumerable<Term> newTerms = terms?.Where(trm => trm.CoEfficient != 0) ?? new Term[0];
			if (!newTerms.Any())
			{
				Terms = new Term[] { Term.Zero };
				return;
			}

			Terms = CloneHelper<Term>.CloneCollection(newTerms).ToArray();
			OrderMonomials();
		}

		public static MultivariatePolynomial Parse(string polynomialString)
		{
			string input = polynomialString;
			if (string.IsNullOrWhiteSpace(input)) { throw new ArgumentException(); }

			string inputString = input.Replace(" ", "").Replace("-", "+-");
			if (inputString.StartsWith("+-")) { inputString = new string(inputString.Skip(1).ToArray()); }
			string[] stringTerms = inputString.Split(new char[] { '+' });

			if (!stringTerms.Any()) { throw new FormatException(); }

			Term[] terms = stringTerms.Select(str => Term.Parse(str)).ToArray();

			return new MultivariatePolynomial(terms);
		}

		public static MultivariatePolynomial GetDerivative(MultivariatePolynomial poly, char symbol)
		{
			List<Term> resultTerms = new List<Term>();
			foreach (Term term in poly.Terms)
			{
				if (term.Variables.Any() && term.Variables.Any(indt => indt.Symbol == symbol))
				{
					BigInteger newTerm_Coefficient = 0;
					List<Indeterminate> newTerm_Variables = new List<Indeterminate>();

					foreach (Indeterminate variable in term.Variables)
					{
						if (variable.Symbol == symbol)
						{
							newTerm_Coefficient = term.CoEfficient * variable.Exponent;

							int newExponent = variable.Exponent - 1;
							if (newExponent > 0)
							{
								newTerm_Variables.Add(new Indeterminate(symbol, newExponent));
							}
						}
						else
						{
							newTerm_Variables.Add(variable.Clone());
						}
					}

					resultTerms.Add(new Term(newTerm_Coefficient, newTerm_Variables.ToArray()));
				}
			}

			return new MultivariatePolynomial(resultTerms.ToArray());
		}

		private void OrderMonomials()
		{
			if (Terms.Length > 1)
			{
				var orderedTerms = Terms.OrderBy(t => t.Degree); // First by degree
				orderedTerms = orderedTerms.ThenByDescending(t => t.VariableCount()); // Then by variable count
				orderedTerms = orderedTerms.ThenBy(t => t.CoEfficient); // Then by coefficient value
				orderedTerms = orderedTerms.
					ThenByDescending(t =>
						new string(t.Variables.OrderBy(v => v.Symbol).Select(v => v.Symbol).ToArray())
					); // Lastly, lexicographic order of variables. Descending order because zero degree terms (smaller stuff) goes first.

				Terms = orderedTerms.ToArray();
			}
		}

		internal bool HasVariables()
		{
			return this.Terms.Any(t => t.HasVariables());
		}

		internal BigInteger MaxCoefficient()
		{
			if (HasVariables())
			{
				var termsWithVariables = this.Terms.Select(t => t).Where(t => t.HasVariables());
				return termsWithVariables.Select(t => t.CoEfficient).Max();
			}
			return -1;
		}

		#endregion

		#region Evaluate

		public BigInteger Evaluate(List<Tuple<char, BigInteger>> indeterminateValues)
		{
			BigInteger result = new BigInteger(0);
			foreach (Term term in Terms)
			{
				BigInteger termValue = term.CoEfficient.Clone();

				if (term.Variables.Any())
				{
					var variableValues =
						term.Variables
						.Select(indetrmnt =>
							indeterminateValues.Where(tup => tup.Item1 == indetrmnt.Symbol)
											  .Select(tup => BigInteger.Pow(tup.Item2, indetrmnt.Exponent))
											  .Single()
						);

					termValue = BigInteger.Multiply(termValue, variableValues.Aggregate(BigInteger.Multiply));
				}

				result = BigInteger.Add(result, termValue);
			}
			return result;
		}

		public double Evaluate(List<Tuple<char, double>> indeterminateValues)
		{
			double result = 0;
			foreach (Term term in Terms)
			{
				double termValue = (double)term.CoEfficient.Clone();

				if (term.Variables.Any())
				{
					var variableValues =
						term.Variables
						.Select(indetrmnt =>
							indeterminateValues.Where(tup => tup.Item1 == indetrmnt.Symbol)
											  .Select(tup => Math.Pow(tup.Item2, indetrmnt.Exponent))
											  .Single()
						);

					termValue *= variableValues.Aggregate((l, r) => l * r);
				}

				result += termValue;
			}
			return result;
		}

		#endregion

		#region Arithmetic

		public static MultivariatePolynomial GCD(MultivariatePolynomial left, MultivariatePolynomial right)
		{
			MultivariatePolynomial dividend = left.Clone();
			MultivariatePolynomial divisor = right.Clone();
			MultivariatePolynomial quotient;
			MultivariatePolynomial remainder;
			BigInteger dividendLeadingCoefficient = 0;
			BigInteger divisorLeadingCoefficient = 0;

			bool swap = false;

			do
			{
				swap = false;

				dividendLeadingCoefficient = dividend.Terms.Last().CoEfficient;
				divisorLeadingCoefficient = divisor.Terms.Last().CoEfficient;

				if (dividend.Degree < divisor.Degree)
				{
					swap = true;
				}
				else if (dividend.Degree == divisor.Degree && dividendLeadingCoefficient < divisorLeadingCoefficient)
				{
					swap = true;
				}

				if (swap)
				{
					MultivariatePolynomial temp = dividend.Clone();
					dividend = divisor;
					divisor = temp.Clone();
				}

				quotient = MultivariatePolynomial.Divide(dividend, divisor);
				dividend = quotient.Clone();

			}
			while (BigInteger.Abs(dividendLeadingCoefficient) > 0 && BigInteger.Abs(divisorLeadingCoefficient) > 0 && dividend.HasVariables() && divisor.HasVariables());

			if (dividend.HasVariables())
			{
				return divisor.Clone();
			}
			else
			{
				return dividend.Clone();
			}
		}

		public static MultivariatePolynomial Add(MultivariatePolynomial left, MultivariatePolynomial right)
		{
			return OneToOneArithmetic(left, right, Term.Add);
		}

		public static MultivariatePolynomial Subtract(MultivariatePolynomial left, MultivariatePolynomial right)
		{
			return OneToOneArithmetic(left, right, Term.Subtract);
		}

		private static MultivariatePolynomial OneToOneArithmetic(MultivariatePolynomial left, MultivariatePolynomial right, Func<Term, Term, Term> operation)
		{
			List<Term> leftTermsList = CloneHelper<Term>.CloneCollection(left.Terms).ToList();

			foreach (Term rightTerm in right.Terms)
			{
				var match = leftTermsList.Where(leftTerm => Term.AreIdentical(leftTerm, rightTerm));
				if (match.Any())
				{
					Term matchTerm = match.Single();
					leftTermsList.Remove(matchTerm);

					Term result = operation.Invoke(matchTerm, rightTerm);
					if (result.CoEfficient != 0)
					{
						if (!leftTermsList.Any(lt => lt.Equals(result)))
						{
							leftTermsList.Add(result);
						}
					}
				}
				else
				{
					leftTermsList.Add(Term.Negate(rightTerm));
				}
			}
			return new MultivariatePolynomial(leftTermsList.ToArray());
		}

		public static MultivariatePolynomial Multiply(MultivariatePolynomial left, MultivariatePolynomial right)
		{
			List<Term> resultTerms = new List<Term>();

			foreach (var leftTerm in left.Terms)
			{
				foreach (var rightTerm in right.Terms)
				{
					Term newTerm = Term.Multiply(leftTerm, rightTerm);

					// Combine like terms
					var likeTerms = resultTerms.Where(trm => Term.AreIdentical(newTerm, trm));
					if (likeTerms.Any())
					{
						resultTerms = resultTerms.Except(likeTerms).ToList();

						Term likeTermsSum = likeTerms.Aggregate(Term.Add);
						Term sum = Term.Add(newTerm, likeTermsSum);

						newTerm = sum;
					}

					// Add new term to resultTerms
					resultTerms.Add(newTerm);
				}
			}

			return new MultivariatePolynomial(resultTerms.ToArray());
		}

		public static MultivariatePolynomial Pow(MultivariatePolynomial poly, int exponent)
		{
			if (exponent < 0)
			{
				throw new NotImplementedException("Raising a polynomial to a negative exponent not supported.");
			}
			else if (exponent == 0)
			{
				return new MultivariatePolynomial(new Term[] { new Term(1, new Indeterminate[0]) });
			}
			else if (exponent == 1)
			{
				return poly.Clone();
			}

			MultivariatePolynomial result = poly.Clone();

			int counter = exponent - 1;
			while (counter != 0)
			{
				result = MultivariatePolynomial.Multiply(result, poly);
				counter -= 1;
			}
			return new MultivariatePolynomial(result.Terms);
		}

		public static MultivariatePolynomial Divide(MultivariatePolynomial left, MultivariatePolynomial right)
		{
			List<Term> newTermsList = new List<Term>();
			List<Term> leftTermsList = CloneHelper<Term>.CloneCollection(left.Terms).ToList();

			foreach (Term rightTerm in right.Terms)
			{
				var matches = leftTermsList.Where(leftTerm => Term.ShareCommonFactor(leftTerm, rightTerm)).ToList();
				if (matches.Any())
				{
					foreach (Term matchTerm in matches)
					{
						leftTermsList.Remove(matchTerm);
						Term result = Term.Divide(matchTerm, rightTerm);
						if (result != Term.Empty)
						{
							if (!newTermsList.Any(lt => lt.Equals(result)))
							{
								newTermsList.Add(result);
							}
						}
					}
				}
				else
				{
					///newTermsList.Add(rightTerm);
				}
			}
			return new MultivariatePolynomial(newTermsList.ToArray());
		}

		#endregion

		#region Overrides and Interface implementations
		public MultivariatePolynomial Clone()
		{
			return new MultivariatePolynomial(CloneHelper<Term>.CloneCollection(Terms).ToArray());
		}

		public bool Equals(MultivariatePolynomial other)
		{
			return this.Equals(this, other);
		}

		public bool Equals(MultivariatePolynomial x, MultivariatePolynomial y)
		{
			if (x == null) { return (y == null) ? true : false; }
			if (!x.Terms.Any()) { return (!y.Terms.Any()) ? true : false; }
			if (x.Terms.Length != y.Terms.Length) { return false; }
			if (x.Degree != y.Degree) { return false; }

			int index = 0;
			foreach (Term term in x.Terms)
			{
				if (!term.Equals(y.Terms[index++])) { return false; }
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as MultivariatePolynomial);
		}

		public int GetHashCode(MultivariatePolynomial obj)
		{
			return obj.GetHashCode();
		}

		public override int GetHashCode()
		{
			int hashCode = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
			if (Terms.Any())
			{
				foreach (var term in Terms)
				{
					hashCode = Term.CombineHashCodes(hashCode, term.GetHashCode());
				}
			}
			return hashCode;
		}

		public override string ToString()
		{
			bool isFirstPass = true;
			string signString = string.Empty;
			string termString = string.Empty;
			string result = string.Empty;

			foreach (Term term in Terms.Reverse())
			{
				signString = string.Empty;
				termString = string.Empty;

				if (isFirstPass)
				{
					isFirstPass = false;
				}
				else
				{
					if (term.CoEfficient.Sign == -1)
					{
						signString = $" - ";
					}
					else if (term.CoEfficient.Sign == 1)
					{
						signString = $" + ";
					}
				}

				termString = term.ToString();

				result += $"{signString}{termString}";
			}

			return result;
		}

		#endregion

	}
}
