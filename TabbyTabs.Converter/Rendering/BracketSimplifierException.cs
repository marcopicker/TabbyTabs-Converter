using System;

namespace TabbyTabs.Converter.Rendering;

public class BracketSimplifierException : Exception
{
    public BracketSimplifierException() {}
    public BracketSimplifierException(string message) : base(message) {}
    public BracketSimplifierException(string message, Exception innerException) : base(message, innerException) {}
    public BracketSimplifierException(string input, int index, string message) : base(CreateMessage(input, index, message)) {}
    public BracketSimplifierException(string input, int index, string message, Exception innerException) : base(CreateMessage(input, index, message), innerException) {}

    private static string CreateMessage(string input, int index, string message)
    {
        return $"A {nameof(BracketSimplifierException)} has occurred for the input \"{input}\" at index {index}: {message}";
    }
}