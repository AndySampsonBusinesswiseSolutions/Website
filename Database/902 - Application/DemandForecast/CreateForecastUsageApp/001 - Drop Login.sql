USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateForecastUsageApp')
    BEGIN
        DROP LOGIN [CreateForecastUsageApp]
    END
GO
