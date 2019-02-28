using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;

namespace MavenSecurities
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(MergeLists(new []{1, 1,1,1,1}));
        }


        private static void print(int[] list)
        {
            foreach(var e in list)
                Console.Write($"{e}, ");
            Console.WriteLine();
        }
        public static long MergeLists(int[] lists)
        {
            var linkedList = new LinkedList<int>(lists.OrderBy(e => e));
            long totalMergeTime = 0;

            print(lists);
            while (linkedList.Count > 1)
            {
                var first = linkedList.First;
                var second = first.Next;
                var nextMergedListMaxSize = first.Value + second.Value;
                totalMergeTime += nextMergedListMaxSize; //merge time in ms equals to total elements 
                
                Console.WriteLine($"Merging {first.Value} and {second.Value} with total {totalMergeTime}");
                linkedList.RemoveFirst();
                linkedList.RemoveFirst();
                
                
                if (!linkedList.Any())
                    return totalMergeTime;
                
                //find a place for the result
                var node = linkedList.First;
                while (node != null && node.Value < nextMergedListMaxSize)
                    node = node.Next;
                
                if (node == null)
                    linkedList.AddLast(nextMergedListMaxSize);
                else
                    linkedList.AddBefore(node, nextMergedListMaxSize);
                
                print(linkedList.ToArray());
            }

            return totalMergeTime;
        }
    }
}