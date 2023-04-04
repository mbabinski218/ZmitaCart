using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.WeatherForecasts.Commands.CreateWeather;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.UnitTests.WeatherForecast.Command;

public class CreateWeatherCommandTest
{
	private readonly Mock<IWeatherRepository> _weatherRepositoryMock;

	public CreateWeatherCommandTest()
	{
		_weatherRepositoryMock = new();
	}
	
	[Test]
	public async Task CreateWeatherCommand_ShouldReturnWeather()
	{
		// Arrange
		var weatherForecast = new CreateWeatherCommand
		{
			Day = "Monday",
			Temperature = 20
		};
		
		var weather = new Weather
		{
			Id = 1,
			Day = "Monday",
			Temperature = 20
		};
		
		_weatherRepositoryMock
			.Setup(x => x.AddWeatherAsync(It.IsAny<Weather>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(weather);
		
		var handler = new CreateWeatherHandler(_weatherRepositoryMock.Object);
		
		// Act
		var result = await handler.Handle(weatherForecast, CancellationToken.None);
		
		// Assert
		Assert.That(result, Is.EqualTo(weather));
	}
}