using System;
using System.Diagnostics.CodeAnalysis;

namespace Parsers
{
    public class ReadParser
    {
        public bool TryGetField(ReadOnlySpan<char> span, int index,
            [NotNullWhen(true)] out string? value)
        {
            const char comma = ',';

            for (int i = 0; i < index; i++)
            {
                int next = span.IndexOf(comma) + 1;
                if (next == 0)
                {
                    value = null;
                    return false;
                }

                span = span[next..];
            }

            int end = span.IndexOf(comma);
            if (end == -1)
            {
                value = null;
                return false;
            }

            span = span[..end];
            value = new string(span);
            return true;
        }
    }
}