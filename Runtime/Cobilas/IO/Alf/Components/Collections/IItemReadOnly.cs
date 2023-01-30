using System;
using Cobilas.Collections;

namespace Cobilas.IO.Alf.Components.Collections {
    public interface IItemReadOnly : IReadOnlyArray<IItemReadOnly>, IDisposable, IConvertible, ICloneable {
        string Name { get; }
        bool IsRoot { get; }
        string Text { get; }
        IItemReadOnly Parent { get; }
        IItemReadOnly this[string name] { get; }

        int IndexOf(string name);
        bool Contains(string name);
    }
}
