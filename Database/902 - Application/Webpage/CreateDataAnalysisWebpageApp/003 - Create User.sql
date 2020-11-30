USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateDataAnalysisWebpageApp')
    BEGIN
        CREATE USER [CreateDataAnalysisWebpageApp] FOR LOGIN [CreateDataAnalysisWebpageApp]
    END
GO

ALTER USER [CreateDataAnalysisWebpageApp] WITH DEFAULT_SCHEMA=[System]
GO
