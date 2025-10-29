// ============================================================================
// File:        VersionModelTests.cs
// Project:     AutoVersion Lite (Test Suite)
// Version:     0.1.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Unit tests for VersionModel class. Verifies parsing, string conversion,
//   and comparison logic for various semantic version scenarios.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using Xunit;
using Solcogito.AutoVersion.Core;

namespace Solcogito.AutoVersion.Tests
{
    public class VersionModelTests
    {
        /// <summary>
        /// Ensures valid versions parse correctly into component fields.
        /// </summary>
        [Fact]
        public void Parse_ValidVersion_Success()
        {
            var v = VersionModel.Parse("1.2.3-alpha+001");

            Assert.Equal(1, v.Major);
            Assert.Equal(2, v.Minor);
            Assert.Equal(3, v.Patch);
            Assert.Equal("alpha", v.Prerelease);
            Assert.Equal("001", v.Build);
        }

        /// <summary>
        /// Ensures malformed input throws FormatException.
        /// </summary>
        [Fact]
        public void Parse_Invalid_Throws()
        {
            Assert.Throws<System.FormatException>(() => VersionModel.Parse("1.2"));
        }

        /// <summary>
        /// Confirms major bump resets minor and patch.
        /// </summary>
        [Fact]
        public void Bump_Major_Works()
        {
            var v = new VersionModel(1, 2, 3);
            var bumped = v.Bump("major");

            Assert.Equal("2.0.0", bumped.ToString());
        }

        /// <summary>
        /// Confirms minor bump resets patch.
        /// </summary>
        [Fact]
        public void Bump_Minor_Works()
        {
            var v = new VersionModel(1, 2, 3);
            var bumped = v.Bump("minor");

            Assert.Equal("1.3.0", bumped.ToString());
        }

        /// <summary>
        /// Confirms patch bump increments correctly.
        /// </summary>
        [Fact]
        public void Bump_Patch_Works()
        {
            var v = new VersionModel(1, 2, 3);
            var bumped = v.Bump("patch");

            Assert.Equal("1.2.4", bumped.ToString());
        }

        /// <summary>
        /// Confirms prerelease bump increments numeric suffix.
        /// </summary>
        [Fact]
        public void Bump_Prerelease_Increments()
        {
            var v = new VersionModel(1, 2, 3, "alpha.1");
            var bumped = v.Bump("prerelease");

            Assert.Equal("1.2.3-alpha.2", bumped.ToString());
        }

        /// <summary>
        /// Ensures comparison follows SemVer precedence.
        /// </summary>
        [Fact]
        public void Compare_Works()
        {
            var a = VersionModel.Parse("1.2.3");
            var b = VersionModel.Parse("1.2.4");
            var c = VersionModel.Parse("1.3.0");

            Assert.True(a.CompareTo(b) < 0);
            Assert.True(b.CompareTo(c) < 0);
            Assert.True(c.CompareTo(a) > 0);
        }
    }
}
