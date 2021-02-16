using System.Diagnostics.CodeAnalysis;

namespace Parsers
{
    public class SplitParser
    {
        public bool TryGetField(string line, int index,
            [NotNullWhen(true)] out string? value)
        {
            var parts = line.Split(',');

            if (parts.Length <= index)
            {
                value = default;
                return false;
            }
            
            value = parts[index];
            return true;
        }
    }
}