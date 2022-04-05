using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExtendedArithmetic;

namespace TestMultivariatePolynomial
{
	[TestClass]
	public class CoreFunctionality
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestParse001()
		{
			string toTest = "X*Y*Z^2 + X*Y + X*Z + Y*Z - 1";

			MultivariatePolynomial testPolynomial = MultivariatePolynomial.Parse(toTest);

			string expected = toTest;//.Replace(" ", "");
			string actual = testPolynomial.ToString();//.Replace(" ", "");
			bool isMatch = (expected == actual);
			string passFailString = isMatch ? "PASS" : "FAIL";
			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			TestContext.WriteLine($"Pass/Fail: \"{passFailString}\"");
			Assert.AreEqual(expected, actual, $"MultivariatePolynomial.Parse(\"{toTest}\").ToString();");
		}

		[TestMethod]
		public void TestParse_ConstantPolynomial001()
		{
			string toTest = "12";

			MultivariatePolynomial testPolynomial = MultivariatePolynomial.Parse(toTest);

			string expected = toTest;//.Replace(" ", "");
			string actual = testPolynomial.ToString();//.Replace(" ", "");
			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			TestContext.WriteLine($"Pass/Fail: \"{((expected == actual) ? "PASS" : "FAIL")}\"");
			Assert.AreEqual(expected, actual, $"MultivariatePolynomial.Parse(\"{toTest}\").ToString();");
		}

		[TestMethod]
		public void TestParse_ConstantPolynomial002()
		{
			string toTest = "-12";

			MultivariatePolynomial testPolynomial = MultivariatePolynomial.Parse(toTest);

			string expected = toTest;//.Replace(" ", "");
			string actual = testPolynomial.ToString();//.Replace(" ", "");
			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			TestContext.WriteLine($"Pass/Fail: \"{((expected == actual) ? "PASS" : "FAIL")}\"");
			Assert.AreEqual(expected, actual, $"MultivariatePolynomial.Parse(\"{toTest}\").ToString();");
		}

		[TestMethod]
		public void TestParse_ConstantPolynomial003()
		{
			string toTest = "X";

			MultivariatePolynomial testPolynomial = MultivariatePolynomial.Parse(toTest);

			string expected = toTest;//.Replace(" ", "");
			string actual = testPolynomial.ToString();//.Replace(" ", "");
			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			TestContext.WriteLine($"Pass/Fail: \"{((expected == actual) ? "PASS" : "FAIL")}\"");
			Assert.AreEqual(expected, actual, $"MultivariatePolynomial.Parse(\"{toTest}\").ToString();");
		}

		[TestMethod]
		public void TestParse_ConstantPolynomial004()
		{
			string toTest = "-X";

			MultivariatePolynomial testPolynomial = MultivariatePolynomial.Parse(toTest);

			string expected = toTest;//.Replace(" ", "");
			string actual = testPolynomial.ToString();//.Replace(" ", "");
			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			TestContext.WriteLine($"Pass/Fail: \"{((expected == actual) ? "PASS" : "FAIL")}\"");
			Assert.AreEqual(expected, actual, $"MultivariatePolynomial.Parse(\"{toTest}\").ToString();");
		}

		[TestMethod]
		public void TestParseNegativeLeadingCoefficient()
		{
			string polynomialExpected = "-8*X^2 - X + 8";
			string leadingTermExpected = "-8*X^2";
			string secondTermExpected = "-X";

			MultivariatePolynomial testPolynomial = MultivariatePolynomial.Parse(polynomialExpected);
			string polynomialActual = testPolynomial.ToString();
			string leadingTermActual = testPolynomial.Terms[2].ToString();
			string secondTermActual = testPolynomial.Terms[1].ToString();

			Assert.AreEqual(polynomialExpected, polynomialActual, $"Expected: \"{polynomialExpected}\"; Actual: \"{polynomialActual}\"");
			Assert.AreEqual(leadingTermExpected, leadingTermActual, $"Expected: \"{leadingTermExpected}\"; Actual: \"{leadingTermActual}\"");
			Assert.AreEqual(secondTermExpected, secondTermActual, $"Expected: \"{secondTermExpected}\"; Actual: \"{secondTermActual}\"");
		}

		[TestMethod]
		public void TestInstantiateZeroCoefficient()
		{
			Indeterminate indt = new Indeterminate('X', 2);
			Term term = new Term(0, new Indeterminate[] { indt });

			MultivariatePolynomial testPolynomial = new MultivariatePolynomial(new Term[] { term });
			string expected = "0";
			string actual = testPolynomial.ToString();
			Assert.AreEqual(expected, actual, $"Expected: \"{expected}\"; Actual: \"{actual}\"");
		}

		[TestMethod]
		public void TestInstantiateEmpty()
		{
			MultivariatePolynomial testPolynomial = new MultivariatePolynomial(new Term[0]);
			string expected = "0";
			string actual = testPolynomial.ToString();
			Assert.AreEqual(expected, actual, $"Expected: \"{expected}\"; Actual: \"{actual}\"");
		}

