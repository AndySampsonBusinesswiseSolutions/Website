USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateFlexReferenceVolumeData.api')
    BEGIN
        CREATE USER [ValidateFlexReferenceVolumeData.api] FOR LOGIN [ValidateFlexReferenceVolumeData.api]
    END
GO

ALTER USER [ValidateFlexReferenceVolumeData.api] WITH DEFAULT_SCHEMA=[System]
GO
