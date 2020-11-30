USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateDateForecastApp')
    BEGIN
        DROP LOGIN [CreateDateForecastApp]
    END
GO
