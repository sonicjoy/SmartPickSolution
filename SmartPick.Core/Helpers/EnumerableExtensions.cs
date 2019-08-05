using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartPick.Core.Models;

namespace SmartPick.Core.Helpers
{
    public static class EnumerableExtensions
    {            
        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, int count)
        {
            var array = source.ToArray();
            return array.Length == count ? array : ShuffleInternal(array, Math.Min(count, array.Length)).Take(count);
        }

        /// <summary>
        /// Fisher–Yates shuffle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static IEnumerable<T> ShuffleInternal<T>(T[] array, int count)
        {
            var rand = new Random();
            for (var n = 0; n < count; n++)
            {
                var k = rand.Next(n, array.Length);
                var temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }

            return array;
        }
    }
}
