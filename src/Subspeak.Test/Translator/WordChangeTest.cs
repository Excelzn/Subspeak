using Shouldly;
using Xunit;

namespace Subspeak.Test
{
    public class WordChangeTest
    {
        [Fact]
        public void CanDetectSymbols()
        {
            char.IsPunctuation('"').ShouldBeTrue();
        }
    }
}
