USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateYearForecast.api')
    BEGIN
        CREATE USER [CreateYearForecast.api] FOR LOGIN [CreateYearForecast.api]
    END
GO

ALTER USER [CreateYearForecast.api] WITH DEFAULT_SCHEMA=[System]
GO
