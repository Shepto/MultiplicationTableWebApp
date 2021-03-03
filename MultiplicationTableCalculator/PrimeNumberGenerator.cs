using System.Collections;
using System.Linq;

namespace MultiplicationTableCalculator
{
    /// <summary>
    /// Class for generating prime numbers
    /// </summary>
    public static class PrimeNumberGenerator
    {
        /// <summary>
        /// Method for getting number of prime numbers using Sieve of Eratosthenes algotithm
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public static int[] getPrimeNumbers(int N)
        {
            int totalPrimes = 0;
            int sizeFactor = 2;
            int s = N * sizeFactor;

            BitArray arr = new BitArray(s);

            while(totalPrimes < N)
            {
                arr = sieveOfEratosthenes(s);
                totalPrimes = (from bool m in arr
                               where m
                               select m).Count();
                sizeFactor++;
                s = N * sizeFactor;
            }

            return CountPrimes(arr, N);
        }

        /// <summary>
        /// Sieve of Eratosthenes algorithm for finding prime numbers (bit array where 1 indicates prime number)
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        private static BitArray sieveOfEratosthenes(int N)
        {
            BitArray primes = new BitArray(N, true);
            primes.Set(0, false);
            primes.Set(1, false);

            for (int i = 0; i * i < N; i++)
            {
                if (primes.Get(i))
                {
                    for (int j = i * i; j < N; j += i)
                    {
                        primes.Set(j, false);
                    }
                }
            }

            return primes;
        }

        /// <summary>
        /// Method for getting all primes numbers from bit array
        /// </summary>
        /// <param name="primes"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        private static int[] CountPrimes(BitArray primes, int N)
        {
            int[] result = new int[N];
            int primesCount = 0;

            for (int i = 0; i < primes.Length; i++)
            {
                if(primesCount == N)
                {
                    break;
                }

                if (primes.Get(i))
                {
                    result[primesCount] = i;
                    primesCount++;
                }
            }

            return result;
        }
    }
}