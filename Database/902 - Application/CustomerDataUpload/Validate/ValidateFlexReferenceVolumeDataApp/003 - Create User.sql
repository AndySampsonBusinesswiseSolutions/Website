USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateFlexReferenceVolumeDataApp')
    BEGIN
        CREATE USER [ValidateFlexReferenceVolumeDataApp] FOR LOGIN [ValidateFlexReferenceVolumeDataApp]
    END
GO

ALTER USER [ValidateFlexReferenceVolumeDataApp] WITH DEFAULT_SCHEMA=[System]
GO
