USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateMonthForecastApp')
    BEGIN
        CREATE USER [CreateMonthForecastApp] FOR LOGIN [CreateMonthForecastApp]
    END
GO

ALTER USER [CreateMonthForecastApp] WITH DEFAULT_SCHEMA=[System]
GO
