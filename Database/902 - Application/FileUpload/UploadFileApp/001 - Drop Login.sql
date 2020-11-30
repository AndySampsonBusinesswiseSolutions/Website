USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'UploadFileApp')
    BEGIN
        DROP LOGIN [UploadFileApp]
    END
GO
