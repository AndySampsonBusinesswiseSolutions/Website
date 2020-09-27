USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateFiveMinuteForecast.api')
    BEGIN
        CREATE USER [CreateFiveMinuteForecast.api] FOR LOGIN [CreateFiveMinuteForecast.api]
    END
GO

ALTER USER [CreateFiveMinuteForecast.api] WITH DEFAULT_SCHEMA=[System]
GO
