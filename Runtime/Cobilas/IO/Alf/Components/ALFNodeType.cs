namespace Cobilas.IO.Alf.Components {
    public enum ALFNodeType : byte {
        UnknownElement = 0,
        EmptyElement = 1,
        OpenElement = 2,
        ClosedElement = 3,
        OpenTextElement = 4,
        ClosedTextElement = 5,
        TextElement = 6,
        SingleLineElement = 7,
        SingleLineComment = 8
    }
}
