using System;
using System.Numerics;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public interface IMultivariatePolynomial : ICloneable<IMultivariatePolynomial>,
		IEquatable<IMultivariatePolynomial>, IEqualityComparer<IMultivariatePolynomial>
	{
		int Degree { get; }
		ITerm[] Terms { get; }

		BigInteger this[int degree]
		{
			get;
			set;
		}

		BigInteger Evaluate(BigInteger indeterminateValue);
	}

}
