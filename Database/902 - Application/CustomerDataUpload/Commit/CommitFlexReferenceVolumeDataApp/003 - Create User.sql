USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitFlexReferenceVolumeDataApp')
    BEGIN
        CREATE USER [CommitFlexReferenceVolumeDataApp] FOR LOGIN [CommitFlexReferenceVolumeDataApp]
    END
GO

ALTER USER [CommitFlexReferenceVolumeDataApp] WITH DEFAULT_SCHEMA=[System]
GO
