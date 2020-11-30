USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'GetMeterSpecificProfile.api')
    BEGIN
        CREATE USER [GetMeterSpecificProfile.api] FOR LOGIN [GetMeterSpecificProfile.api]
    END
GO

ALTER USER [GetMeterSpecificProfile.api] WITH DEFAULT_SCHEMA=[System]
GO
