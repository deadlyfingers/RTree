using System.Collections.Generic;

namespace RTree
{
	public interface ISpatialDatabase<T> : ISpatialIndex<T>
	{
		void Insert(T item);
		void Delete(T item);
		void Clear();

		void BulkLoad(IEnumerable<T> items);
	}
}