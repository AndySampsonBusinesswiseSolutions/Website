USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreUsageUploadTempFlexReferenceVolumeData.api')
    BEGIN
        CREATE USER [StoreUsageUploadTempFlexReferenceVolumeData.api] FOR LOGIN [StoreUsageUploadTempFlexReferenceVolumeData.api]
    END
GO

ALTER USER [StoreUsageUploadTempFlexReferenceVolumeData.api] WITH DEFAULT_SCHEMA=[System]
GO
