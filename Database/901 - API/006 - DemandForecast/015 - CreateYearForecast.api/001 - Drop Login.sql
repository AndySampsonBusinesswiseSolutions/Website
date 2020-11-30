USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateYearForecast.api')
    BEGIN
        DROP LOGIN [CreateYearForecast.api]
    END
GO
