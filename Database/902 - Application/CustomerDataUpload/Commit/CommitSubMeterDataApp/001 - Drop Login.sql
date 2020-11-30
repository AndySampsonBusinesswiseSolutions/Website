USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitSubMeterDataApp')
    BEGIN
        DROP LOGIN [CommitSubMeterDataApp]
    END
GO
