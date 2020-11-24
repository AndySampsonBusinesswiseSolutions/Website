USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateCrossSheetEntityDataApp')
    BEGIN
        CREATE USER [ValidateCrossSheetEntityDataApp] FOR LOGIN [ValidateCrossSheetEntityDataApp]
    END
GO

ALTER USER [ValidateCrossSheetEntityDataApp] WITH DEFAULT_SCHEMA=[System]
GO