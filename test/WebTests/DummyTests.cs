using Xunit;

namespace WebTests
{
    public class DummyTests
    {
        [Fact]
        public void ShouldPass()
        {
            Assert.True(true);
        }

        [Fact]
        public void ShouldAlsoPass()
        {
            Assert.False(false);
        }
    }
}
