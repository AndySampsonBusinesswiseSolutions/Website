USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterData.api')
    BEGIN
        DROP LOGIN [CommitMeterData.api]
    END
GO
