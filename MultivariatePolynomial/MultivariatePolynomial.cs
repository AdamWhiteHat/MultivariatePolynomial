using System;
using System.Linq;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public class MultivariatePolynomial
	{
		public Term[] Terms { get; private set; }

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

		public override string ToString()
		{
			return string.Join(" + ", Terms.Select(trm => trm.ToString())).Replace("+ -", "- ");
		}
	}
}
