USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitAreaToMeterData.api')
    BEGIN
        DROP LOGIN [CommitAreaToMeterData.api]
    END
GO
