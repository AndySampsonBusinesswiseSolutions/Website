USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CheckPrerequisiteAPI.api')
    BEGIN
        CREATE LOGIN [CheckPrerequisiteAPI.api] WITH PASSWORD=N'zghu5SUuzPZmbh6e', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO