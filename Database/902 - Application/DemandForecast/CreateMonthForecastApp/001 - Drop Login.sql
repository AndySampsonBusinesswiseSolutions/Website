USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateMonthForecastApp')
    BEGIN
        DROP LOGIN [CreateMonthForecastApp]
    END
GO
