using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskOne
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class ZeroSumCounter
    {
        public static IEnumerable<Tuple<int,int>> FindZeroSumSubarrays(int[] array)
        {
            var knownSums = new Dictionary<long, List<int>>();
            knownSums.Add(0,new List<int>(){-1});
            
            long sum = 0;
            for (int currentIndex = 0; currentIndex < array.Length; currentIndex++)
            {
                sum += array[currentIndex];
                List<int> indexes;
                if (knownSums.TryGetValue(sum, out indexes))
                {
                    foreach(var index in indexes)
                        yield return Tuple.Create(index+1, currentIndex);
                    
                    indexes.Add(currentIndex);
                }
                else
                 knownSums[sum] = new List<int>(){currentIndex};
            }
        }

        public static int FindZeroSubarraysCount(int[] array)
        {
            return FindZeroSumSubarrays(array).Count();
        }
        
    }
}