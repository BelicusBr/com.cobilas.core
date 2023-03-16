using System.Collections.Generic;

namespace Cobilas.Collections {
    public interface IReadOnlyArray<T> : IReadOnlyArray, IEnumerable<T> {
        T this[int index] { get; }
    }
}