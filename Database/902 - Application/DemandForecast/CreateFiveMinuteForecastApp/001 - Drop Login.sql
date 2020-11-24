USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateFiveMinuteForecastApp')
    BEGIN
        DROP LOGIN [CreateFiveMinuteForecastApp]
    END
GO
