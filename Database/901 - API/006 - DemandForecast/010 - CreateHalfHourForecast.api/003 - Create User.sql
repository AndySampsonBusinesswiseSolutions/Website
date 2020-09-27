USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateHalfHourForecast.api')
    BEGIN
        CREATE USER [CreateHalfHourForecast.api] FOR LOGIN [CreateHalfHourForecast.api]
    END
GO

ALTER USER [CreateHalfHourForecast.api] WITH DEFAULT_SCHEMA=[System]
GO
