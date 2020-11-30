USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidatePassword.api')
    BEGIN
        CREATE LOGIN [ValidatePassword.api] WITH PASSWORD=N'7zhGuASSgmrJJFkd', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO