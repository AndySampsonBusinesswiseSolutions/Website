USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempFlexReferenceVolumeData.api')
    BEGIN
        DROP LOGIN [StoreUsageUploadTempFlexReferenceVolumeData.api]
    END
GO
