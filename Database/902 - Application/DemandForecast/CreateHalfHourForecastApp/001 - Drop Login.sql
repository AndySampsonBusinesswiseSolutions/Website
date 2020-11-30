USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateHalfHourForecastApp')
    BEGIN
        DROP LOGIN [CreateHalfHourForecastApp]
    END
GO
