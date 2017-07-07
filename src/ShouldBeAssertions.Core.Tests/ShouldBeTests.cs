using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ShouldBeAssertions.Core.Tests
{
    
    public class ShouldBeTests
    {
        public IEnumerable<int> GetInts(params int[] ints)
        {
            for (int i = 0; i < ints.Length; i++)
            {
                yield return ints[i];
            }
        }

        [Fact]
        public void ShouldBeThrowsErrorWhenListLengthNotEqual()
        {
            var actual = GetInts(0, 1).ToList();
            Assert.Throws<ShouldBeException>(() =>
            {
                actual.ShouldLookLike(new[] {0, 1, 2});
            });
        }
    }
}