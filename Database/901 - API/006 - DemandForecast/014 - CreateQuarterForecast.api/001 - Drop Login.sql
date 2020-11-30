USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateQuarterForecast.api')
    BEGIN
        DROP LOGIN [CreateQuarterForecast.api]
    END
GO
