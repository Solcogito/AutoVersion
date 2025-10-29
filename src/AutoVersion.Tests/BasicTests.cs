using Xunit;
using Solcogito.AutoVersion.Core;

namespace Solcogito.AutoVersion.Tests
{
    public class BasicTests
    {
        [Fact]
        public void VersionModel_Parses_And_Bumps_Correctly()
        {
            var v = VersionModel.Parse("1.2.3");
            var next = v.Bump("patch");
            Assert.Equal("1.2.4", next.ToString());
        }

        [Fact]
        public void VersionModel_PreRelease_Works()
        {
            var v = new VersionModel(1, 0, 0, "alpha.1");
            Assert.Equal("1.0.0-alpha.1", v.ToString());
        }
    }
}
