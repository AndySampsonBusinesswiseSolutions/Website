USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateWeekForecast.api')
    BEGIN
        CREATE USER [CreateWeekForecast.api] FOR LOGIN [CreateWeekForecast.api]
    END
GO

ALTER USER [CreateWeekForecast.api] WITH DEFAULT_SCHEMA=[System]
GO
