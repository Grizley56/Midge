using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.Server.Extensions
{
	public static class CollectionExtensions
	{
		public static IReadOnlyCollection<T> AsReadOnly<T>(this ICollection<T> source)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			return source as IReadOnlyCollection<T> ?? new ReadOnlyCollectionAdapter<T>(source);
		}

		sealed class ReadOnlyCollectionAdapter<T> : IReadOnlyCollection<T>
		{
			private readonly ICollection<T> _source;
			public ReadOnlyCollectionAdapter(ICollection<T> source) => _source = source;
			public int Count => _source.Count;
			public IEnumerator<T> GetEnumerator() => _source.GetEnumerator();
			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}
	}
}
