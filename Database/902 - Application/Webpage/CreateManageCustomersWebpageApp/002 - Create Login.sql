USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateManageCustomersWebpageApp')
    BEGIN
       CREATE LOGIN [CreateManageCustomersWebpageApp] WITH PASSWORD=N'J33e23q9DXJw5aTL', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
