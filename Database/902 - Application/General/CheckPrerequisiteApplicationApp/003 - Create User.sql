USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CheckPrerequisiteApplicationApp')
    BEGIN
        CREATE USER [CheckPrerequisiteApplicationApp] FOR LOGIN [CheckPrerequisiteApplicationApp]
    END
GO

ALTER USER [CheckPrerequisiteApplicationApp] WITH DEFAULT_SCHEMA=[System]
GO