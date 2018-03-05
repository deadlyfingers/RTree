using System.Collections.Generic;

namespace RTree
{
	public interface ISpatialIndex<T>
	{
		IEnumerable<T> Search();
		IEnumerable<T> Search(Envelope boundingBox);
	}
}