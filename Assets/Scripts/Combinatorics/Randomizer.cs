using System;
using System.Collections;
using System.Collections.Generic;

namespace Facet.Combinatorics
{
    public class Randomizer<T>
    {
        public T[] Shuffle(T[] array)
        {
            Random rng = new Random(DateTime.Now.Second);
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }

            return array;
        }
    }
}
