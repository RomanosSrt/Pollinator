using ForecastService.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Pollinator
{
    public class ServiceResponseTests
    {
        [Fact]
        public void Constructor_WithResponse_IsSuccessIsTrue()
        {
            var sr = new ServiceResponse<string>("hello");

            Assert.False(sr.IsSuccess);
            Assert.Equal("hello", sr.ErrorMessage);
            Assert.Null(sr.Response);
        }

        [Fact]
        public void Constructor_WithErrorMessage_IsSuccessIsFalse()
        {
            var sr = new ServiceResponse<string>("Something went wrong");

            Assert.False(sr.IsSuccess);
            Assert.NotNull(sr.ErrorMessage);
            Assert.Null(sr.Response);
        }

        [Theory]
        [InlineData("Network error")]
        [InlineData("Timeout")]
        [InlineData("Invalid response")]
        public void Constructor_WithDifferentErrorMessages_IsSuccessAlwaysFalse(string errorMessage)
        {
            var sr = new ServiceResponse<string>(errorMessage);

            Assert.False(sr.IsSuccess);
            Assert.Equal(errorMessage, sr.ErrorMessage);
        }

        [Fact]
        public void Constructor_WithNullResponse_IsSuccessIsStillTrue()
        {
            // IsSuccess only checks ErrorMessage — a null payload is still "success"
            var sr = new ServiceResponse<string?>((string?)null);

            Assert.True(sr.IsSuccess);
            Assert.Null(sr.Response);
        }
    }
}
