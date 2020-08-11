USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitAssetToSubMeterData.api')
    BEGIN
        DROP LOGIN [CommitAssetToSubMeterData.api]
    END
GO
