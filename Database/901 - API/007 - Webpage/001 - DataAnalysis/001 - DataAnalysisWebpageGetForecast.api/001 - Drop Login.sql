USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'DataAnalysisWebpageGetForecast.api')
    BEGIN
        DROP LOGIN [DataAnalysisWebpageGetForecast.api]
    END
GO
