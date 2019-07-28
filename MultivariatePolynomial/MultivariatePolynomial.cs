using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace PolynomialLibrary
{

	public class MultivariatePolynomial
	{
		public Term[] Terms { get; private set; }

		#region Constructor & Parse

		public MultivariatePolynomial(Term[] terms)
		{
			Terms = terms;
		}

		public static MultivariatePolynomial Parse(string polynomialString)
		{
			string input = polynomialString;
			if (string.IsNullOrWhiteSpace(input)) { throw new ArgumentException(); }

			string inputString = input.Replace(" ", "").Replace("-", "+-");
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

		#endregion

		#region Evaluate

		public BigInteger Evaluate(List<Tuple<char, BigInteger>> indeterminateValues)
		{
			foreach (Term term in Terms)
			{
				term.SetIndeterminateValues(indeterminateValues);
			}

			BigInteger result = new BigInteger(0);
			foreach (Term term in Terms)
			{
				result = BigInteger.Add(result, term.Evaluate());
			}
			return result;
		}

		#endregion

		#region Arithmetic

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
				var match = leftTermsList.Where(leftTerm => Term.AreCompatable(leftTerm, rightTerm));

				if (match.Any())
				{
					Term matchTerm = match.Single();
					leftTermsList.Remove(matchTerm);

					Term sum = operation.Invoke(matchTerm, rightTerm);
					leftTermsList.Add(sum);
				}
				else
				{
					leftTermsList.Add(rightTerm);
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
					var likeTerms = resultTerms.Where(trm => Term.AreCompatable(newTerm, trm));
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
				return new MultivariatePolynomial(new Term[] { new Term(1, new Indeterminate[] { }) });
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
			return result;
		}

		#endregion

		#region Overrides and Interface implementations

		public MultivariatePolynomial Clone()
		{
			return new MultivariatePolynomial(CloneHelper<Term>.CloneCollection(Terms).ToArray());
		}

		public override string ToString()
		{
			return string.Join(" + ", Terms.Select(trm => trm.ToString())).Replace("+ -", "- ");
		}

		#endregion

	}
}