		[TestMethod]
		public void TestInstantiateNull()
		{
			MultivariatePolynomial testPolynomial = new MultivariatePolynomial(null);
			string expected = "0";
			string actual = testPolynomial.ToString();
			Assert.AreEqual(expected, actual, $"Expected: \"{expected}\"; Actual: \"{actual}\"");
		}

		[TestMethod]
		public void TestNumericIndeterminateSymbol()
		{
			Action instantiateSymbol = new Action(() =>
			{
				Indeterminate indeterminate = new Indeterminate('2', 1);
			});

			Assert.ThrowsException<ArgumentException>(instantiateSymbol);
		}

		[TestMethod]
		public void TestParseAndToString()
		{
			string[] toTest = new string[]
			{
				"a*b*c*d - w*x*y*z",
				"X - 1",
				"2*X^4 + 13*X^3 + 29*X^2 + 29*X + 13",
				"w^2*x*y + w*x + w*y + 1",
				"x*y + x + y + 1",
				"7*x*y + 14*y - 3*x - 1",
				"4*x*y - 2*y - 3*x - 1",
				"4*x*y - 11*y - 12*x",
				"12*x",
				"x",
				"1",
				"0"
			};

			int counter = 1;
			foreach (string testString in toTest)
			{
				MultivariatePolynomial testPolynomial = MultivariatePolynomial.Parse(testString);
				string expected = testString;//.Replace(" ", "");
				string actual = testPolynomial.ToString();//.Replace(" ", "");
				bool isMatch = (expected == actual);
				string passFailString = isMatch ? "PASS" : "FAIL";
				string inputOutputString = isMatch ? $"Polynomial: \'{testPolynomial.ToString()}\"" : $"Expected: \"{expected}\"; Actual: \"{actual}\"";
				TestContext.WriteLine($"Test #{counter} => Pass/Fail: \"{passFailString}\" {inputOutputString}");
				Assert.AreEqual(expected, actual, $"Test #{counter}: MultivariatePolynomial.Parse(\"{testString}\").ToString();");

				counter++;
			}
		}

		[TestMethod]
		public void TestEvaluate_BigInteger()
		{
			string expected = "8551120982818029391";

			string polyString = "2*x^4 + 13*y^3 + 29*x^2 + 29*y + 13";
			MultivariatePolynomial poly = MultivariatePolynomial.Parse(polyString);
			List<Tuple<char, BigInteger>> indeterminants = new List<Tuple<char, BigInteger>>()
			{
				new Tuple<char, BigInteger>('x', 45468),
				new Tuple<char, BigInteger>('y', 63570),
			};

			BigInteger result = poly.Evaluate(indeterminants);
			string actual = result.ToString();

			TestContext.WriteLine($"Result: \"{actual}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Evaluate({polyString}) where {string.Join(" and ", indeterminants.Select(tup => $"{tup.Item1} = {tup.Item2}"))}");
		}

		[TestMethod]
		public void TestEvaluate_Double()
		{
			string expected = "15565";

			string polyString = "10000*x^4 + 125*y^3 + 500*x^2 + 75*y + 13";
			MultivariatePolynomial poly = MultivariatePolynomial.Parse(polyString);
			List<Tuple<char, double>> indeterminants = new List<Tuple<char, double>>()
			{
				new Tuple<char, double>('x', 1.1d),
				new Tuple<char, double>('y', 1.2d),
			};

			double result = poly.Evaluate(indeterminants);
			string actual = result.ToString();

			TestContext.WriteLine($"Result: \"{actual}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Evaluate({polyString}) where {string.Join(" and ", indeterminants.Select(tup => $"{tup.Item1} = {tup.Item2}"))}");
		}

		[TestMethod]
		public void TestEvaluate_Complex()
		{
			string expected = "(0, 0)";

			string polyString = "x^2 + 1";
			MultivariatePolynomial poly = MultivariatePolynomial.Parse(polyString);
			List<Tuple<char, Complex>> indeterminants = new List<Tuple<char, Complex>>()
			{
				new Tuple<char, Complex>('x', Complex.ImaginaryOne)
			};

			Complex evaluated = poly.Evaluate(indeterminants);
			Complex result = new Complex(Math.Round(evaluated.Real, 1), Math.Round(evaluated.Imaginary, 1));
			string actual = result.ToString();

			TestContext.WriteLine($"Result: \"{actual}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Evaluate({polyString}) where x = sqrt(-1)");
		}

		[TestMethod]
		public void TestMonomialOrdering()
		{
			string toParse = "3*X^2*Y^3 + 6*X* Y^4 + X^3*Y^2 + 4*X^5 - 6*X^2*Y + 3*X* Y*Z - 5*X^2 + 3*Y^3 + 24*X* Y - 4";
			string expected = "4*X^5 + 6*X*Y^4 + 3*X^2*Y^3 + X^3*Y^2 + 3*Y^3 - 6*X^2*Y + 3*X*Y*Z - 5*X^2 + 24*X*Y - 4";

			MultivariatePolynomial poly = MultivariatePolynomial.Parse(toParse);
			string actual = poly.ToString();

			TestContext.WriteLine($"Result: \"{actual}\".");
			Assert.AreEqual(expected, actual, $"Test of: Monomial Ordering");
		}
	}
}
