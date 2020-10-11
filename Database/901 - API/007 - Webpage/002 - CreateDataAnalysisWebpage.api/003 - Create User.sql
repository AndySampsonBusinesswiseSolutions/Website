USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateDataAnalysisWebpage.api')
    BEGIN
        CREATE USER [CreateDataAnalysisWebpage.api] FOR LOGIN [CreateDataAnalysisWebpage.api]
    END
GO

ALTER USER [CreateDataAnalysisWebpage.api] WITH DEFAULT_SCHEMA=[System]
GO
