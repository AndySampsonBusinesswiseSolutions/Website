USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitSubMeterData.api')
    BEGIN
        DROP LOGIN [CommitSubMeterData.api]
    END
GO
