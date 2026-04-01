using System;

namespace Cosmi;

internal static class Throwhelper
{
    public static void Throw_InvalidOperationException(string message)
    {
        throw new InvalidOperationException(message);
    }
}
