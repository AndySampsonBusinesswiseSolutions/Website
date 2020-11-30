USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitLineLossFactorClassToMeterDataApp')
    BEGIN
        CREATE USER [CommitLineLossFactorClassToMeterDataApp] FOR LOGIN [CommitLineLossFactorClassToMeterDataApp]
    END
GO

ALTER USER [CommitLineLossFactorClassToMeterDataApp] WITH DEFAULT_SCHEMA=[System]
GO
