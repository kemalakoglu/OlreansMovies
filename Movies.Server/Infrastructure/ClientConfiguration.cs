﻿namespace Movies.Server.Infrastructure;

public class ClientConfiguration
{
	public int DelayInitialConnectSeconds { get; set; } = 5;
	public ClientRetryConfig ConnectionRetry { get; set; } = new();
}

public class ClientRetryConfig
{
	public int TotalRetries { get; set; } = 25;
	public int MinDelaySeconds { get; set; } = 3;
	public int MaxDelaySeconds { get; set; } = 10;
}