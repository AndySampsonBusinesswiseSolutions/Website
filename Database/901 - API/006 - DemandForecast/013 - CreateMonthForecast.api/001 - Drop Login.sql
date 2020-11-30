USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateMonthForecast.api')
    BEGIN
        DROP LOGIN [CreateMonthForecast.api]
    END
GO
