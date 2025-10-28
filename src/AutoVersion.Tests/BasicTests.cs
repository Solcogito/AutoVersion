using Xunit;
using AutoVersion.Core;

namespace AutoVersion.Tests
{
    public class BasicTests
    {
        [Fact]
        public void Version_IsDefined()
        {
            Assert.Equal("0.0.0", VersionInfo.GetVersion());
        }
    }
}
