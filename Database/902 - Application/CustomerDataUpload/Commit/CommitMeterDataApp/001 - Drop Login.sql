USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterDataApp')
    BEGIN
        DROP LOGIN [CommitMeterDataApp]
    END
GO
