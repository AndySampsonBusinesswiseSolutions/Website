USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateDateForecast.api')
    BEGIN
        CREATE USER [CreateDateForecast.api] FOR LOGIN [CreateDateForecast.api]
    END
GO

ALTER USER [CreateDateForecast.api] WITH DEFAULT_SCHEMA=[System]
GO
