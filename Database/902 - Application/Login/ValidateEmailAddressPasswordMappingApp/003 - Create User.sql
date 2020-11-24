USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateEmailAddressPasswordMappingApp')
    BEGIN
        CREATE USER [ValidateEmailAddressPasswordMappingApp] FOR LOGIN [ValidateEmailAddressPasswordMappingApp]
    END
GO

ALTER USER [ValidateEmailAddressPasswordMappingApp] WITH DEFAULT_SCHEMA=[System]
GO