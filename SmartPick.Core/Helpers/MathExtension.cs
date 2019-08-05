using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartPick.Core.Helpers
{
    public static class MathExtension
    {
        private static readonly int[] Primes = {2, 3, 5, 7, 11, 13, 17, 19, 23, 29};

        public static bool IsPrime(int number)
        {
            return true;
        }

        public static int[] DecomposeInteger(int input)
        {
            var primes = new List<int>();
            var ind = 0;
            while (input > 1)
            {
                if(input % Primes[ind] == 0)
                {
                    input /= Primes[ind];
                    primes.Add(Primes[ind]);
                    ind = 0;
                }
                else
                {
                    ind++;
                }
            }

            return primes.OrderByDescending(c  => c).ToArray();
        }
    }
}
