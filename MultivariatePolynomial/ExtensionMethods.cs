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
	}
}
