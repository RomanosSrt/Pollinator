using ForecastService.Contracts;
using ForecastService.Models.OpenMeteo.Responses;
using ForecastService.Models.Pollinator.QueryParams;
using ForecastService.Models.Pollinator.Settings;
using ForecastService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Tests.Forecast
{
    public class ForecastDataServiceTests
    {
        private static (ForecastDataService sut, Mock<IForecastClient> clientMock) CreateSut()
        {
            var loggerMock = new Mock<ILogger<ForecastDataService>>();

            var settings = Options.Create(new OpenMeteoSettings
            {
                PollenBaseUrl = "https://air-quality-api.open-meteo.com/v1/air-quality?"
            });

            var clientMock = new Mock<IForecastClient>();

            var sut = new ForecastDataService(loggerMock.Object, settings, clientMock.Object);

            return (sut, clientMock);
        }

        // A minimal valid PollenIndexesParams used across many tests.
        private static PollenIndexesParams ValidParams() => new()
        {
            Latitudes = [37.98f],
            Longtitudes = [23.73f],
            HourlyPollenTypes = [PollenType.olive_pollen, PollenType.grass_pollen],
            ForecastPeriod = 3
        };

        // A realistic API response object.
        private static PollenIndexesResponse SampleResponse() => new()
        {
            latitude = 37.98,
            longitude = 23.73,
            timezone = "Europe/Athens",
            hourly = new Hourly
            {
                time = ["2026-05-26T00:00", "2026-05-26T01:00"],
                olive_pollen = [12.5, 14.0],
                grass_pollen = [3.0, 4.5],
                alder_pollen = [],
                birch_pollen = [],
                mugwort_pollen = [],
                ragweed_pollen = []
            }
        };

        // ─── [Fact] tests — single scenario, no inputs ────────────────────────────

        [Fact]
        public async Task Get3DPollenIndexes_WhenClientReturnsData_WrapsItInServiceResponse()
        {
            // Arrange
            var (sut, clientMock) = CreateSut();

            // Tell the mock: whenever GetAsync is called with ANY string → return SampleResponse
            clientMock
                .Setup(c => c.GetAsync<PollenIndexesResponse>(It.IsAny<string>()))
                .ReturnsAsync(SampleResponse());

            // Act
            var result = await sut.Get3DPollenIndexes(ValidParams());

            // Assert
            Assert.True(result.IsSuccess);                        // no error message was set
            Assert.NotNull(result.Response);                      // the response object exists
            Assert.Equal(37.98, result.Response.latitude);        // data was passed through correctly
        }

        [Fact]
        public async Task Get3DPollenIndexes_WhenClientReturnsData_CallsClientExactlyOnce()
        {
            // Arrange
            var (sut, clientMock) = CreateSut();

            clientMock
                .Setup(c => c.GetAsync<PollenIndexesResponse>(It.IsAny<string>()))
                .ReturnsAsync(SampleResponse());

            // Act
            await sut.Get3DPollenIndexes(ValidParams());

            // Assert — Moq lets you verify a mock was called a specific number of times
            clientMock.Verify(
                c => c.GetAsync<PollenIndexesResponse>(It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task Get3DPollenIndexes_WhenClientThrows_ExceptionBubblesUp()
        {
            // Arrange
            var (sut, clientMock) = CreateSut();

            // Tell the mock: throw an HttpRequestException instead of returning data
            clientMock
                .Setup(c => c.GetAsync<PollenIndexesResponse>(It.IsAny<string>()))
                .ThrowsAsync(new HttpRequestException("Service unavailable"));

            // Act + Assert — Assert.ThrowsAsync verifies the expected exception is thrown
            await Assert.ThrowsAsync<HttpRequestException>(
                () => sut.Get3DPollenIndexes(ValidParams()));
        }

        [Fact]
        public async Task Get3DPollenIndexes_BuiltUrl_ContainsLatitudeValue()
        {
            // Arrange — we capture the URL the service sends to the client
            var (sut, clientMock) = CreateSut();
            string? capturedUrl = null;

            clientMock
                .Setup(c => c.GetAsync<PollenIndexesResponse>(It.IsAny<string>()))
                .Callback<string>(url => capturedUrl = url)   // intercept the argument
                .ReturnsAsync(SampleResponse());

            // Act
            await sut.Get3DPollenIndexes(ValidParams());

            // Assert — the URL must carry the latitude we passed in
            Assert.NotNull(capturedUrl);
            Assert.Contains("37.98", capturedUrl);
        }

        [Fact]
        public async Task Get3DPollenIndexes_BuiltUrl_ContainsLongitudeValue()
        {
            var (sut, clientMock) = CreateSut();
            string? capturedUrl = null;

            clientMock
                .Setup(c => c.GetAsync<PollenIndexesResponse>(It.IsAny<string>()))
                .Callback<string>(url => capturedUrl = url)
                .ReturnsAsync(SampleResponse());

            await sut.Get3DPollenIndexes(ValidParams());

            Assert.NotNull(capturedUrl);
            Assert.Contains("23.73", capturedUrl);
        }

        [Fact]
        public async Task Get3DPollenIndexes_BuiltUrl_ContainsHourlyPollenTypes()
        {
            var (sut, clientMock) = CreateSut();
            string? capturedUrl = null;

            clientMock
                .Setup(c => c.GetAsync<PollenIndexesResponse>(It.IsAny<string>()))
                .Callback<string>(url => capturedUrl = url)
                .ReturnsAsync(SampleResponse());

            await sut.Get3DPollenIndexes(ValidParams());

            Assert.NotNull(capturedUrl);
            Assert.Contains("olive_pollen", capturedUrl);
            Assert.Contains("grass_pollen", capturedUrl);
        }

        [Fact]
        public async Task Get3DPollenIndexes_BuiltUrl_StartsWithBaseUrl()
        {
            var (sut, clientMock) = CreateSut();
            string? capturedUrl = null;

            clientMock
                .Setup(c => c.GetAsync<PollenIndexesResponse>(It.IsAny<string>()))
                .Callback<string>(url => capturedUrl = url)
                .ReturnsAsync(SampleResponse());

            await sut.Get3DPollenIndexes(ValidParams());

            Assert.NotNull(capturedUrl);
            Assert.StartsWith("https://air-quality-api.open-meteo.com", capturedUrl);
        }

        [Fact]
        public async Task Get3DPollenIndexes_WhenAllPollenTypesRequested_AllAppearInUrl()
        {
            var (sut, clientMock) = CreateSut();
            string? capturedUrl = null;

            var allTypesParams = new PollenIndexesParams
            {
                Latitudes = [37.98f],
                Longtitudes = [23.73f],
                HourlyPollenTypes = Enum.GetValues<PollenType>().ToList(),
                ForecastPeriod = 3
            };

            clientMock
                .Setup(c => c.GetAsync<PollenIndexesResponse>(It.IsAny<string>()))
                .Callback<string>(url => capturedUrl = url)
                .ReturnsAsync(SampleResponse());

            await sut.Get3DPollenIndexes(allTypesParams);

            // Every pollen type must be present in the query string
            foreach (var pollenType in Enum.GetValues<PollenType>())
                Assert.Contains(pollenType.ToString(), capturedUrl);
        }

        // ─── [Theory] tests — same logic, multiple inputs ─────────────────────────

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(7)]
        public async Task Get3DPollenIndexes_BuiltUrl_ContainsForecastPeriod(int forecastDays)
        {
            // This [Theory] runs 3 times — once for each [InlineData] value.
            // It proves the forecast_days param is correctly forwarded for any value.
            var (sut, clientMock) = CreateSut();
            string? capturedUrl = null;

            var parameters = new PollenIndexesParams
            {
                Latitudes = [37.98f],
                Longtitudes = [23.73f],
                HourlyPollenTypes = [PollenType.olive_pollen],
                ForecastPeriod = forecastDays
            };

            clientMock
                .Setup(c => c.GetAsync<PollenIndexesResponse>(It.IsAny<string>()))
                .Callback<string>(url => capturedUrl = url)
                .ReturnsAsync(SampleResponse());

            await sut.Get3DPollenIndexes(parameters);

            Assert.Contains(forecastDays.ToString(), capturedUrl);
        }

        [Theory]
        [InlineData(37.98f, 23.73f)]   // Athens
        [InlineData(40.64f, 22.94f)]   // Thessaloniki
        [InlineData(35.33f, 25.13f)]   // Heraklion
        public async Task Get3DPollenIndexes_BuiltUrl_ContainsCorrectCoordinates(
            float lat, float lon)
        {
            var (sut, clientMock) = CreateSut();
            string? capturedUrl = null;

            var parameters = new PollenIndexesParams
            {
                Latitudes = [lat],
                Longtitudes = [lon],
                HourlyPollenTypes = [PollenType.olive_pollen],
                ForecastPeriod = 3
            };

            clientMock
                .Setup(c => c.GetAsync<PollenIndexesResponse>(It.IsAny<string>()))
                .Callback<string>(url => capturedUrl = url)
                .ReturnsAsync(SampleResponse());

            await sut.Get3DPollenIndexes(parameters);

            Assert.Contains(lat.ToString(), capturedUrl);
            Assert.Contains(lon.ToString(), capturedUrl);
        }
    }
}
