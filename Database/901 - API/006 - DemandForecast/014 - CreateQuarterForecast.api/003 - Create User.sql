USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateQuarterForecast.api')
    BEGIN
        CREATE USER [CreateQuarterForecast.api] FOR LOGIN [CreateQuarterForecast.api]
    END
GO

ALTER USER [CreateQuarterForecast.api] WITH DEFAULT_SCHEMA=[System]
GO
