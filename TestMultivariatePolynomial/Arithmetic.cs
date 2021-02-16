using System;
using System.Collections.Generic;
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

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Add({augend}, {addend});");
		}

		[TestMethod]
		public void TestAddUnlikeTerms()
		{
			string augend = "X^2";
			string addend = "6";

			string expected = "X^2 + 6";

			MultivariatePolynomial polyAugend = MultivariatePolynomial.Parse(augend);
			MultivariatePolynomial polyAddend = MultivariatePolynomial.Parse(addend);

			MultivariatePolynomial sum = MultivariatePolynomial.Add(polyAugend, polyAddend);
			string actual = sum.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Add({augend}, {addend});");
		}

		[TestMethod]
		public void TestSubtractUnlikeTerms()
		{
			string minuend = "X^2";
			string subtrahend = "6";

			string expected = "X^2 - 6";

			MultivariatePolynomial polyMinuend = MultivariatePolynomial.Parse(minuend);
			MultivariatePolynomial polySubtrahend = MultivariatePolynomial.Parse(subtrahend);

			MultivariatePolynomial difference = MultivariatePolynomial.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Subtract: ({minuend}) - ({subtrahend})");
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

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
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

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
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

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
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

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
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

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
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

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
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

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
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

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
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

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
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

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
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

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
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

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
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

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.GCD({polyString1}, {polyString2});");
		}

		[TestMethod]
		public void TestFunctionalComposition001()
		{
			MultivariatePolynomial indeterminateX = MultivariatePolynomial.Parse("6*X + 1");

			MultivariatePolynomial zero = MultivariatePolynomial.Parse("0");
			MultivariatePolynomial minusOne = MultivariatePolynomial.Parse("-1");
			MultivariatePolynomial one = MultivariatePolynomial.Parse("1");
			MultivariatePolynomial X = MultivariatePolynomial.Parse("X");

			MultivariatePolynomial even = MultivariatePolynomial.Parse("2*Y");
			MultivariatePolynomial odd = MultivariatePolynomial.Parse("2*X + 1");


			string expecting1 = "1";
			string expecting2 = "6*X + 1";
			string expecting3 = "12*Y + 1";
			//string expecting4 = "";
			//string expecting5 = "";		


			string actual1 = MultivariatePolynomial.Pow(indeterminateX, 0).ToString();
			string actual2 = MultivariatePolynomial.Pow(indeterminateX, 1).ToString();
			string actual3 = indeterminateX.FunctionalComposition(new List<Tuple<char, MultivariatePolynomial>>() { new Tuple<char, MultivariatePolynomial>('X', even) }).ToString();
			//string actual4 = composition4.ToString();
			//string actual5 = composition5.ToString();

			Assert.AreEqual(expecting1, actual1);
			Assert.AreEqual(expecting2, actual2);
			Assert.AreEqual(expecting3, actual3);
			//Assert.AreEqual(expecting4, actual4);
			//Assert.AreEqual(expecting5, actual5);
		}
		[TestMethod]
		public void TestFunctionalComposition002()
		{
			MultivariatePolynomial indeterminateX = MultivariatePolynomial.Parse("6*X + 1");
			MultivariatePolynomial indeterminateY = MultivariatePolynomial.Parse("6*Y - 1");
			MultivariatePolynomial polyn = MultivariatePolynomial.Multiply(indeterminateX, indeterminateY);

			MultivariatePolynomial zero = MultivariatePolynomial.Parse("0");
			MultivariatePolynomial minusOne = MultivariatePolynomial.Parse("-1");
			MultivariatePolynomial six = MultivariatePolynomial.Parse("6");

			MultivariatePolynomial even = MultivariatePolynomial.Parse("2*Y");
			MultivariatePolynomial odd = MultivariatePolynomial.Parse("2*X + 1");

			MultivariatePolynomial inversePolyn = MultivariatePolynomial.Multiply(polyn, minusOne); // -36*X*Y + 6*X - 6*Y + 1

			List<Tuple<char, MultivariatePolynomial>> indeterminantsOddEven = new List<Tuple<char, MultivariatePolynomial>>()
			{
				new Tuple<char, MultivariatePolynomial>('X', odd),
				new Tuple<char, MultivariatePolynomial>('Y', even),
			};

			List<Tuple<char, MultivariatePolynomial>> indeterminantsConstants = new List<Tuple<char, MultivariatePolynomial>>()
			{
				new Tuple<char, MultivariatePolynomial>('X', zero),
				new Tuple<char, MultivariatePolynomial>('Y', minusOne),
			};

			List<Tuple<char, MultivariatePolynomial>> indeterminantsInverse = new List<Tuple<char, MultivariatePolynomial>>()
			{
				new Tuple<char, MultivariatePolynomial>('X', inversePolyn),
				new Tuple<char, MultivariatePolynomial>('Y', inversePolyn),
			};

			List<Tuple<char, MultivariatePolynomial>> indeterminantSix = new List<Tuple<char, MultivariatePolynomial>>()
			{
				new Tuple<char, MultivariatePolynomial>('X', six)
			};

			MultivariatePolynomial composition1 = polyn.FunctionalComposition(indeterminantsOddEven); // 36*X*Y + 6*Y - 6*X - 1
			MultivariatePolynomial composition2 = polyn.FunctionalComposition(indeterminantsInverse);
			MultivariatePolynomial composition3 = polyn.FunctionalComposition(indeterminantsConstants);
			MultivariatePolynomial composition4 = minusOne.FunctionalComposition(indeterminantSix);
			MultivariatePolynomial composition5 = indeterminateX.FunctionalComposition(indeterminantsConstants);

			string expecting1 = "144*X*Y - 12*X + 84*Y - 7";
			string expecting2 = "46656*X^2*Y^2 + 1296*X^2 - 15552*X^2*Y + 15552*X*Y^2 + 432*X - 5184*X*Y + 1296*Y^2 - 432*Y + 35";
			string expecting3 = "-7";
			string expecting4 = "-1";
			string expecting5 = "1";

			string actual1 = composition1.ToString();
			string actual2 = composition2.ToString();
			string actual3 = composition3.ToString();
			string actual4 = composition4.ToString();
			string actual5 = composition5.ToString();

			//Assert.AreEqual(expecting1, actual1);
			//Assert.AreEqual(expecting2, actual2);
			Assert.AreEqual(expecting3, actual3);
			Assert.AreEqual(expecting4, actual4);
			Assert.AreEqual(expecting5, actual5);
		}
	}
}
