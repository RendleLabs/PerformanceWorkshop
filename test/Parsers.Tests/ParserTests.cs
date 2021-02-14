using System;
using System.Text;
using Xunit;

namespace Parsers.Tests
{
    public class ParserTests
    {
        private const string Line =
            "6/1/44 12:00,Los Alamos,NM,Disk,1+ Hour,Disk hovers over Los Alamos during the development of the atomic bombs; pursued by planes.,5/12/09";
        
        [Fact]
        public void SplitParserWorks()
        {
            var target = new SplitParser();
            Assert.True(target.TryGetField(Line, 2, out var actual));
            Assert.Equal("NM", actual);
        }
    }
}
