USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterToProfileClassData.api')
    BEGIN
        DROP LOGIN [CommitMeterToProfileClassData.api]
    END
GO
