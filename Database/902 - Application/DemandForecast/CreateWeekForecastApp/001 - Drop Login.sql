USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateWeekForecastApp')
    BEGIN
        DROP LOGIN [CreateWeekForecastApp]
    END
GO
