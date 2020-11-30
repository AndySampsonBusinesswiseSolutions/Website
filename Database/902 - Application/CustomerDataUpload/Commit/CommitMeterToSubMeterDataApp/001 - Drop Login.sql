USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterToSubMeterDataApp')
    BEGIN
        DROP LOGIN [CommitMeterToSubMeterDataApp]
    END
GO
