using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolynomialLibrary;

namespace TestMultivariatePolynomial
{
	[TestClass]
	public class CoreFunctionality
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestParseAndToString()
		{
			string[] toTest = new string[]
			{
				"a*b*c*d - w*x*y*z",
				"X - 1",
				"2*X^4 + 13*X^3 + 29*X^2 + 29*X + 13",
				"w^2*x*y + w*x + w*y + 1",
				"144*x*y + 12*x + 12*y + 1",
				"144*x*y - 12*x + 12*y - 1",
				"144*x*y - 12*x - 12*y - 1",
				"144*x*y - 12*x - 12*y",
				"144*x",
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
		public void TestEvaluate()
		{
			BigInteger expected = BigInteger.Parse("104053773133");

			string polyString = "36*x*y - 6*x - 6*y + 1";
			MultivariatePolynomial poly = MultivariatePolynomial.Parse(polyString);
			List<Tuple<char, BigInteger>> indeterminants = new List<Tuple<char, BigInteger>>()
			{
				new Tuple<char, BigInteger>('x', 45468),
				new Tuple<char, BigInteger>('y', 63570),
			};

			BigInteger actual = poly.Evaluate(indeterminants);

			TestContext.WriteLine($"Result: \"{actual}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.Evaluate({polyString}) where {string.Join(" and ", indeterminants.Select(tup => $"{tup.Item1} = {tup.Item2}"))}");
		}
	}
}
