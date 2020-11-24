USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateHalfHourForecastApp')
    BEGIN
        CREATE USER [CreateHalfHourForecastApp] FOR LOGIN [CreateHalfHourForecastApp]
    END
GO

ALTER USER [CreateHalfHourForecastApp] WITH DEFAULT_SCHEMA=[System]
GO
