USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateMonthForecast.api')
    BEGIN
        CREATE USER [CreateMonthForecast.api] FOR LOGIN [CreateMonthForecast.api]
    END
GO

ALTER USER [CreateMonthForecast.api] WITH DEFAULT_SCHEMA=[System]
GO
