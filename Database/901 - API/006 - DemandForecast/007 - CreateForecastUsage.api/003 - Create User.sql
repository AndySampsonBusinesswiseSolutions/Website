USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateForecastUsage.api')
    BEGIN
        CREATE USER [CreateForecastUsage.api] FOR LOGIN [CreateForecastUsage.api]
    END
GO

ALTER USER [CreateForecastUsage.api] WITH DEFAULT_SCHEMA=[System]
GO
