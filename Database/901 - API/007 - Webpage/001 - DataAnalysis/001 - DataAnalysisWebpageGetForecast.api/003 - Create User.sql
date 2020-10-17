USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'DataAnalysisWebpageGetForecast.api')
    BEGIN
        CREATE USER [DataAnalysisWebpageGetForecast.api] FOR LOGIN [DataAnalysisWebpageGetForecast.api]
    END
GO

ALTER USER [DataAnalysisWebpageGetForecast.api] WITH DEFAULT_SCHEMA=[System]
GO
