USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreLoginAttemptApp')
    BEGIN
        CREATE LOGIN [StoreLoginAttemptApp] WITH PASSWORD=N'mLdas-Y*x2rbnJ2e', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO