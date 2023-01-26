using System;
using Cobilas.Collections;

namespace Cobilas.IO.Alf.Components.Collections {
    public interface IItemReadOnly : IReadOnlyArray<IItemReadOnly>, IConvertible, ICloneable {
        string Name { get; }
        bool IsRoot { get; }
        string Text { get; }
        IItemReadOnly Parent { get; }
    }
}
