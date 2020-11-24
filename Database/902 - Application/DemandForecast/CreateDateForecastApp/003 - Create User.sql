USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateDateForecastApp')
    BEGIN
        CREATE USER [CreateDateForecastApp] FOR LOGIN [CreateDateForecastApp]
    END
GO

ALTER USER [CreateDateForecastApp] WITH DEFAULT_SCHEMA=[System]
GO
