using FluentValidation;

namespace ZmitaCart.Application.Commands.WeatherForecastCommands.CreateWeather;

public class CreateWeatherValidator : AbstractValidator<CreateWeatherCommand>
{
	public CreateWeatherValidator()
	{
		RuleFor(w => w.Day)
			.NotEmpty()
			.WithMessage("Day is required");
		
		RuleFor(w => w.Temperature)
			.NotEmpty()
			.WithMessage("Temperature is required");
	}
}