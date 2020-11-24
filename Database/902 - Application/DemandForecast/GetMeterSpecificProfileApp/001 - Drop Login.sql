USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'GetMeterSpecificProfileApp')
    BEGIN
        DROP LOGIN [GetMeterSpecificProfileApp]
    END
GO
