USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateManageCustomersWebpage.api')
    BEGIN
       CREATE LOGIN [CreateManageCustomersWebpage.api] WITH PASSWORD=N'J33e23q9DXJw5aTL', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
