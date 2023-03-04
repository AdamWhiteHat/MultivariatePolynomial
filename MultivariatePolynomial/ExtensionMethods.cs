using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace ExtendedArithmetic
{
	public static class BigIntegerExtensionMethods
	{
		public static BigInteger Clone(this BigInteger source)
		{
			return new BigInteger(source.ToByteArray());
		}

		/// <summary>Returns the square root of a BigInteger.</summary>
		public static BigInteger SquareRoot(this BigInteger input)
		{
			if (input.IsZero) return new BigInteger(0);
			if (input.IsOne) return new BigInteger(1);

			BigInteger n = new BigInteger(0);
			BigInteger p = new BigInteger(0);
			BigInteger low = new BigInteger(0);
			BigInteger high = BigInteger.Abs(input);

			while (high > low + 1)
			{
				n = (high + low) >> 1;
				p = n * n;

				if (input < p)
					high = n;
				else if (input > p)
					low = n;
				else
					break;
			}
			return input == p ? n : low;
		}

		/// <summary>
		/// Returns all divisors of an integer, including 1 and itself.
		/// </summary>
		public static List<BigInteger> GetAllDivisors(this BigInteger value)
		{
			BigInteger n = value;

			if (BigInteger.Abs(n) == 1)
			{
				return new List<BigInteger> { n };
			}

			List<BigInteger> results = new List<BigInteger>();
			if (n.Sign == -1)
			{
				results.Add(-1);
				n = n * BigInteger.MinusOne;
			}
			for (BigInteger i = 1; i * i < n; i++)
			{
				if (n % i == 0)
				{
					results.Add(i);
				}
			}
			for (BigInteger i = n.SquareRoot(); i >= 1; i--)
			{
				if (n % i == 0)
				{
					results.Add(n / i);
				}
			}
			return results;
		}
	}
}
