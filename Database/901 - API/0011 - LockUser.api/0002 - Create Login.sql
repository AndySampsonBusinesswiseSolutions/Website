USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'LockUser.api')
    BEGIN
        CREATE LOGIN [LockUser.api] WITH PASSWORD=N'JM7!?q#g#uTyM^!v', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO