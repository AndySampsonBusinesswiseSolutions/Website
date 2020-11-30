USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateEmailAddressApp')
    BEGIN
        CREATE USER [ValidateEmailAddressApp] FOR LOGIN [ValidateEmailAddressApp]
    END
GO

ALTER USER [ValidateEmailAddressApp] WITH DEFAULT_SCHEMA=[System]
GO