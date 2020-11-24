USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterToProfileClassDataApp')
    BEGIN
        DROP LOGIN [CommitMeterToProfileClassDataApp]
    END
GO
