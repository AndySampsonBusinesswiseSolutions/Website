USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterToSubMeterData.api')
    BEGIN
        DROP LOGIN [CommitMeterToSubMeterData.api]
    END
GO
