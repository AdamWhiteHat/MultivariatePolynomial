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
			string augend = "X^2 + 2*X - 1";
			string addend = "2*X^2 - 3*X + 6";

			string expected = "3*X^2 - X + 5";

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
			string minuend = "3*X";
			string subtrahend = "X + 2";

			string expected = "2*X - 2";

			MultivariatePolynomial polyMinuend = MultivariatePolynomial.Parse(minuend);
			MultivariatePolynomial polySubtrahend = MultivariatePolynomial.Parse(subtrahend);

			MultivariatePolynomial difference = MultivariatePolynomial.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Subtract: \"{difference}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Subtract: ({minuend}) - ({subtrahend})");
		}

		[TestMethod]
		public void TestSubtract2()
		{
			string minuend = "36*X*Y + 6*X + 6*Y + 1";
			string subtrahend = "36*X*Y + 1";

			string expected = "6*X + 6*Y";

			MultivariatePolynomial polyMinuend = MultivariatePolynomial.Parse(minuend);
			MultivariatePolynomial polySubtrahend = MultivariatePolynomial.Parse(subtrahend);

			MultivariatePolynomial difference = MultivariatePolynomial.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Difference: \"{difference}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Subtract: ({minuend}) - ({subtrahend})");
		}

		[TestMethod]
		public void TestSubtract3()
		{
			string minuend = "2*X^3 + 2*X - 1";
			string subtrahend = "2*X^2 - 5*X - 6";

			string expected = "2*X^3 - 2*X^2 + 7*X + 5";

			MultivariatePolynomial polyMinuend = MultivariatePolynomial.Parse(minuend);
			MultivariatePolynomial polySubtrahend = MultivariatePolynomial.Parse(subtrahend);

			MultivariatePolynomial difference = MultivariatePolynomial.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Subtract: \"{difference}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Subtract: ({minuend}) - ({subtrahend})");
		}

		[TestMethod]
		public void TestSubtract4()
		{
			string minuend = "3*X^2*Y^3 + 2*X^3*Y^2 + 6*X*Y^2 + 4*X^3 - 6*X^2*Y + 3*X*Y - 2*X^2 + 12*X - 6";
			string subtrahend = "X^3*Y^2 + 3*X^2 - 3*Y^2 - 12*X - 2";

			string expected = "3*X^2*Y^3 + X^3*Y^2 + 4*X^3 + 6*X*Y^2 - 6*X^2*Y + 3*Y^2 - 5*X^2 + 3*X*Y + 24*X - 4";

			MultivariatePolynomial polyMinuend = MultivariatePolynomial.Parse(minuend);
			MultivariatePolynomial polySubtrahend = MultivariatePolynomial.Parse(subtrahend);

			MultivariatePolynomial difference = MultivariatePolynomial.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Subtract: \"{difference}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Subtract: ({minuend}) - ({subtrahend})");
		}

		[TestMethod]
		public void TestSubtract5()
		{
			string minuend = "504*X*Y*Z^2 + 216*X*Y - 42*X*Z^2 - 18*X + 84*Y*Z^2 + 36*Y - 7*Z^2 - 3";
			string subtrahend = "X*Y*Z^2 + 42*X*Z^2 - 8*X - X^2 - 3";

			string expected = "503*X*Y*Z^2 + 84*Y*Z^2 - 84*X*Z^2 + X^2 - 7*Z^2 + 216*X*Y + 36*Y - 10*X";

			MultivariatePolynomial polyMinuend = MultivariatePolynomial.Parse(minuend);
			MultivariatePolynomial polySubtrahend = MultivariatePolynomial.Parse(subtrahend);

			MultivariatePolynomial difference = MultivariatePolynomial.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Subtract: \"{difference}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Subtract: ({minuend}) - ({subtrahend})");
		}

		[TestMethod]
		public void TestMultiply1()
		{
			string lhs = "6*X + 1";
			string rhs = "6*Y + 1";
			string expected = "36*X*Y + 6*X + 6*Y + 1";

			MultivariatePolynomial polylhs = MultivariatePolynomial.Parse(lhs);
			MultivariatePolynomial polyrhs = MultivariatePolynomial.Parse(rhs);

			MultivariatePolynomial polyProdcut = MultivariatePolynomial.Multiply(polylhs, polyrhs);

			string actual = polyProdcut.ToString();

			TestContext.WriteLine($"Product: \"{actual}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Multiply({lhs}, {rhs});");
		}

		[TestMethod]
		public void TestMultiplySameSymbols()
		{
			string lhs = "6*X + 1";
			string rhs = "6*X - 1";
			string expected = "36*X^2 - 1";

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
			string dividend = "36*X*Y + 6*X + 6*Y + 1";
			string divisor = "6*X + 1";

			string expected = "6*Y + 1";

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
			string dividend = "2*X*Y^2 + 3*X*Y + 4*Y^2 + 6*Y";
			string divisor = "X + 2";

			string expected = "2*Y^2 + 3*Y";

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
			string polyBaseString = "2*X*Y^2 - 1";
			int exponent = 2;

			string expected = "4*X^2*Y^4 - 4*X*Y^2 + 1";

			MultivariatePolynomial polyBase = MultivariatePolynomial.Parse(polyBaseString);
			MultivariatePolynomial power = MultivariatePolynomial.Pow(polyBase, exponent);

			string actual = power.ToString();

			TestContext.WriteLine($"Pow: \"{power}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Pow({polyBaseString}, {exponent});");
		}

		[TestMethod]
		public void TestGetDerivative1()
		{
			string polyString = "132*X*Y + 77*X + 55*Y + 1";
			string expected = "132*Y + 77";

			MultivariatePolynomial poly = MultivariatePolynomial.Parse(polyString);
			MultivariatePolynomial derivative = MultivariatePolynomial.GetDerivative(poly, 'X');

			string actual = derivative.ToString();

			TestContext.WriteLine($"Derivative: \"{derivative}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.GetDerivative({polyString});");
		}

		[TestMethod]
		public void TestGetDerivative2()
		{
			string polyString = "4*X^2*Y^4 - 4*X*Y^2 + 1";
			string expected = "8*X*Y^4 - 4*Y^2";

			MultivariatePolynomial poly = MultivariatePolynomial.Parse(polyString);
			MultivariatePolynomial derivative = MultivariatePolynomial.GetDerivative(poly, 'X');

			string actual = derivative.ToString();

			TestContext.WriteLine($"Derivative: \"{derivative}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.GetDerivative({polyString});");
		}

		[TestMethod]
		public void TestGCD()
		{
			//throw new NotImplementedException();

			string polyString1 = "X^4 + 8*X^3 + 21*X^2 + 22*X + 8";     //"X^4 + 8*X^3 + 21*X^2 + 22*X + 8";
			string polyString2 = "X^3 + 6*X^2 + 11*X + 6";              //"X^3 + 6*X^2 + 11*X + 6";
			string expected = "X^2 + 3*X + 2";                          //"X^2 + 3*X + 2";

			MultivariatePolynomial poly1 = MultivariatePolynomial.Parse(polyString1);
			MultivariatePolynomial poly2 = MultivariatePolynomial.Parse(polyString2);
			MultivariatePolynomial gcd = MultivariatePolynomial.GCD(poly1, poly2);

			string actual = gcd.ToString();

			TestContext.WriteLine($"GCD: \"{gcd}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.GCD({polyString1}, {polyString2});");
		}

	}
}
