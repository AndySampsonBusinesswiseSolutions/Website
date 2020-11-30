USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'UploadFileApp')
    BEGIN
        CREATE USER [UploadFileApp] FOR LOGIN [UploadFileApp]
    END
GO

ALTER USER [UploadFileApp] WITH DEFAULT_SCHEMA=[System]
GO
