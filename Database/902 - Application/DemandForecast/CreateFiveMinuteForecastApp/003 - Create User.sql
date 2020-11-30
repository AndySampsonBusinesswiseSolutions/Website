USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateFiveMinuteForecastApp')
    BEGIN
        CREATE USER [CreateFiveMinuteForecastApp] FOR LOGIN [CreateFiveMinuteForecastApp]
    END
GO

ALTER USER [CreateFiveMinuteForecastApp] WITH DEFAULT_SCHEMA=[System]
GO
