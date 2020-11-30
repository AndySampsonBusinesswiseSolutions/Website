USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateHalfHourForecast.api')
    BEGIN
        DROP LOGIN [CreateHalfHourForecast.api]
    END
GO
