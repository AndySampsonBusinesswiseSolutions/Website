USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateWeekForecast.api')
    BEGIN
        DROP LOGIN [CreateWeekForecast.api]
    END
GO
