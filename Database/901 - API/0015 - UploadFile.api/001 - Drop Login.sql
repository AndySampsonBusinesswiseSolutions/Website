USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'UploadFile.api')
    BEGIN
        DROP LOGIN [UploadFile.api]
    END
GO
