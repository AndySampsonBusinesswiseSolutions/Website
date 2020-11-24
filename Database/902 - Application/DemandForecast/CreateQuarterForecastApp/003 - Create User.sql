USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateQuarterForecastApp')
    BEGIN
        CREATE USER [CreateQuarterForecastApp] FOR LOGIN [CreateQuarterForecastApp]
    END
GO

ALTER USER [CreateQuarterForecastApp] WITH DEFAULT_SCHEMA=[System]
GO
