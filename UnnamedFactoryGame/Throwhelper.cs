using System;
using System.Runtime.CompilerServices;

namespace Cosmi;

internal static class Throwhelper
{
    public static void Throw_InvalidOperationException(string message)
    {
        throw new InvalidOperationException(message);
    }

    public static void Throw_ArgumentException(string message, string? parameterName = default)
    {
        throw new ArgumentException(message, parameterName);
    }
}
