using System;
using Xunit;

namespace TaskThree.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
        }
    }
}


//Example test:    (4, '1B 2C,2D 4D', '2B 2D 3D 4D 4A') 
//WRONG ANSWER  (got 0, -1 expected 1,1) 
//
//Example test:    (3, '1A 1B,2C 2C', '1B') 
//WRONG ANSWER  (got 0, -1 expected 0,1) 
//
//Example test:    (12, '1A 2A,12A 12A', '12A') 