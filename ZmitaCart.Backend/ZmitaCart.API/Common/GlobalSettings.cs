namespace ZmitaCart.API.Common;

public class GlobalSettings
{
	public const string sectionName = "Global";
	public string Url { get; init; } = null!;
	public string ApplicationDbName { get; init; } = null!;
	public string Origin { get; init; } = null!;
	public string ChatHubUrl { get; init; } = null!;
	public string HealthCheckUrl { get; init; } = null!;
}