USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateSubMeterDataApp')
    BEGIN
        CREATE USER [ValidateSubMeterDataApp] FOR LOGIN [ValidateSubMeterDataApp]
    END
GO

ALTER USER [ValidateSubMeterDataApp] WITH DEFAULT_SCHEMA=[System]
GO
