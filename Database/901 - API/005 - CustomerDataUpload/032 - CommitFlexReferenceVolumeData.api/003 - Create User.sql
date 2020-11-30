USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitFlexReferenceVolumeData.api')
    BEGIN
        CREATE USER [CommitFlexReferenceVolumeData.api] FOR LOGIN [CommitFlexReferenceVolumeData.api]
    END
GO

ALTER USER [CommitFlexReferenceVolumeData.api] WITH DEFAULT_SCHEMA=[System]
GO
