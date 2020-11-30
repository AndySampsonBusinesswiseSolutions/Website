USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateForecastUsageApp')
    BEGIN
        CREATE USER [CreateForecastUsageApp] FOR LOGIN [CreateForecastUsageApp]
    END
GO

ALTER USER [CreateForecastUsageApp] WITH DEFAULT_SCHEMA=[System]
GO
