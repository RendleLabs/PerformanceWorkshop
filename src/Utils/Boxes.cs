using System;
using System.Linq;

namespace Utils
{
    public static class Boxes
    {
        public class BoxedInts
        {
            internal BoxedInts() { }
            
            private static readonly object[] Boxed = Enumerable.Range(0, 128).Cast<object>().ToArray();

            public object this[int value] => value is > -1 and < 128 ? Boxed[value] : value;
        }

        public static readonly BoxedInts Integers = new();

        public static readonly object True = true;
        public static readonly object False = false;
    }
}
