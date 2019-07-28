using System.Linq;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public interface ICloneable<T>
	{
		T Clone();
	}

	public static class CloneHelper<T>
	{
		public static IEnumerable<T> CloneCollection(IEnumerable<ICloneable<T>> list)
		{
			return list.Select(t => t.Clone());
		}
	}
}
