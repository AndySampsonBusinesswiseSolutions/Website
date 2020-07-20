USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempFlexReferenceVolumeData.api')
    BEGIN
       CREATE LOGIN [ValidateUsageUploadTempFlexReferenceVolumeData.api] WITH PASSWORD=N'h9CMbkML68NCyMNj', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
