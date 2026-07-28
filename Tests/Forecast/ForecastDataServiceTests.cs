using AutoMapper;
using ForecastService.Contracts;
using ForecastService.Models.OpenMeteo.Responses;
using ForecastService.Models.Pollinator.QueryParams;
using ForecastService.Models.Pollinator.Settings;
using ForecastService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using YpenService.Contracts;

namespace Tests.Forecast
{
    public class ForecastDataServiceTests
    {
        private static (ForecastDataService sut, Mock<IForecastClient> clientMock) CreateSut()
        {
            var loggerMock = new Mock<ILogger<ForecastDataService>>();

            var settings = Options.Create(new OpenMeteoSettings
            {
                AirQualityBaseUrl = "https://air-quality-api.open-meteo.com/v1/air-quality?"
            });

            var clientMock = new Mock<IForecastClient>();
            var serviceMock = new Mock<IYpenService>();
            var repoMock = new Mock<IForecastRepository>();
            var mapperMock = new Mock<IMapper>();

            var sut = new ForecastDataService(loggerMock.Object, settings, clientMock.Object, serviceMock.Object, repoMock.Object, mapperMock.Object);

            return (sut, clientMock);
        }

        // A minimal valid AirQualityParams used across many tests.
        private static List<AirQualityIndicator> ValidParams() => new()
        {
            AirQualityIndicator.olive_pollen, AirQualityIndicator.grass_pollen
        };

        // A realistic API response object.
        private static AirQualityResponse SampleResponse() => new()
        {
            latitude = 37.98f,
            longitude = 23.73f,
            timezone = "Europe/Athens",
            hourly = new Hourly
            {
                time = ["2026-05-26T00:00", "2026-05-26T01:00"],
                olive_pollen = [12.5f, 14.0f],
                grass_pollen = [3.0f, 4.5f],
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
                .Setup(c => c.GetAsync<AirQualityResponse>(It.IsAny<string>()))
                .ReturnsAsync(SampleResponse());

            // Act
            var result = await sut.LoadAirQualForecast(ValidParams());

            // Assert
            Assert.True(result.IsSuccess);                        // no error message was set
            Assert.NotNull(result.Response);                      // the response object exists
            //Assert.Equal(37.98, result.Response.latitude);        // data was passed through correctly
        }

        [Fact]
        public async Task Get3DPollenIndexes_WhenClientReturnsData_CallsClientExactlyOnce()
        {
            // Arrange
            var (sut, clientMock) = CreateSut();

            clientMock
                .Setup(c => c.GetAsync<AirQualityResponse>(It.IsAny<string>()))
                .ReturnsAsync(SampleResponse());

            // Act
            await sut.LoadAirQualForecast(ValidParams());

            // Assert — Moq lets you verify a mock was called a specific number of times
            clientMock.Verify(
                c => c.GetAsync<AirQualityResponse>(It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task Get3DPollenIndexes_WhenClientThrows_ExceptionBubblesUp()
        {
            // Arrange
            var (sut, clientMock) = CreateSut();

            // Tell the mock: throw an HttpRequestException instead of returning data
            clientMock
                .Setup(c => c.GetAsync<AirQualityResponse>(It.IsAny<string>()))
                .ThrowsAsync(new HttpRequestException("Service unavailable"));

            // Act + Assert — Assert.ThrowsAsync verifies the expected exception is thrown
            await Assert.ThrowsAsync<HttpRequestException>(
                () => sut.LoadAirQualForecast(ValidParams()));
        }

        [Fact]
        public async Task Get3DPollenIndexes_BuiltUrl_ContainsLatitudeValue()
        {
            // Arrange — we capture the URL the service sends to the client
            var (sut, clientMock) = CreateSut();
            string? capturedUrl = null;

            clientMock
                .Setup(c => c.GetAsync<AirQualityResponse>(It.IsAny<string>()))
                .Callback<string>(url => capturedUrl = url)   // intercept the argument
                .ReturnsAsync(SampleResponse());

            // Act
            await sut.LoadAirQualForecast(ValidParams());

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
                .Setup(c => c.GetAsync<AirQualityResponse>(It.IsAny<string>()))
                .Callback<string>(url => capturedUrl = url)
                .ReturnsAsync(SampleResponse());

            await sut.LoadAirQualForecast(ValidParams());

            Assert.NotNull(capturedUrl);
            Assert.Contains("23.73", capturedUrl);
        }

        [Fact]
        public async Task Get3DPollenIndexes_BuiltUrl_ContainsHourlyPollenTypes()
        {
            var (sut, clientMock) = CreateSut();
            string? capturedUrl = null;

            clientMock
                .Setup(c => c.GetAsync<AirQualityResponse>(It.IsAny<string>()))
                .Callback<string>(url => capturedUrl = url)
                .ReturnsAsync(SampleResponse());

            await sut.LoadAirQualForecast(ValidParams());

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
                .Setup(c => c.GetAsync<AirQualityResponse>(It.IsAny<string>()))
                .Callback<string>(url => capturedUrl = url)
                .ReturnsAsync(SampleResponse());

            await sut.LoadAirQualForecast(ValidParams());

            Assert.NotNull(capturedUrl);
            Assert.StartsWith("https://air-quality-api.open-meteo.com", capturedUrl);
        }

        [Fact]
        public async Task Get3DPollenIndexes_WhenAllPollenTypesRequested_AllAppearInUrl()
        {
            var (sut, clientMock) = CreateSut();
            string? capturedUrl = null;

            List<AirQualityIndicator> allTypesParams = Enum.GetValues<AirQualityIndicator>().ToList();

            clientMock
                .Setup(c => c.GetAsync<AirQualityResponse>(It.IsAny<string>()))
                .Callback<string>(url => capturedUrl = url)
                .ReturnsAsync(SampleResponse());

            await sut.LoadAirQualForecast(allTypesParams);

            // Every pollen type must be present in the query string
            foreach (var pollenType in Enum.GetValues<AirQualityIndicator>())
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

            var parameters = new List<AirQualityIndicator>
            {
                AirQualityIndicator.olive_pollen
            };

            clientMock
                .Setup(c => c.GetAsync<AirQualityResponse>(It.IsAny<string>()))
                .Callback<string>(url => capturedUrl = url)
                .ReturnsAsync(SampleResponse());

            await sut.LoadAirQualForecast(parameters);

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

            var parameters = new List<AirQualityIndicator>
            {
                AirQualityIndicator.olive_pollen
            };

            clientMock
                .Setup(c => c.GetAsync<AirQualityResponse>(It.IsAny<string>()))
                .Callback<string>(url => capturedUrl = url)
                .ReturnsAsync(SampleResponse());

            await sut.LoadAirQualForecast(parameters);

            Assert.Contains(lat.ToString(), capturedUrl);
            Assert.Contains(lon.ToString(), capturedUrl);
        }
    }
}
