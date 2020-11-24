USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateWeekForecastApp')
    BEGIN
        CREATE USER [CreateWeekForecastApp] FOR LOGIN [CreateWeekForecastApp]
    END
GO

ALTER USER [CreateWeekForecastApp] WITH DEFAULT_SCHEMA=[System]
GO
