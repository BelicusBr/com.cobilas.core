using System.Collections;

namespace Cobilas.Collections {
    public interface IReadOnlyArray : IEnumerable {
        int Count { get; }
        object this[int index] { get; }
    }
}
