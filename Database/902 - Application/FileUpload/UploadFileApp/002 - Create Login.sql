USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'UploadFileApp')
    BEGIN
       CREATE LOGIN [UploadFileApp] WITH PASSWORD=N'puFbyaAvrzMgC3MU', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
