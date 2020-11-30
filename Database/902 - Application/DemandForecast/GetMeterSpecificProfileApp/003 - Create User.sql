USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'GetMeterSpecificProfileApp')
    BEGIN
        CREATE USER [GetMeterSpecificProfileApp] FOR LOGIN [GetMeterSpecificProfileApp]
    END
GO

ALTER USER [GetMeterSpecificProfileApp] WITH DEFAULT_SCHEMA=[System]
GO
