USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'UploadFile.api')
    BEGIN
        CREATE USER [UploadFile.api] FOR LOGIN [UploadFile.api]
    END
GO

ALTER USER [UploadFile.api] WITH DEFAULT_SCHEMA=[System]
GO
