USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempFlexReferenceVolumeData.api')
    BEGIN
        DROP LOGIN [ValidateUsageUploadTempFlexReferenceVolumeData.api]
    END
GO
