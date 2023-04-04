using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.WeatherForecasts.Queries.GetWeather;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.UnitTests.WeatherForecast.Queries;

public class GetWeatherForecastHandlerTests
{
	private readonly Mock<IWeatherRepository> _weatherRepositoryMock;

	public GetWeatherForecastHandlerTests()
	{
		_weatherRepositoryMock = new();
	}

	[Test]
	public async Task GetWeatherForecastHandler_ShouldReturnWeather()
	{
		// Arrange
		var weatherForecast = new GetWeatherQuery
		{
			Id = 1
		};
		
		var weather = new Weather
		{
			Id = 1,
			Day = "Monday",
			Temperature = 20
		};
		
		_weatherRepositoryMock
			.Setup(x => x.GetWeatherById(It.IsAny<int>()))
			.Returns(weather);
		
		var handler = new GetWeatherHandler(_weatherRepositoryMock.Object);
		
		// Act
		var result = await handler.Handle(weatherForecast, CancellationToken.None);
		
		// Assert
		Assert.That(result, Is.EqualTo(weather));
	}
}