using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestMultivariatePolynomial")]

namespace ExtendedArithmetic
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
				var orderedTerms = Terms.OrderByDescending(t => t.Degree); // First by degree
				orderedTerms = orderedTerms.ThenBy(t => t.VariableCount()); // Then by variable count
				orderedTerms = orderedTerms.ThenByDescending(t => t.CoEfficient); // Then by coefficient value
				orderedTerms = orderedTerms.
					ThenBy(t =>
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
											  .FirstOrDefault()
						);

					if (variableValues.Any())
					{
						termValue = BigInteger.Multiply(termValue, variableValues.Aggregate(BigInteger.Multiply));
					}
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
											  .FirstOrDefault()
						);

					if (variableValues.Any())
					{
						termValue *= variableValues.Aggregate((l, r) => l * r);
					}
				}

				result += termValue;
			}
			return result;
		}

		public Complex Evaluate(List<Tuple<char, Complex>> indeterminateValues)
		{
			Complex result = 0;
			foreach (Term term in Terms)
			{
				Complex termValue = new Complex((double)term.CoEfficient.Clone(), 0);

				if (term.Variables.Any())
				{
					var variableValues =
						term.Variables
						.Select(indetrmnt =>
							indeterminateValues.Where(tup => tup.Item1 == indetrmnt.Symbol)
											  .Select(tup => Complex.Pow(tup.Item2, (double)indetrmnt.Exponent))
											  .FirstOrDefault()
						);

					if (variableValues.Any())
					{
						termValue *= variableValues.Aggregate((l, r) => l * r);
					}
				}

				result += termValue;
			}
			return result;
		}

		/// <summary>
		/// Like the Evaluate method, except it replaces indeterminates with Polynomials instead of integers,
		/// and returns the resulting (usually large) Polynomial
		/// </summary>
		public MultivariatePolynomial FunctionalComposition(List<Tuple<char, MultivariatePolynomial>> indeterminateValues)
		{
			List<Term> terms = this.Terms.ToList();
			List<MultivariatePolynomial> composedTerms = new List<MultivariatePolynomial>();

			foreach (Term trm in terms)
			{
				MultivariatePolynomial constant = MultivariatePolynomial.Parse(trm.CoEfficient.ToString());
				List<MultivariatePolynomial> toCompose = new List<MultivariatePolynomial>();
				toCompose.Add(constant.Clone());
				foreach (Indeterminate variable in trm.Variables)
				{
					int exp = variable.Exponent;
					MultivariatePolynomial valueOfIndeterminate = indeterminateValues.Where(tup => tup.Item1 == variable.Symbol).Select(tup => tup.Item2).FirstOrDefault();
					if (valueOfIndeterminate == null)
					{
						MultivariatePolynomial thisVariableAsPoly = new MultivariatePolynomial(new Term[] { new Term(BigInteger.One, new Indeterminate[] { variable }) });
						toCompose.Add(thisVariableAsPoly);
					}
					else if (exp == 0) { continue; }
					else if (exp == 1)
					{
						toCompose.Add(valueOfIndeterminate);
					}
					else
					{
						MultivariatePolynomial toMultiply = MultivariatePolynomial.Pow(valueOfIndeterminate, exp);
						toCompose.Add(toMultiply);
					}
				}
				MultivariatePolynomial composed = MultivariatePolynomial.Product(toCompose);
				composedTerms.Add(composed);
			}

			MultivariatePolynomial result = MultivariatePolynomial.Sum(composedTerms);
			return result;
		}

		#endregion

		#region Arithmetic

		public static MultivariatePolynomial GCD(MultivariatePolynomial left, MultivariatePolynomial right)
		{
			// TODO: This method needs to employ several strategies in order to perform GCD:
			//
			// IF
			//		The polynomial only contains 1 indeterminant,
			//		that is, if the polynomial is actually univariate
			// THEN
			//		Attempt to factor the polynomial into roots (X + 1)(X + 3)(X + 6)
			//		by "factoring" (i.e. apply the Factor method of my univariate polynomial library ExtendedArithmetic.Polynomial)
			//		Remove from the dividend matching roots in the divisor.
			// ELSE
			//		Groebner basis methods:
			//		-Divisibility of monomials
			//			Going monomial by monomial, attempt to apply Divisibility of monomials (terms):
			//			For each pair of monomials in the Cartesian product of the divisor's terms and the dividend's terms:
			//			One says that A divides B, or that N is a multiple of B, if m_i ≤ b_i for every i;
			//			that is, if A is componentwise not greater than B.
			//			In this case, the quotient B/A is defined as B/A = x_1^b_1 − a_1 ... x_n^b_n − a_n.
			//			In other words, the exponent vector of B/A is the componentwise subtraction
			//			of the exponent vectors of B and A.
			//			
			//			The greatest common divisor, gcd(A, B), of A and B
			//			is the monomial x_1^min(a_1 , b_1) ... x_n^min(a_n, b_n)
			//			whose exponent vector is the componentwise minimum of A and B.
			//		-Lead-reduction
			//		-S-Polynomial
			//
			// ELSE
			//		Does it make sense to apply the Rational root theorem to integral polynomials?
			//		Probably not.
			//		GenericMultivariatePolynomial library which have real or rational coefficients
			//		could would benefit from the Rational root theorem.
			//

			// For debugging purposes:
			// var leftGCD = Term.GetCommonDivisors(left.Terms);
			// var rightGCD = Term.GetCommonDivisors(right.Terms);

			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));

			List<char> leftSymbols = left.Terms.SelectMany(t => t.Variables.Where(v => v.Exponent > 0).Select(v => v.Symbol)).Distinct().ToList();
			List<char> rightSymbols = right.Terms.SelectMany(t => t.Variables.Where(v => v.Exponent > 0).Select(v => v.Symbol)).Distinct().ToList();
			List<char> combinedSymbols = leftSymbols.Concat(rightSymbols).Distinct().ToList();

			// If polynomial is actually univariate
			if (combinedSymbols.Count <= 1)
			{
				List<MultivariatePolynomial> leftFactors = Factor(left);
				List<MultivariatePolynomial> rightFactors = Factor(right);

				Console.WriteLine("Left.Factors():");
				Console.WriteLine(string.Join(Environment.NewLine, leftFactors));
				Console.WriteLine("");

				Console.WriteLine("Right.Factors():");
				Console.WriteLine(string.Join(Environment.NewLine, rightFactors));
				Console.WriteLine();


				List<MultivariatePolynomial> smaller = leftFactors;
				List<MultivariatePolynomial> larger = rightFactors;
				if (leftFactors.Count > rightFactors.Count)
				{
					smaller = rightFactors;
					larger = leftFactors;
				}

				List<MultivariatePolynomial> common = new List<MultivariatePolynomial>();
				foreach (var factor in smaller)
				{
					if (larger.Contains(factor))
					{
						common.Add(factor);
					}
				}

				MultivariatePolynomial product = MultivariatePolynomial.Parse("1");

				foreach (var poly in common)
				{
					product = MultivariatePolynomial.Multiply(product, poly);
				}

				return product;
			}
			else
			{
				List<Term> newTermsList = new List<Term>();
				List<Term> leftTermsList = CloneHelper<Term>.CloneCollection(left.Terms).ToList();

				foreach (Term rightTerm in right.Terms)
				{
					var commonDivisorsList = leftTermsList.Select(trm => Term.GetCommonDivisors(trm, rightTerm)).ToList();
					var matches = leftTermsList.Where(leftTerm => Term.ShareCommonFactor(leftTerm, rightTerm)).ToList();
					if (matches.Any())
					{
						foreach (Term matchTerm in matches)
						{
							leftTermsList.Remove(matchTerm);
							Term quotient = Term.Divide(matchTerm, rightTerm);
							if (quotient != Term.Empty)
							{
								//if (!newTermsList.Any(lt => lt.Equals(quotient)))
								//{
								newTermsList.Add(quotient);
								//}
							}
						}
					}
				}

				MultivariatePolynomial result = new MultivariatePolynomial(newTermsList.ToArray());
				return result;
			}
		}

		/// <summary>
		/// Factors the specified polynomial.
		/// </summary>
		public static List<MultivariatePolynomial> Factor(MultivariatePolynomial polynomial)
		{
			if (polynomial == null) throw new ArgumentNullException(nameof(polynomial));

			List<MultivariatePolynomial> results = new List<MultivariatePolynomial>();

			List<char> symbols = polynomial.Terms.SelectMany(t => t.Variables.Where(v => v.Exponent > 0).Select(v => v.Symbol)).Distinct().ToList();
			if (symbols.Count >= 1) // Rational root theorem, essentially
			{
				char symbol = symbols.First();

				MultivariatePolynomial remainingPoly = polynomial.Clone();

				IEnumerable<BigInteger> coefficients = remainingPoly.Terms.Select(trm => trm.CoEfficient);
				BigInteger gcd = coefficients.Aggregate(BigInteger.GreatestCommonDivisor);
				if (gcd > 1)
				{
					MultivariatePolynomial gcdPoly = MultivariatePolynomial.Parse(gcd.ToString());
					results.Add(gcdPoly);
					remainingPoly = Divide(remainingPoly, gcdPoly);
				}

				if (remainingPoly.Degree == 0)
				{
					return results;
				}

				int resultCount = -1;

				while (remainingPoly.Degree > 0)
				{
					if (resultCount == results.Count)
					{
						break;
					}

					var leadingCoeffQ = remainingPoly.Terms.First().CoEfficient;
					var constantCoeffP = remainingPoly.Terms.Last().CoEfficient;

					var constantDivisors = BigIntegerExtensionMethods.GetAllDivisors(constantCoeffP).ToList();
					var leadingDivisors = BigIntegerExtensionMethods.GetAllDivisors(leadingCoeffQ).ToList();

					// <(denominator/numerator), numerator, denominator>
					List<Tuple<double, BigInteger, BigInteger>> candidates =
						constantDivisors.SelectMany(n => leadingDivisors.SelectMany(d =>
						{
							List<Tuple<double, BigInteger, BigInteger>> selected = new List<Tuple<double, BigInteger, BigInteger>>();
							BigInteger num1 = n;
							BigInteger num2 = BigInteger.Negate(n);
							BigInteger denom = d;

							double quotient1 = (double)num1 / (double)denom;
							double quotient2 = (double)num2 / (double)denom;
							selected.Add(new Tuple<double, BigInteger, BigInteger>(quotient1, num1, denom));
							selected.Add(new Tuple<double, BigInteger, BigInteger>(quotient2, num2, denom));

							return selected;
						})).ToList();

					candidates = candidates.OrderBy(tup => Math.Abs(tup.Item1))
										   .ThenByDescending(tup => Math.Sign(tup.Item1))
										   .ToList();

					resultCount = results.Count;

					foreach (Tuple<double, BigInteger, BigInteger> candidate in candidates)
					{
						double evalResult = remainingPoly.Evaluate(new List<Tuple<char, double>>() { new Tuple<char, double>(symbol, candidate.Item1) });
						bool isRoot = (evalResult == 0.0d);

						if (!isRoot)
						{
							continue;
						}

						string rootMonomial = $"{candidate.Item3}*{symbol} {(BigInteger.Negate(candidate.Item2).Sign == -1 ? "-" : "+")} {BigInteger.Abs(candidate.Item2)}";
						MultivariatePolynomial factor = MultivariatePolynomial.Parse(rootMonomial);

						remainingPoly = Divide(remainingPoly, factor);
						results.Add(factor);

						break;
					}
				}
			}
			else
			{

			}
			return results;
		}

		public static MultivariatePolynomial Sum(IEnumerable<MultivariatePolynomial> polys)
		{
			MultivariatePolynomial result = null;
			foreach (MultivariatePolynomial p in polys)
			{
				if (result == null)
				{
					result = p.Clone();
				}
				else
				{
					result = MultivariatePolynomial.Add(result, p);
				}
			}
			return result;
		}

		public static MultivariatePolynomial Product(IEnumerable<MultivariatePolynomial> polys)
		{
			MultivariatePolynomial result = null;
			foreach (MultivariatePolynomial p in polys)
			{
				if (result == null)
				{
					result = p.Clone();
				}
				else
				{
					result = MultivariatePolynomial.Multiply(result, p);
				}
			}
			return result;
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
				var match = leftTermsList.Where(leftTerm => Term.HasIdenticalIndeterminates(leftTerm, rightTerm));
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
					if (operation == Term.Subtract)
					{
						leftTermsList.Add(Term.Negate(rightTerm));
					}
					else
					{
						leftTermsList.Add(rightTerm);
					}
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
					var likeTerms = resultTerms.Where(trm => Term.HasIdenticalIndeterminates(newTerm, trm));
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
			// Multivariate polynomial division is ill defined.
			// 
			// When going monomial-by-monomial (or term-by-term), dividing them by some common multiple,
			// as part of the process of dividing a multivariate polynomial,
			// the order in which you visit each term matters and the result you get can be different
			// depending on what order you divide the terms in.
			// 
			// So for consistent, reliable results, you must impose a total, well-order ordering on the monomials.
			// However, there is no obvious natural ordering to arbitrary monomials.
			// To illustrate what I mean, let me give you an example:
			// Which of the two following monomials has the greater value?
			// 5*x^2*y^3
			// or
			// 5*y^2*x^3 ?
			// 
			// It is not so clear.
			// 
			// We must impose an ordering. Luckily, what ordering we choose is not as important as it is
			// that the ordering is well-ordered, total and it is consistent and enforced.
			//
			// Because multivariate polynomial division is a whole different beast,
			// this method handles two different cases:
			// 1) Where both polynomials contain 1 or less variables and where both of those variables are the same (when applicable)
			// 2) Where one or both polynomials are multivariate (i.e. All other cases)
			//
			//

			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));

			List<char> leftSymbols = left.Terms.SelectMany(t => t.Variables.Where(v => v.Exponent > 0).Select(v => v.Symbol)).Distinct().ToList();
			List<char> rightSymbols = right.Terms.SelectMany(t => t.Variables.Where(v => v.Exponent > 0).Select(v => v.Symbol)).Distinct().ToList();
			List<char> combinedSymbols = leftSymbols.Concat(rightSymbols).Distinct().ToList();

			// If polynomial is actually univariate
			if (combinedSymbols.Count <= 1)
			{
				if (right.Degree > left.Degree)
				{
					return left;
				}

				int rightDegree = right.Degree;
				int quotientDegree = (left.Degree - rightDegree) + 1;

				// Turn an array of Terms into an array of coefficients, including terms with coefficient of zero,
				// such that index into the array encodes its degree/exponent
				BigInteger[] rem = Enumerable.Repeat(BigInteger.Zero, left.Degree + 1).ToArray();
				foreach (Term t in left.Terms)
				{
					rem[t.Degree] = t.CoEfficient;
				}
				// Turn an array of Terms into an array of coefficients
				BigInteger[] rightCoeffs = Enumerable.Repeat(BigInteger.Zero, rightDegree + 1).ToArray();
				foreach (Term t in right.Terms)
				{
					rightCoeffs[t.Degree] = t.CoEfficient;
				}
				// Array of coefficients to hold our result.
				BigInteger[] quotient = Enumerable.Repeat(BigInteger.Zero, quotientDegree + 1).ToArray();

				BigInteger leadingCoefficent = rightCoeffs[rightDegree];

				// The leading coefficient is the only number we ever divide by
				// (so if right is monic, polynomial division does not involve division at all!)
				for (int i = quotientDegree - 1; i >= 0; i--)
				{
					quotient[i] = BigInteger.Divide(rem[rightDegree + i], leadingCoefficent);
					rem[rightDegree + i] = new BigInteger(0);

					for (int j = rightDegree + i - 1; j >= i; j--)
					{
						rem[j] = BigInteger.Subtract(rem[j], BigInteger.Multiply(quotient[i], rightCoeffs[j - i]));
					}
				}

				// Turn array of coefficients into array of terms.
				char symbol = 'X';
				if (combinedSymbols.Any())
				{
					symbol = combinedSymbols.First();
				}
				List<Term> newTerms = new List<Term>();
				int index = -1;
				foreach (BigInteger q in quotient)
				{
					index++;

					if (q != 0)
					{
						Term newTerm;

						if (index == 0)
						{
							newTerm = new Term(q, Indeterminate.Empty);
						}
						else
						{
							newTerm = new Term(q, new Indeterminate[] { new Indeterminate(symbol, index) });
						}

						newTerms.Add(newTerm);
					}
				}

				return new MultivariatePolynomial(newTerms.ToArray());
			}
			else // All other cases (i.e. actually multivariate)
			{
				List<Term> newTermsList = new List<Term>();
				List<Term> leftTermsList = CloneHelper<Term>.CloneCollection(left.Terms).ToList();
				List<Term> rightTermsList = CloneHelper<Term>.CloneCollection(right.Terms).ToList();

				foreach (Term rightTerm in rightTermsList)
				{
					var matches = leftTermsList.Where(leftTerm => Term.ShareCommonFactor(leftTerm, rightTerm)).ToList();
					if (matches.Any())
					{
						foreach (Term matchTerm in matches)
						{
							leftTermsList.Remove(matchTerm);
							Term quotient = Term.Divide(matchTerm, rightTerm);
							if (quotient != Term.Empty)
							{
								if (!newTermsList.Any(lt => lt.Equals(quotient)))
								{
									newTermsList.Add(quotient);
								}
							}
						}
					}
					else
					{
						///newTermsList.Add(rightTerm);
					}
				}
				MultivariatePolynomial result = new MultivariatePolynomial(newTermsList.ToArray());
				return result;
			}
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

			result = string.Join(" + ", Terms.Select(trm => trm.ToString()));
			result = result.Replace(" + -", " - ");

			return result;
		}

		#endregion

	}
}
