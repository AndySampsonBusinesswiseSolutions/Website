USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateYearForecastApp')
    BEGIN
        DROP LOGIN [CreateYearForecastApp]
    END
GO
