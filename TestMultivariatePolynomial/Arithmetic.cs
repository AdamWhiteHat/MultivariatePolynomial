using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolynomialLibrary;

namespace TestMultivariatePolynomial
{
	[TestClass]
	public class Arithmetic
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestAdd()
		{
			string augend = "x^2 + 2*x - 1";
			string addend = "2*x^2 - 3*x + 6";

			string expected = "3*x^2 - x + 5";

			MultivariatePolynomial polyAugend = MultivariatePolynomial.Parse(augend);
			MultivariatePolynomial polyAddend = MultivariatePolynomial.Parse(addend);

			MultivariatePolynomial sum = MultivariatePolynomial.Add(polyAugend, polyAddend);
			string actual = sum.ToString();

			TestContext.WriteLine($"Sum: \"{sum}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Add({augend}, {addend});");
		}

		[TestMethod]
		public void TestSubtract1()
		{
			string minuend = "3*x";
			string subtrahend = "x + 2";

			string expected = "2*x - 2";

			MultivariatePolynomial polyMinuend = MultivariatePolynomial.Parse(minuend);
			MultivariatePolynomial polySubtrahend = MultivariatePolynomial.Parse(subtrahend);

			MultivariatePolynomial difference = MultivariatePolynomial.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Subtract: \"{difference}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Subtract({minuend}, {subtrahend});");
		}

		[TestMethod]
		public void TestSubtract2()
		{
			string minuend = "36*x*y + 6*x + 6*y + 1";
			string subtrahend = "36*x*y + 1";

			string expected = "6*x + 6*y";

			MultivariatePolynomial polyMinuend = MultivariatePolynomial.Parse(minuend);
			MultivariatePolynomial polySubtrahend = MultivariatePolynomial.Parse(subtrahend);

			MultivariatePolynomial difference = MultivariatePolynomial.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Difference: \"{difference}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Subtract({minuend}, {subtrahend});");
		}

		[TestMethod]
		public void TestMultiply()
		{
			string lhs = "6*x + 1";
			string rhs = "6*y + 1";
			string expected = "36*x*y + 6*x + 6*y + 1";

			MultivariatePolynomial polylhs = MultivariatePolynomial.Parse(lhs);
			MultivariatePolynomial polyrhs = MultivariatePolynomial.Parse(rhs);

			MultivariatePolynomial polyProdcut = MultivariatePolynomial.Multiply(polylhs, polyrhs);

			string actual = polyProdcut.ToString();

			TestContext.WriteLine($"Product: \"{actual}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Multiply({lhs}, {rhs});");
		}

		[TestMethod]
		public void TestDivide1()
		{
			string dividend = "36*x*y + 6*x + 6*y + 1";
			string divisor = "6*x + 1";

			string expected = "6*y + 1";

			MultivariatePolynomial polyDivedend = MultivariatePolynomial.Parse(dividend);
			MultivariatePolynomial polyDivisor = MultivariatePolynomial.Parse(divisor);

			MultivariatePolynomial quotient = MultivariatePolynomial.Divide(polyDivedend, polyDivisor);
			string actual = quotient.ToString();

			TestContext.WriteLine($"Quotient: \"{quotient}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Divide({dividend}, {divisor});");
		}

		[TestMethod]
		public void TestDivide2()
		{
			string dividend = "2*x*y^2 + 3*x*y + 4*y^2 + 6*y";
			string divisor = "x + 2";

			string expected = "2*y^2 + 3*y";

			MultivariatePolynomial polyDivedend = MultivariatePolynomial.Parse(dividend);
			MultivariatePolynomial polyDivisor = MultivariatePolynomial.Parse(divisor);

			MultivariatePolynomial quotient = MultivariatePolynomial.Divide(polyDivedend, polyDivisor);
			string actual = quotient.ToString();

			TestContext.WriteLine($"Quotient: \"{quotient}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Divide({dividend}, {divisor});");
		}

		[TestMethod]
		public void TestPow()
		{
			string polyBaseString = "2*x*y^2 - 1";
			int exponent = 2;

			string expected = "4*x^2*y^4 - 4*x*y^2 + 1";

			MultivariatePolynomial polyBase = MultivariatePolynomial.Parse(polyBaseString);
			MultivariatePolynomial power = MultivariatePolynomial.Pow(polyBase, exponent);

			string actual = power.ToString();

			TestContext.WriteLine($"Pow: \"{power}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Pow({polyBaseString}, {exponent});");
		}

		[TestMethod]
		public void TestGetDerivative1()
		{
			string polyString = "132*x*y + 77*x + 55*y + 1";
			string expected = "132*y + 77";

			MultivariatePolynomial poly = MultivariatePolynomial.Parse(polyString);
			MultivariatePolynomial derivative = MultivariatePolynomial.GetDerivative(poly, 'x');

			string actual = derivative.ToString();

			TestContext.WriteLine($"Derivative: \"{derivative}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.GetDerivative({polyString});");
		}

		[TestMethod]
		public void TestGetDerivative2()
		{
			string polyString = "4*x^2*y^4 - 4*x*y^2 + 1";
			string expected = "8*x*y^4 - 4*y^2";

			MultivariatePolynomial poly = MultivariatePolynomial.Parse(polyString);
			MultivariatePolynomial derivative = MultivariatePolynomial.GetDerivative(poly, 'x');

			string actual = derivative.ToString();

			TestContext.WriteLine($"Derivative: \"{derivative}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.GetDerivative({polyString});");
		}

		[TestMethod]
		public void TestGCD()
		{
			throw new NotImplementedException();

			string polyString1 = "11*x + 4";   //"x^4 + 8*x^3 + 21*x^2 + 22*x + 8";
			string polyString2 = "7*x + 2";    //"x^3 + 6*x^2 + 11*x + 6";
			string expected = "6";             //"x^2 + 3*x + 2";

			MultivariatePolynomial poly1 = MultivariatePolynomial.Parse(polyString1);
			MultivariatePolynomial poly2 = MultivariatePolynomial.Parse(polyString2);
			MultivariatePolynomial gcd= MultivariatePolynomial.GCD(poly1, poly2);

			string actual = gcd.ToString();

			TestContext.WriteLine($"GCD: \"{gcd}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.GCD({polyString1}, {polyString2});");
		}

	}
}
