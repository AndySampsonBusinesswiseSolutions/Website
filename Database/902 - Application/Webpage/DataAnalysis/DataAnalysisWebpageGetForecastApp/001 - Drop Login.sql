USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'DataAnalysisWebpageGetForecastApp')
    BEGIN
        DROP LOGIN [DataAnalysisWebpageGetForecastApp]
    END
GO
