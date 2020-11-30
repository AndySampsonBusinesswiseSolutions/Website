USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreFlexReferenceVolumeData.api')
    BEGIN
        CREATE USER [StoreFlexReferenceVolumeData.api] FOR LOGIN [StoreFlexReferenceVolumeData.api]
    END
GO

ALTER USER [StoreFlexReferenceVolumeData.api] WITH DEFAULT_SCHEMA=[System]
GO
