USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateQuarterForecastApp')
    BEGIN
        DROP LOGIN [CreateQuarterForecastApp]
    END
GO
