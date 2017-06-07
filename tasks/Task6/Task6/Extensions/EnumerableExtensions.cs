using System;
using System.Collections.Generic;

namespace Task6.Extensions
{
    public static class EnumerableExtensions
    {
        /* Thank you, Sir John Skeet
         * https://stackoverflow.com/a/648240
         */
        public static T RandomElement<T>(this IEnumerable<T> source, Random rng)
        {
            var current = default(T);
            var count = 0;
            foreach (var element in source)
            {
                count++;
                if (rng.Next(count) == 0)
                {
                    current = element;
                }
            }
            if (count == 0)
            {
                throw new InvalidOperationException("Sequence was empty");
            }
            return current;
        }

    }
}
