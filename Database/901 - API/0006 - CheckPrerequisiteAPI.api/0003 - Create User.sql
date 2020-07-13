USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CheckPrerequisiteAPI.api')
    BEGIN
        CREATE USER [CheckPrerequisiteAPI.api] FOR LOGIN [CheckPrerequisiteAPI.api]
    END
GO

ALTER USER [CheckPrerequisiteAPI.api] WITH DEFAULT_SCHEMA=[System]
GO