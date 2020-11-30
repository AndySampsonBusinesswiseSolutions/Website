USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreLoginAttempt.api')
    BEGIN
        CREATE LOGIN [StoreLoginAttempt.api] WITH PASSWORD=N'mLdas-Y*x2rbnJ2e', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO