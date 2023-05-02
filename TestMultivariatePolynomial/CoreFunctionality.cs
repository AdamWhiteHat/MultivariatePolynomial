using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using NUnit.Framework;
using ExtendedArithmetic;

namespace TestMultivariatePolynomial
{
	[TestFixture(Category = "TestCoreFunctionality")]
	public class CoreFunctionality
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[Test]
		public void TestParse000()
		{
			string[] toTest = new string[] {
				"-0",
				"0",
				"1",
				"-1",
				"X",
				"-X",
				"-X^1",
				"X^1",
				"1*X^1",
				"-1*X^1",
				"-X^0",
				"X^0",
				"1*X^0",
				"-1*X^0",
				"-1*X + 1",
				"-1*X - 1",
				"1*X - 1",
				"1*X + 1",
				"-1*X - 1*X^0"
			};

			foreach (string s in toTest)
			{
				try
				{
					MultivariatePolynomial testPolynomial = MultivariatePolynomial.Parse(s);
					TestContext.WriteLine($"Success parsing \"{s}\"");
				}
				catch (Exception ex)
				{

					Assert.Fail($"MultivariatePolynomial.Parse(string) Failed to parse the string \"{s}\"");
					TestContext.WriteLine($"{ex.GetType().Name} caught: \"{ex.Message}\".");
				}
			}
		}

		[Test]
		public void TestParse_Constant()
		{
			MultivariatePolynomial minusOne = MultivariatePolynomial.Parse("-1");
			MultivariatePolynomial one = MultivariatePolynomial.Parse("1");
			MultivariatePolynomial two = MultivariatePolynomial.Parse("2");

			Assert.IsFalse(minusOne.Terms.All(trm => trm.Variables.Any()), "minusOne.Terms.All(trm => trm.Variables.Any())");
			Assert.IsFalse(one.Terms.All(trm => trm.Variables.Any()), "one.Terms.All(trm => trm.Variables.Any())");
			Assert.IsFalse(two.Terms.All(trm => trm.Variables.Any()), "two.Terms.All(trm => trm.Variables.Any())");

			Term term1 = Term.Parse("1");
			Assert.IsFalse(term1.Variables.Any(), "Term.Parse(\"1\").Variables.Any()");
		}

		[Test]
		public void TestParse001()
		{
			string toTest = "X*Y*Z^2 + X*Y + X*Z + Y*Z - 1";

			MultivariatePolynomial testPolynomial = MultivariatePolynomial.Parse(toTest);

			string expected = toTest;//.Replace(" ", "");
			string actual = testPolynomial.ToString();//.Replace(" ", "");
			bool isMatch = (expected == actual);
			string passFailString = isMatch ? "PASS" : "FAIL";

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");

			TestContext.WriteLine($"Pass/Fail: \"{passFailString}\"");
			Assert.AreEqual(expected, actual, $"MultivariatePolynomial.Parse(\"{toTest}\").ToString();");
		}

		[Test]
		public void TestParse002()
		{
			string expected1 = "6*Y"; // "10*Z^2*X*W + 5*Z^2*Y + 2*Z*X*U + Z*Y - 2*X - V + 6*Z + T^7 + 6";
			string expected2 = "Y + 6";
			string expected3 = "6*Y + 6";

			MultivariatePolynomial result1 = MultivariatePolynomial.Multiply(MultivariatePolynomial.Parse("6"), MultivariatePolynomial.Parse("Y"));
			MultivariatePolynomial result2 = MultivariatePolynomial.Add(MultivariatePolynomial.Parse("Y"), MultivariatePolynomial.Parse("6"));
			MultivariatePolynomial result3 = MultivariatePolynomial.Add(result1, MultivariatePolynomial.Parse("6"));
			string actual1 = result1.ToString();
			string actual2 = result2.ToString();
			string actual3 = result3.ToString();

			Assert.AreEqual(expected1, actual1, $"MultivariatePolynomial.Multiply(6, Y) = {actual1}");
			Assert.AreEqual(expected2, actual2, $"MultivariatePolynomial.Add(Y, 6) = {actual2}");
			Assert.AreEqual(expected3, actual3, $"MultivariatePolynomial.Add(6*Y, 6) = {actual3}");

		}

		[Test]
		public void TestParse_ConstantPolynomial001()
		{
			string toTest = "12";

			MultivariatePolynomial testPolynomial = MultivariatePolynomial.Parse(toTest);

			string expected = toTest;//.Replace(" ", "");
			string actual = testPolynomial.ToString();//.Replace(" ", "");
			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			TestContext.WriteLine($"Pass/Fail: \"{((expected == actual) ? "PASS" : "FAIL")}\"");
			Assert.AreEqual(expected, actual, $"MultivariatePolynomial.Parse(\"{toTest}\").ToString();");
		}

		[Test]
		public void TestParse_ConstantPolynomial002()
		{
			string toTest = "-12";

			MultivariatePolynomial testPolynomial = MultivariatePolynomial.Parse(toTest);

			string expected = toTest;//.Replace(" ", "");
			string actual = testPolynomial.ToString();//.Replace(" ", "");

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			TestContext.WriteLine($"Pass/Fail: \"{((expected == actual) ? "PASS" : "FAIL")}\"");
			Assert.AreEqual(expected, actual, $"MultivariatePolynomial.Parse(\"{toTest}\").ToString();");
		}

