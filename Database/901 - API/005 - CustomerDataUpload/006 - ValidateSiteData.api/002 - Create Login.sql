USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateSiteData.api')
    BEGIN
       CREATE LOGIN [ValidateSiteData.api] WITH PASSWORD=N'w2fs7druC2jUCNfQ', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
