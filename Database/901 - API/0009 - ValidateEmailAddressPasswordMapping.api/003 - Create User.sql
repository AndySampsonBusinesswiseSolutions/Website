USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateEmailAddressPasswordMapping.api')
    BEGIN
        CREATE USER [ValidateEmailAddressPasswordMapping.api] FOR LOGIN [ValidateEmailAddressPasswordMapping.api]
    END
GO

ALTER USER [ValidateEmailAddressPasswordMapping.api] WITH DEFAULT_SCHEMA=[System]
GO