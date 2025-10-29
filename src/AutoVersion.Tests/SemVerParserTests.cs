// ============================================================================
// File:        SemVerParserTests.cs
// Project:     AutoVersion Lite (Test Suite)
// Version:     0.1.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Tests the SemVerParser utility for validation and safe parsing behavior.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using Xunit;
using Solcogito.AutoVersion.Core;

namespace Solcogito.AutoVersion.Tests
{
    public class SemVerParserTests
    {
        /// <summary>
        /// Valid version strings should return true.
        /// </summary>
        [Fact]
        public void IsValid_TrueForValidInput()
        {
            Assert.True(SemVerParser.IsValid("1.0.0"));
            Assert.True(SemVerParser.IsValid("2.5.9-beta"));
            Assert.True(SemVerParser.IsValid("10.3.7-alpha+build.2"));
        }

        /// <summary>
        /// Invalid strings should return false.
        /// </summary>
        [Fact]
        public void IsValid_FalseForInvalidInput()
        {
            Assert.False(SemVerParser.IsValid("1"));
            Assert.False(SemVerParser.IsValid("v1.2.3"));
            Assert.False(SemVerParser.IsValid("1.2"));
        }

        /// <summary>
        /// TryParse should return a valid model without throwing.
        /// </summary>
        [Fact]
        public void TryParse_ReturnsTrueForValid()
        {
            var success = SemVerParser.TryParse("1.2.3-alpha", out var result);
            Assert.True(success);
            Assert.Equal("1.2.3-alpha", result.ToString());
        }

        /// <summary>
        /// TryParse should safely fail for invalid input.
        /// </summary>
        [Fact]
        public void TryParse_ReturnsFalseForInvalid()
        {
            var success = SemVerParser.TryParse("oops", out var result);
            Assert.False(success);
            Assert.Null(result);
        }
    }
}
