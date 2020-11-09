USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateDataAnalysisWebpage.api')
    BEGIN
        DROP LOGIN [CreateDataAnalysisWebpage.api]
    END
GO
