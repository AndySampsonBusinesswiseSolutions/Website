USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateYearForecastApp')
    BEGIN
        CREATE USER [CreateYearForecastApp] FOR LOGIN [CreateYearForecastApp]
    END
GO

ALTER USER [CreateYearForecastApp] WITH DEFAULT_SCHEMA=[System]
GO
