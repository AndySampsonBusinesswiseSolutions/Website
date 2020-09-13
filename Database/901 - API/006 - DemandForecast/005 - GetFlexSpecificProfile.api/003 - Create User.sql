USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'GetFlexSpecificProfile.api')
    BEGIN
        CREATE USER [GetFlexSpecificProfile.api] FOR LOGIN [GetFlexSpecificProfile.api]
    END
GO

ALTER USER [GetFlexSpecificProfile.api] WITH DEFAULT_SCHEMA=[System]
GO
