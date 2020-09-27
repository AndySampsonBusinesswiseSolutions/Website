USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateDateForecast.api')
    BEGIN
        DROP LOGIN [CreateDateForecast.api]
    END
GO
