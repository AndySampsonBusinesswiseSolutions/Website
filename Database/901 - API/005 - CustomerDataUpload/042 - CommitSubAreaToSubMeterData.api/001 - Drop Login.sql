USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitSubAreaToSubMeterData.api')
    BEGIN
        DROP LOGIN [CommitSubAreaToSubMeterData.api]
    END
GO
