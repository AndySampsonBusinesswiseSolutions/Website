USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'GetProfileIdApp')
    BEGIN
       CREATE LOGIN [GetProfileIdApp] WITH PASSWORD=N'cJRxAJC7xnsP5PBF', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
