USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateForecastUsage.api')
    BEGIN
        DROP LOGIN [CreateForecastUsage.api]
    END
GO
