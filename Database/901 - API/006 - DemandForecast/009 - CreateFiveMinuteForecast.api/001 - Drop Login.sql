USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateFiveMinuteForecast.api')
    BEGIN
        DROP LOGIN [CreateFiveMinuteForecast.api]
    END
GO
