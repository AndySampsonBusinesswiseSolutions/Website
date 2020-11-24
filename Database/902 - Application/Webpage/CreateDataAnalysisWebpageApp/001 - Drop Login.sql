USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateDataAnalysisWebpageApp')
    BEGIN
        DROP LOGIN [CreateDataAnalysisWebpageApp]
    END
GO
