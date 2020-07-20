USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateUsageUploadTempFlexReferenceVolumeData.api')
    BEGIN
        CREATE USER [ValidateUsageUploadTempFlexReferenceVolumeData.api] FOR LOGIN [ValidateUsageUploadTempFlexReferenceVolumeData.api]
    END
GO

ALTER USER [ValidateUsageUploadTempFlexReferenceVolumeData.api] WITH DEFAULT_SCHEMA=[System]
GO
