USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitAreaToMeterDataApp')
    BEGIN
        DROP LOGIN [CommitAreaToMeterDataApp]
    END
GO
