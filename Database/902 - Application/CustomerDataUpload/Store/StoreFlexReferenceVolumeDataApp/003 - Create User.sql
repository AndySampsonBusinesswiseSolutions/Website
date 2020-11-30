USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreFlexReferenceVolumeDataApp')
    BEGIN
        CREATE USER [StoreFlexReferenceVolumeDataApp] FOR LOGIN [StoreFlexReferenceVolumeDataApp]
    END
GO

ALTER USER [StoreFlexReferenceVolumeDataApp] WITH DEFAULT_SCHEMA=[System]
GO
