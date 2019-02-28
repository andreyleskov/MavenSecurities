using System;
using TaskOne;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    //up to 10 test cases 
    public class UnitTest1
    {
        private ITestOutputHelper _helper;

        public UnitTest1(ITestOutputHelper helper)
        {
            _helper = helper;
        }
        [Fact]
        public void Test1()
        {
            var counter = new ZeroSumCounter();
            var array = new [] {2, -2, 3, 0, 4, -7};
            var total = ZeroSumCounter.FindZeroSumSubarrays(array);

            foreach (var arrayBoundries in total)
            {
                _helper.WriteLine($"found array [{arrayBoundries.Item1}:{arrayBoundries.Item2}]");
                var zeroSubArray = "";
                for (int i = arrayBoundries.Item1; i <= arrayBoundries.Item2; i++)
                    zeroSubArray += array[i] + " ";
                
                _helper.WriteLine(zeroSubArray);
            }

        }
    }
    
   
}