		[Test]
		public void TestParse_ConstantPolynomial003()
		{
			string toTest = "X";

			MultivariatePolynomial testPolynomial = MultivariatePolynomial.Parse(toTest);

			string expected = toTest;//.Replace(" ", "");
			string actual = testPolynomial.ToString();//.Replace(" ", "");

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			TestContext.WriteLine($"Pass/Fail: \"{((expected == actual) ? "PASS" : "FAIL")}\"");
			Assert.AreEqual(expected, actual, $"MultivariatePolynomial.Parse(\"{toTest}\").ToString();");
		}

		[Test]
		public void TestParse_ConstantPolynomial004()
		{
			string toTest = "-X";

			MultivariatePolynomial testPolynomial = MultivariatePolynomial.Parse(toTest);

			string expected = toTest;//.Replace(" ", "");
			string actual = testPolynomial.ToString();//.Replace(" ", "");

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			TestContext.WriteLine($"Pass/Fail: \"{((expected == actual) ? "PASS" : "FAIL")}\"");
			Assert.AreEqual(expected, actual, $"MultivariatePolynomial.Parse(\"{toTest}\").ToString();");
		}

		[Test]
		public void TestParseNegativeLeadingCoefficient()
		{
			string polynomialExpected = "-8*X^2 - X + 8";
			string leadingTermExpected = "-8*X^2";
			string secondTermExpected = "-X";

			MultivariatePolynomial testPolynomial = MultivariatePolynomial.Parse(polynomialExpected);
			string polynomialActual = testPolynomial.ToString();
			string leadingTermActual = testPolynomial.Terms[0].ToString();
			string secondTermActual = testPolynomial.Terms[1].ToString();

			TestContext.WriteLine(polynomialActual);
			TestContext.WriteLine(leadingTermActual);
			TestContext.WriteLine(secondTermActual);
			TestContext.WriteLine(testPolynomial.Terms[2].ToString());

			Assert.AreEqual(polynomialExpected, polynomialActual, $"Expected: \"{polynomialExpected}\"; Actual: \"{polynomialActual}\"");
			Assert.AreEqual(leadingTermExpected, leadingTermActual, $"Expected: \"{leadingTermExpected}\"; Actual: \"{leadingTermActual}\"");
			Assert.AreEqual(secondTermExpected, secondTermActual, $"Expected: \"{secondTermExpected}\"; Actual: \"{secondTermActual}\"");
		}

		[Test]
		public void TestInstantiateZeroCoefficient()
		{
			Indeterminate indt = new Indeterminate('X', 2);
			Term term = new Term(0, new Indeterminate[] { indt });

			MultivariatePolynomial testPolynomial = new MultivariatePolynomial(new Term[] { term });
			string expected = "0";
			string actual = testPolynomial.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Expected: \"{expected}\"; Actual: \"{actual}\"");
		}

		[Test]
		public void TestInstantiateEmpty()
		{
			MultivariatePolynomial testPolynomial = new MultivariatePolynomial(new Term[0]);
			string expected = "0";
			string actual = testPolynomial.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Expected: \"{expected}\"; Actual: \"{actual}\"");
		}

		[Test]
		public void TestInstantiateNull()
		{
			MultivariatePolynomial testPolynomial = new MultivariatePolynomial(null);
			string expected = "0";
			string actual = testPolynomial.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Expected: \"{expected}\"; Actual: \"{actual}\"");
		}

		[Test]
		public void TestNumericIndeterminateSymbol()
		{
			TestDelegate testDelegate = new TestDelegate(() =>
			{
				Indeterminate indeterminate = new Indeterminate('2', 1);
			});

			Assert.Throws(typeof(ArgumentException), testDelegate);
		}

		[Test]
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

		[Test]
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

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Evaluate({polyString}) where {string.Join(" and ", indeterminants.Select(tup => $"{tup.Item1} = {tup.Item2}"))}");
		}

		[Test]
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
			result = Math.Round(result, 9);
			string actual = result.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Evaluate({polyString}) where {string.Join(" and ", indeterminants.Select(tup => $"{tup.Item1} = {tup.Item2}"))}");
		}

		[Test]
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
			string actual = result.ToString()
									.Replace("<", "(")
									.Replace(">", ")")
									.Replace(";", ",");

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Evaluate({polyString}) where x = sqrt(-1)");
		}

		[Test]
		public void TestMonomialOrdering()
		{
			string toParse = "3*X^2*Y^3 + 6*X* Y^4 + X^3*Y^2 + 4*X^5 - 6*X^2*Y + 3*X* Y*Z - 5*X^2 + 3*Y^3 + 24*X* Y - 4";
			string expected = "4*X^5 + 6*X*Y^4 + 3*X^2*Y^3 + X^3*Y^2 + 3*Y^3 - 6*X^2*Y + 3*X*Y*Z - 5*X^2 + 24*X*Y - 4";

			MultivariatePolynomial poly = MultivariatePolynomial.Parse(toParse);

			string debug = string.Join(Environment.NewLine,
				poly.Terms.Select(
					   trm =>
							$"Deg:{trm.Degree} Var.Cnt:{trm.VariableCount()} CoEff:{trm.CoEfficient} {string.Join("", trm.Variables.Select(ind => ind.Symbol).OrderBy(c => c).ToList())} => {trm.ToString()}"));
			TestContext.WriteLine($"Term Info:");
			TestContext.WriteLine(debug);

			string actual = poly.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: Monomial Ordering");
		}
	}
}
